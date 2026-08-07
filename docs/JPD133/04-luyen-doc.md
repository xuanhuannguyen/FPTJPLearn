# JPD133 – Hướng dẫn biên soạn Luyện đọc và Vấn đáp

## 1. Phạm vi và cấu trúc base project

Trong base project, nội dung này có hai nhánh khác nhau:

| Nhánh | Mục đích | Schema chính | Nơi dùng |
|---|---|---|---|
| `reading` | Đọc câu/đoạn ngắn, có câu Nhật, nghĩa và romaji | `speaking/{courseCode}.lessons.json` | Speaking Reading |
| `qa` `NO_IMAGE` | Nghe câu hỏi và trả lời không dùng tranh | `lesson{n}_no_image.json` | Speaking Q&A |
| `qa` `WITH_IMAGE` | Nhìn tranh, nghe câu hỏi và trả lời theo tranh | `lesson{n}_with_image.json` | Speaking Q&A |

Không tạo `reading/*.json` riêng khi chưa bổ sung module mới. Với JPD133, giữ đúng schema hiện có của Speaking và QA.

## 2. Quy ước JPD133

| Trường | Giá trị |
|---|---|
| `courseCode` | `jpd133` |
| Mã bài | `JPD133-L08` đến `JPD133-L11` |
| `accessTier` | Bài 8 `free`; Bài 9–11 `premium` nếu nhập cùng chính sách Grammar/Vocabulary |
| `packageCode` Speaking | `speaking_jpd133` |
| `packageCode` QA | Dùng `jpd133` theo loader hiện có; không tự đổi thành package mới |
| `lessonType` | `reading` cho bài đọc; `qa` cho bài vấn đáp |
| `level` | `N5` nếu dữ liệu được đưa vào Exam Practice |

Reading Speaking JPD133 được tổ chức thành **100 lesson đọc độc lập**:

- Bài 8: `R001`–`R025`, lesson number `801`–`825`.
- Bài 9: `R001`–`R025`, lesson number `901`–`925`.
- Bài 10: `R001`–`R025`, lesson number `1001`–`1025`.
- Bài 11: `R001`–`R025`, lesson number `1101`–`1125`.

Mỗi lesson đọc là một chủ đề đời sống và có **10–15 câu** trong `sentences[]`. Không gộp 25 chủ đề thành một lesson lớn.

Mã nội dung khuyến nghị:

- Bài đọc: `JPD133-L08-R001`.
- Section không tranh: `JPD133-L08-R001-NI`.
- Bộ tranh: `JPD133-L08-R001-P01`.
- Câu hỏi: `JPD133-L08-R001-Q001`.
- Ngữ pháp liên kết: dùng mã đã có trong Grammar, ví dụ `JPD133-L08-G001`.

## 3. Nhánh `reading`: bài đọc Speaking

File import chuẩn:

`server/JPLearn.Infrastructure/Data/Imports/speaking/jpd133.lessons.json`

Schema cấp file:

```json
{
  "courseCode": "jpd133",
  "accessTier": "premium",
  "packageCode": "speaking_jpd133",
  "lessons": []
}
```

Schema lesson:

```json
{
  "id": 8,
  "topic": "Gia đình và bạn bè",
  "title": "家族と友達",
  "subtitle": "Nói về gia đình và người bạn quan trọng.",
  "summary": "Đoạn đọc ngắn về gia đình, nơi sống và đặc điểm của người thân.",
  "lessonType": "reading",
  "sentences": []
}
```

Schema câu đọc:

```json
{
  "jp": "私はよこはまに住んでいます。",
  "vi": "Tôi đang sống ở Yokohama.",
  "romaji": "watashi wa yokohama ni sundeimasu."
}
```

Quy tắc:

- `jp` có thể dùng markup furigana của Speaking: `[[住|す]]んでいます`.
- `vi` là bản dịch hoàn chỉnh, không để placeholder.
- `romaji` là romaji của toàn câu, không dùng hiragana thay cho romaji.
- Không đưa `reading`, `meaning`, `sampleAnswers` vào sentence của nhánh này.
- Nếu cần hiển thị cách đọc Kanji trên web, dùng markup `[[Kanji|かな]]`; seed sẽ tạo `ContentHtml`.
- Mỗi lesson phải có ít nhất một sentence và `lessonType: "reading"`.

## 4. Nhánh QA không tranh

Nguồn biên soạn có thể là Markdown hoặc JSON trong `material/Luyện Nói/Vấn đáp JPTJPLearn`. File phát hành client dùng:

`client/public/data/speaking/jpd133/qa/lesson{lessonNumber}_no_image.json`

Schema cấp file:

```json
{
  "courseCode": "jpd133",
  "lessonNumber": 8,
  "lessonTitle": "Bài 8 - Gia đình và bạn bè",
  "questionMode": "NO_IMAGE",
  "dataPurpose": "oral_exam_practice_web",
  "lessonOverview": {
    "shortSummary": "...",
    "studentCanDo": [],
    "mainSkills": [],
    "mainGrammarFocus": [],
    "examTipSummary": "..."
  },
  "grammarBank": [],
  "vocabularySets": [],
  "sections": []
}
```

