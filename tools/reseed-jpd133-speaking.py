"""Reseed only JPD133 speaking readings from the checked-in import file.

Usage: DATABASE_URL=... uv run --no-project --with 'psycopg[binary]' \
    python tools/reseed-jpd133-speaking.py
"""

import html
import json
import os
import re
import uuid
from datetime import datetime, timezone
from pathlib import Path

import psycopg


ROOT = Path(__file__).resolve().parents[1]
IMPORT = ROOT / "server/JPLearn.Infrastructure/Data/Imports/speaking/jpd133.lessons.json"
COURSE_ID = uuid.UUID("88888888-1133-0000-0000-000000000001")
RUBY_PATTERN = re.compile(r"\[\[([^|\]]+)\|([^\]]+)\]\]")
KANJI_PATTERN = re.compile(r"[\u3400-\u4dbf\u4e00-\u9fff\uf900-\ufaff々]")


def to_plain_text(japanese):
    return RUBY_PATTERN.sub(lambda match: match.group(1), japanese).replace(" ", "")


def to_ruby_html(japanese):
    parts = []
    offset = 0
    for match in RUBY_PATTERN.finditer(japanese):
        parts.append(html.escape(japanese[offset:match.start()], quote=True))
        base = html.escape(match.group(1), quote=True)
        reading = html.escape(match.group(2), quote=True)
        parts.append(f"<ruby>{base}<rt>{reading}</rt></ruby>")
        offset = match.end()
    parts.append(html.escape(japanese[offset:], quote=True))
    return "".join(parts)


def lesson_id(number):
    return uuid.UUID(f"88888888-1133-0000-0000-{number + 100:012d}")


def sentence_id(number, sentence_number):
    return uuid.UUID(f"88888888-1133-0000-0000-{number * 1000 + sentence_number:012d}")


def load_and_validate():
    data = json.loads(IMPORT.read_text(encoding="utf-8"))
    lessons = data["lessons"]
    if data["courseCode"] != "jpd133" or [x["id"] for x in lessons] != list(range(801, 826)):
        raise ValueError("Expected exactly JPD133 reading lessons 801–825")
    for lesson in lessons:
        if lesson.get("lessonType") != "reading" or len(lesson["sentences"]) != 10:
            raise ValueError(f"Invalid reading lesson {lesson['id']}")
        japanese = set()
        for sentence in lesson["sentences"]:
            jp, vi, romaji = (sentence[key].strip() for key in ("jp", "vi", "romaji"))
            if not re.search(r"[。！？]$", jp) or len(vi) < 10 or not re.match(r"[A-Za-z]", romaji):
                raise ValueError(f"Incomplete sentence in lesson {lesson['id']}")
            unannotated_text = RUBY_PATTERN.sub("", jp)
            if KANJI_PATTERN.search(unannotated_text):
                raise ValueError(f"Kanji is missing furigana in lesson {lesson['id']}: {jp}")
            if any(not match.group(2).strip() for match in RUBY_PATTERN.finditer(jp)):
                raise ValueError(f"Empty furigana in lesson {lesson['id']}: {jp}")
            plain_text = to_plain_text(jp)
            if plain_text in japanese:
                raise ValueError(f"Duplicate sentence in lesson {lesson['id']}")
            japanese.add(plain_text)
    return data


