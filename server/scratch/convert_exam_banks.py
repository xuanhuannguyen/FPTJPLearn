import json
import os
import re

def clean_text(text):
    if not text:
        return ""
    
    # Remove "Đọc đoạn văn sau và chọn đáp án đúng: 「...」" pattern
    text = re.sub(r'Đọc đoạn văn sau[^:]*:?\s*「[^」]*」', '', text)
    
    # Remove "Đọc đoạn văn sau và chọn đáp án đúng:" prefix
    text = re.sub(r'Đọc đoạn văn sau[^:]*:?\s*', '', text)
    
    # Remove "(Dựa vào đoạn văn...)" suffix
    text = re.sub(r'\(Dựa vào đoạn văn[^\)]*\)', '', text)
    text = re.sub(r'\(Dựa vào bài đọc[^\)]*\)', '', text)
    
    # Remove empty lines at the start
    text = text.strip()
    
    return text

def convert_bank(input_path, output_path, course_code, level):
    if not os.path.exists(input_path):
        print(f"Error: {input_path} not found.")
        return

    with open(input_path, 'r', encoding='utf-8') as f:
        data = json.load(f)
    
    questions = []
    passages = []
    passage_content_map = {} # content -> id
    
    topic_map = {
        "Hán tự - Cách đọc": "kanji",
        "Hán tự - Cách viết": "kanji",
        "Ngữ pháp": "grammar",
        "Ngữ pháp - Cấu trúc": "grammar",
        "Ngữ pháp - Tính từ": "grammar",
        "Từ vựng": "vocabulary",
        "Từ vựng - Nghĩa từ": "vocabulary",
        "Từ vựng - Cách dùng": "vocabulary",
        "Sắp xếp câu": "grammar",
        "Đọc hiểu": "reading",
        "Đọc hiểu - Nội dung": "reading",
        "Đọc hiểu - Từ vựng": "reading",
        "Đọc hiểu - Điền từ": "reading",
        "Đọc hiểu - Trả lời câu hỏi": "reading",
        "Đọc hiểu - N5": "reading",
        "Đọc hiểu - Chi tiết": "reading",
        "Đọc hiểu - Phương tiện": "reading",
        "Đàm thoại": "conversation",
        "Giao tiếp": "conversation",
        "Hán tự": "kanji"
    }

    for item in data:
        raw_type = item.get("type", "")
        topic = topic_map.get(raw_type, "vocabulary")
        question_text = item.get("question", "")
        explanation = item.get("explanation", "")
        p_content = item.get("passage")
        
        q_type = "standalone"
        passage_id = None
        
        # 1. Extraction logic for embedded passages
        if '【Đoạn văn】' in question_text:
            parts = re.split(r'\n\s*\n', question_text)
            if len(parts) >= 2:
                p_content = parts[0].replace('【Đoạn văn】', '').strip()
                question_text = "\n\n".join(parts[1:]).strip()
        
        # If passage is in brackets 「...」 and we don't have a p_content yet
        if not p_content and ("Đọc hiểu" in raw_type or topic == "reading"):
            match = re.search(r'「([^」]{20,})」', question_text) # At least 20 chars to be a passage
            if match:
                p_content = match.group(1).strip()
                # We'll clean the question text later using clean_text
        
        if p_content:
            p_content = p_content.strip()
            q_type = "reading"
            topic = "reading"
            
            if p_content not in passage_content_map:
                new_p_id = len(passages) + 1
                passage_content_map[p_content] = new_p_id
                passages.append({
                    "id": new_p_id,
                    "title": f"Đoạn văn {new_p_id}",
                    "content": p_content,
                    "topic": "reading",
                    "level": level
                })
            passage_id = passage_content_map[p_content]
            
            # Additional cleaning if question text contains "trong đoạn văn:"
            if "trong đoạn văn:" in question_text:
                question_text = question_text.split("trong đoạn văn:")[0] + "trong đoạn văn."

        # Apply global cleaning
        question_text = clean_text(question_text)
        
        options = []
        for opt in item.get("options", []):
            options.append({
                "label": opt.get("label"),
                "text": opt.get("text"),
                "isCorrect": str(opt.get("label")).strip().upper() == str(item.get("answer")).strip().upper()
            })
            
        questions.append({
            "id": item.get("id"),
            "passageId": passage_id,
            "type": q_type,
            "topic": topic,
            "level": level,
            "questionText": question_text,
            "explanation": explanation,
            "orderIndex": item.get("id"),
            "options": options
        })
        
    result = {
        "courseCode": course_code,
        "level": level,
        "topics": [
            {"code": "kanji", "label": "Hán tự", "orderIndex": 1},
            {"code": "vocabulary", "label": "Từ vựng", "orderIndex": 2},
            {"code": "grammar", "label": "Ngữ pháp", "orderIndex": 3},
            {"code": "conversation", "label": "Giao tiếp", "orderIndex": 4},
            {"code": "reading", "label": "Đọc hiểu", "orderIndex": 5}
        ],
        "passages": passages,
        "questions": questions
    }
    
    with open(output_path, 'w', encoding='utf-8') as f:
        json.dump(result, f, ensure_ascii=False, indent=2)
    
    print(f"Converted {course_code}: {len(questions)} questions, {len(passages)} passages.")

# Paths
jpd113_in = r'b:\FPT\Project\FPT_JPD\material\JPD113\JPD113_Full_Bank.json'
jpd113_out = r'b:\FPT\Project\FPT_JPD\server\JPLearn.Infrastructure\Data\Imports\exam\jpd113.questions.json'

jpd123_in = r'b:\FPT\Project\FPT_JPD\material\JPD123\jpd123_questions_final.json'
jpd123_out = r'b:\FPT\Project\FPT_JPD\server\JPLearn.Infrastructure\Data\Imports\exam\jpd123.questions.json'

if __name__ == "__main__":
    convert_bank(jpd113_in, jpd113_out, "jpd113", "N5")
    convert_bank(jpd123_in, jpd123_out, "jpd123", "N4")
    print("Done!")