Schema `sections` và câu hỏi:

```json
{
  "sectionId": "jpd133_l8_no_image_part1",
  "sectionTitle": "Phần 1 - 家族と友達",
  "sectionViTitle": "Gia đình và bạn bè",
  "sectionGoal": "Sinh viên trả lời được câu hỏi về gia đình và nơi sống.",
  "questionList": [
    {
      "questionId": "jpd133_l8_no_image_p1_q01",
      "order": 1,
      "question": {
        "ja": "どこに住んでいますか。",
        "vi": "Bạn đang sống ở đâu?"
      },
      "answerType": "location_short",
      "sampleAnswers": [
        {
          "ja": "ハノイに住んでいます。",
          "vi": "Tôi đang sống ở Hà Nội."
        }
      ],
      "grammarIds": ["JPD133-L08-G001"],
      "relatedVocabulary": [
        {
          "word": "住みます［住む］",
          "reading": "すみます",
          "meaning": "Sống, sinh sống"
        }
      ],
      "explanation": "Câu hỏi yêu cầu địa điểm cư trú; dùng Nに住んでいます.",
      "tips": ["Nghe từ khóa どこに.", "Trả lời bằng địa điểm + に住んでいます."],
      "commonMistakes": ["Dùng で thay cho に trong 住んでいます."]
    }
  ]
}
```

Bắt buộc với mỗi câu hỏi:

- `questionId`, `order`, `question.ja`, `question.vi`.
- Ít nhất một `sampleAnswers` có `ja` và `vi`.
- `answerType`, `grammarIds`, `relatedVocabulary`.
- `explanation`, `tips`, `commonMistakes`.
- `grammarIds` phải trỏ đến mẫu có thật trong Grammar JPD133.
- `relatedVocabulary` phải lấy từ `server/JPLearn.Infrastructure/Data/Imports/vocabulary/jpd133.lessons.json`, không tự tạo từ ngoài danh sách nếu không cần thiết.

## 5. Nhánh QA có tranh

File phát hành client:

`client/public/data/speaking/jpd133/qa/lesson{lessonNumber}_with_image.json`

Schema cấp file giống `NO_IMAGE`, nhưng dùng `questionMode: "WITH_IMAGE"` và thay `sections` bằng `pictureSets`:

```json
{
  "courseCode": "jpd133",
  "lessonNumber": 8,
  "lessonTitle": "Bài 8 - Gia đình và bạn bè",
  "questionMode": "WITH_IMAGE",
  "dataPurpose": "oral_exam_practice_web",
  "lessonOverview": {
    "shortSummary": "Sinh viên nhìn tranh và trả lời câu hỏi về gia đình, người và hoạt động.",
    "studentCanDo": [],
    "mainSkills": [],
    "mainGrammarFocus": [],
    "examTipSummary": "..."
  },
  "grammarBank": [],
  "pictureSets": []
}
```

Schema `pictureSets`:

```json
{
  "pictureId": "jpd133_l8_p01",
  "pictureTitle": "Gia đình trong phòng khách",
  "imageUrl": "/data/speaking/jpd133/qa/lesson8_tranh1.png",
  "questions": [
    {
      "questionId": "jpd133_l8_p01_q01",
      "order": 1,
      "question": {
        "ja": "この人はだれですか。",
        "vi": "Người này là ai?"
      },
      "answerType": "person_identification",
      "sampleAnswers": [
        {
          "ja": "母です。",
          "vi": "Là mẹ tôi."
        }
      ],
      "grammarIds": ["JPD133-L08-G004"],
      "relatedVocabulary": [
        {
          "word": "母",
          "reading": "はは",
          "meaning": "Mẹ của mình"
        }
      ],
      "explanation": "Câu hỏi yêu cầu xác định người trong tranh.",
      "tips": ["Xác định người được hỏi trước khi trả lời."],
      "commonMistakes": ["Không dùng お母さん khi nói về mẹ của mình trong ngữ cảnh này."]
    }
  ]
}
```

Quy tắc ảnh:

- `pictureId` duy nhất trong lesson.
- `imageUrl` phải là đường dẫn public thật, không dùng đường dẫn máy local.
- Ảnh lưu tại `client/public/data/speaking/jpd133/qa/`.
- Không chèn chữ tiếng Nhật do AI sinh trực tiếp vào ảnh nếu UI đã có text overlay.
- Mỗi tranh phải có ít nhất một câu hỏi và đáp án gợi ý.

## 6. Grammar bank và vocabulary bank

`grammarBank` dùng đúng schema:

```json
{
  "grammarId": "JPD133-L08-G001",
  "pattern": "Vて形 + います",
  "meaning": "đang sống / đang ở",
  "usage": "Dùng với 住みます để diễn tả trạng thái cư trú.",
  "example": {
    "ja": "私はよこはまに住んでいます。",
    "vi": "Tôi đang sống ở Yokohama."
  }
}
```