def reseed(connection, data):
    now = datetime.now(timezone.utc)
    with connection.cursor() as cursor:
        cursor.execute(
            '''INSERT INTO speaking_courses
               ("Id", "Code", "Title", "Description", "AccessTier", "PackageCode", "OrderIndex", "IsActive", "CreatedAt", "UpdatedAt")
               VALUES (%s, 'jpd133', 'JPD133', %s, %s, %s, 3, true, %s, %s)
               ON CONFLICT ("Code") DO UPDATE SET
                 "Title" = EXCLUDED."Title", "Description" = EXCLUDED."Description",
                 "AccessTier" = EXCLUDED."AccessTier", "PackageCode" = EXCLUDED."PackageCode",
                 "OrderIndex" = EXCLUDED."OrderIndex", "IsActive" = true, "UpdatedAt" = EXCLUDED."UpdatedAt"
               RETURNING "Id"''',
            (
                COURSE_ID,
                "Bài đọc luyện nói tổng hợp JPD133 Bài 8–11.",
                data["accessTier"],
                data["packageCode"],
                now,
                now,
            ),
        )
        course_id = cursor.fetchone()[0]

        for lesson in data["lessons"]:
            number = lesson["id"]
            cursor.execute(
                '''INSERT INTO speaking_lessons
                   ("Id", "CourseId", "CourseCode", "LessonNumber", "Title", "Description",
                    "AccessTier", "PackageCode", "LessonType", "OrderIndex", "IsActive", "CreatedAt", "UpdatedAt")
                   VALUES (%s, %s, 'jpd133', %s, %s, %s, %s, %s, 'reading', %s, true, %s, %s)
                   ON CONFLICT ("CourseCode", "LessonNumber") DO UPDATE SET
                     "CourseId" = EXCLUDED."CourseId", "Title" = EXCLUDED."Title",
                     "Description" = EXCLUDED."Description", "AccessTier" = EXCLUDED."AccessTier",
                     "PackageCode" = EXCLUDED."PackageCode", "LessonType" = 'reading',
                     "OrderIndex" = EXCLUDED."OrderIndex", "IsActive" = true,
                     "UpdatedAt" = EXCLUDED."UpdatedAt"
                   RETURNING "Id"''',
                (
                    lesson_id(number), course_id, number, lesson["title"],
                    f'{lesson["topic"]} - {lesson["subtitle"]}',
                    data["accessTier"], data["packageCode"], number, now, now,
                ),
            )
            db_lesson_id = cursor.fetchone()[0]
            for sentence_number, sentence in enumerate(lesson["sentences"], 1):
                jp = sentence["jp"]
                cursor.execute(
                    '''INSERT INTO speaking_sentences
                       ("Id", "LessonId", "SentenceNumber", "PlainText", "ContentHtml",
                        "MeaningVi", "Romaji", "OrderIndex", "IsActive", "CreatedAt", "UpdatedAt")
                       VALUES (%s, %s, %s, %s, %s, %s, %s, %s, true, %s, %s)
                       ON CONFLICT ("LessonId", "SentenceNumber") DO UPDATE SET
                         "PlainText" = EXCLUDED."PlainText", "ContentHtml" = EXCLUDED."ContentHtml",
                         "MeaningVi" = EXCLUDED."MeaningVi", "Romaji" = EXCLUDED."Romaji",
                         "OrderIndex" = EXCLUDED."OrderIndex", "IsActive" = true,
                         "UpdatedAt" = EXCLUDED."UpdatedAt"''',
                    (
                        sentence_id(number, sentence_number), db_lesson_id, sentence_number,
                        to_plain_text(jp), to_ruby_html(jp), sentence["vi"],
                        sentence["romaji"], sentence_number, now, now,
                    ),
                )

        cursor.execute(
            '''UPDATE speaking_lessons SET "IsActive" = false, "UpdatedAt" = %s
               WHERE "CourseCode" = 'jpd133' AND "LessonNumber" <> ALL(%s)''',
            (now, [lesson["id"] for lesson in data["lessons"]]),
        )
        cursor.execute(
            '''UPDATE speaking_sentences AS s SET "IsActive" = false, "UpdatedAt" = %s
               FROM speaking_lessons AS l WHERE s."LessonId" = l."Id"
               AND l."CourseCode" = 'jpd133'
               AND (NOT l."IsActive" OR s."SentenceNumber" > 10)''',
            (now,),
        )
        cursor.execute(
            '''SELECT count(DISTINCT l."Id"), count(s."Id"),
                      count(*) FILTER (WHERE length(s."PlainText") <= 2 OR length(s."MeaningVi") < 10)
               FROM speaking_lessons AS l
               JOIN speaking_sentences AS s ON s."LessonId" = l."Id" AND s."IsActive"
               WHERE l."CourseCode" = 'jpd133' AND l."IsActive"'''
        )
        result = cursor.fetchone()
        if result != (25, 250, 0):
            raise ValueError(f"Post-seed verification failed: {result}")
        return result


def main():
    url = os.environ.get("DATABASE_URL")
    if not url:
        raise SystemExit("DATABASE_URL is required")
    data = load_and_validate()
    with psycopg.connect(url) as connection:
        result = reseed(connection, data)
    print(f"JPD133 speaking reseeded: {result[0]} lessons, {result[1]} complete sentences")


if __name__ == "__main__":
    main()