`vocabularySets` chỉ dùng khi cần nhóm từ ở phần tổng quan; câu hỏi vẫn dùng `relatedVocabulary`:

```json
{
  "setId": "jpd133_l8_vocab_01",
  "title": "Gia đình",
  "items": [
    {
      "word": "母",
      "reading": "はは",
      "meaning": "Mẹ của mình",
      "type": "Danh từ",
      "note": "Dùng khi nói về mẹ của mình."
    }
  ]
}
```

Đối chiếu nguồn:

- Grammar: `server/JPLearn.Infrastructure/Data/Imports/grammar_jpd133.json`.
- Vocabulary: `server/JPLearn.Infrastructure/Data/Imports/vocabulary/jpd133.lessons.json`.
- Không dùng mã Grammar giả hoặc từ vựng không có trong import JPD133.

## 7. Bài đọc có câu hỏi đọc hiểu

Nếu nội dung là bài đọc dùng cho Exam Practice, không dùng schema QA ở trên. Khi đó thêm vào file exam:

```json
{
  "id": 801,
  "title": "JPD133 Bài 8 - Đoạn văn 1",
  "content": "私はよこはまに住んでいます。",
  "topic": "reading",
  "level": "N5",
  "orderIndex": 1
}
```

Câu hỏi phải liên kết bằng `passageId` đúng với `passages[].id`:

```json
{
  "id": 80101,
  "passageId": 801,
  "type": "reading",
  "topic": "reading",
  "level": "N5",
  "questionText": "話している人はどこに住んでいますか。",
  "explanation": "Đoạn văn có câu 私はよこはまに住んでいます。",
  "orderIndex": 1,
  "options": [
    { "label": "A", "text": "よこはま", "isCorrect": true },
    { "label": "B", "text": "とうきょう", "isCorrect": false },
    { "label": "C", "text": "おおさか", "isCorrect": false },
    { "label": "D", "text": "なごや", "isCorrect": false }
  ]
}
```

Không đặt câu hỏi đọc hiểu vào file Speaking QA và không đặt `imageUrl` vào Exam passage nếu UI Exam chưa hỗ trợ ảnh.

## 8. Cấu trúc Markdown nguồn trước khi chuyển JSON

Mỗi lesson nên có hai phần độc lập:

```text
# JPD133-L08 – Bài 8

## A. Reading Speaking
### R001 – Đoạn đọc
- Chủ đề:
- Mục tiêu:
- Từ bắt buộc:
- Ngữ pháp bắt buộc:
#### Câu 1
- Nhật:
- Dịch:
- Romaji:

## B. Q&A không tranh
### NI001 – Phần 1
#### Q001
- Câu hỏi:
- Dịch:
- Đáp án gợi ý:
- Giải thích:
- Từ vựng liên quan:
- Ngữ pháp liên quan:
- Mẹo:
- Lỗi thường gặp:

## C. Q&A có tranh
### P001 – Tên tranh
- Image source:
- Image URL:
#### Q001
- Câu hỏi:
- Dịch:
- Đáp án gợi ý:
- Giải thích:
- Từ vựng liên quan:
- Ngữ pháp liên quan:
- Mẹo:
- Lỗi thường gặp:
```

Markdown chỉ là nguồn biên soạn. Trước khi đưa lên web phải chuyển về JSON đúng schema TypeScript/basecode, không import Markdown trực tiếp.

## 9. Checklist trước khi gửi dữ liệu

- [ ] Xác định rõ nhánh `reading`, `NO_IMAGE`, `WITH_IMAGE` hoặc Exam `reading`.
- [ ] `courseCode` luôn là `jpd133` viết thường.
- [ ] Bài 8–11 dùng lesson number đúng nguồn, không đổi thành 1–4 nếu loader cần số bài gốc.
- [ ] Mỗi câu có tiếng Nhật, bản dịch và đáp án gợi ý.
- [ ] Q&A có `questionId`, `answerType`, `sampleAnswers`, `grammarIds` và `relatedVocabulary`.
- [ ] `sampleAnswers` có ít nhất một cách trả lời đúng; nếu có nhiều cách, ghi đủ.
- [ ] Từ vựng đối chiếu với vocabulary import JPD133: word, reading, meaning.
- [ ] Grammar ID đối chiếu với grammar import JPD133.
- [ ] Câu Nhật, reading/romaji và bản dịch khớp nhau.
- [ ] Không có `[CẦN BỔ SUNG]`, placeholder hoặc đáp án rỗng trong file phát hành.
- [ ] Không dùng `Nさんさん`, tính từ như danh từ, hoặc trợ từ sai.
- [ ] `WITH_IMAGE` có ảnh tồn tại tại `imageUrl` và không dùng đường dẫn local.
- [ ] `questionId`, `pictureId`, `sectionId` không trùng trong cùng lesson.
- [ ] Q&A không tranh không có `pictureSets`.
- [ ] Q&A có tranh có `pictureSets`, mỗi set có `questions`.
- [ ] Exam reading dùng `passageId` và `type: "reading"`.
- [ ] Chạy validator JSON trước khi copy sang `client/public/data`.
