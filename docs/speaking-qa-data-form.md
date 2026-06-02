# Form chung dữ liệu vấn đáp luyện nói

Tài liệu này dùng để tạo data cho các bài vấn đáp tiếp theo. Mỗi bài có thể có 2 file:

- `client/public/data/speaking/{courseCode}/qa/lesson{lessonNumber}_no_image.json`
- `client/public/data/speaking/{courseCode}/qa/lesson{lessonNumber}_with_image.json`

Ví dụ:

- `client/public/data/speaking/jpd113/qa/lesson2_no_image.json`
- `client/public/data/speaking/jpd113/qa/lesson2_with_image.json`

## Quy tắc chung

- `courseCode`: dùng chữ thường theo folder, ví dụ `jpd113`, `jpd123`.
- `lessonNumber`: số bài.
- `questionMode`: dùng `NO_IMAGE` hoặc `WITH_IMAGE`.
- `questionId`: đặt ổn định, không đổi sau khi đã phát hành.
- `order`: thứ tự trong section hoặc trong tranh.
- `answerType`: đặt theo chủ đề câu trả lời, ví dụ `name_short`, `country_polite`, `job_full_question`, `birthday_month_day`, `hobby_short`.
- `relatedVocabulary`: chỉ giữ từ vựng liên quan trực tiếp tới câu hỏi đó. Câu hỏi công việc chỉ để từ công việc; câu hỏi sinh nhật chỉ để tháng/ngày/sinh nhật; câu hỏi sở thích chỉ để sở thích.
- `sampleAnswers`: nên có 1-3 câu trả lời mẫu, ưu tiên câu ngắn dễ thi vấn đáp.
- `tips`: mẹo nghe từ khóa hoặc mẹo trả lời.
- `commonMistakes`: lỗi hay gặp. Nếu chưa có, để mảng rỗng `[]`.

## Object câu hỏi chuẩn

```json
{
  "questionId": "jpd113_l2_p1_q01",
  "order": 1,
  "question": {
    "ja": "おなまえは？",
    "vi": "Tên bạn là gì?"
  },
  "answerType": "name_short",
  "sampleAnswers": [
    {
      "ja": "アンです。",
      "vi": "Tôi là An."
    }
  ],
  "grammarIds": ["g_l2_001"],
  "relatedVocabulary": [
    {
      "word": "おなまえ",
      "reading": "おなまえ",
      "meaning": "Tên, cách nói lịch sự"
    }
  ],
  "tips": [
    "Nghe thấy おなまえ thì trả lời tên."
  ],
  "commonMistakes": [
    "Trả lời quá dài khi chỉ cần nói tên + です。"
  ],
  "explanation": "Câu hỏi rút gọn để hỏi tên.",
  "relatedGrammar": [
    {
      "pattern": "Nです。",
      "meaning": "Là N.",
      "howToUse": "Dùng để trả lời ngắn bằng danh từ + です。"
    }
  ]
}
```

`grammarIds`, `explanation`, `relatedGrammar` là optional theo UI hiện tại, nhưng nên điền nếu có nội dung để sau này mở rộng hướng dẫn.

## Template không tranh

```json
{
  "courseCode": "jpd113",
  "lessonNumber": 2,
  "lessonTitle": "Bài 2 - Tên bài",
  "questionMode": "NO_IMAGE",
  "dataPurpose": "oral_exam_practice_web",
  "lessonOverview": {
    "shortSummary": "Tóm tắt bài học trong 1 câu.",
    "studentCanDo": [
      "Sinh viên có thể trả lời ..."
    ],
    "mainSkills": [
      "Nghe từ khóa chính.",
      "Trả lời ngắn đúng mẫu câu."
    ],
    "mainGrammarFocus": [
      "Mẫu ngữ pháp chính của bài."
    ],
    "examTipSummary": "Mẹo thi vấn đáp chung cho bài này."
  },
  "grammarBank": [
    {
      "grammarId": "g_l2_001",
      "pattern": "Mẫu câu",
      "meaning": "Nghĩa tiếng Việt",
      "usage": "Khi nào dùng mẫu này.",
      "example": {
        "ja": "Ví dụ tiếng Nhật.",
        "vi": "Dịch tiếng Việt."
      }
    }
  ],
  "sections": [
    {
      "sectionId": "jpd113_l2_no_image_part1",
      "sectionTitle": "Phần 1 - Chủ đề tiếng Nhật",
      "sectionViTitle": "Chủ đề tiếng Việt",
      "sectionGoal": "Mục tiêu của phần này.",
      "questionList": [
        {
          "questionId": "jpd113_l2_p1_q01",
          "order": 1,
          "question": {
            "ja": "Câu hỏi tiếng Nhật",
            "vi": "Dịch câu hỏi"
          },
          "answerType": "topic_question_type",
          "sampleAnswers": [
            {
              "ja": "Câu trả lời mẫu.",
              "vi": "Dịch câu trả lời."
            }
          ],
          "grammarIds": ["g_l2_001"],
          "relatedVocabulary": [
            {
              "word": "Từ liên quan trực tiếp",
              "reading": "Cách đọc nếu có",
              "meaning": "Nghĩa tiếng Việt"
            }
          ],
          "tips": [],
          "commonMistakes": []
        }
      ]
    }
  ]
}
```

## Template có tranh

```json
{
  "courseCode": "jpd113",
  "lessonNumber": 2,
  "lessonTitle": "Bài 2 - Câu hỏi có tranh",
  "questionMode": "WITH_IMAGE",
  "dataPurpose": "oral_exam_practice_web",
  "lessonOverview": {
    "shortSummary": "Tóm tắt bài học trong 1 câu.",
    "studentCanDo": [
      "Sinh viên có thể nhìn tranh và trả lời ..."
    ],
    "mainSkills": [
      "Quan sát thông tin trong tranh.",
      "Nghe câu hỏi và chọn đúng thông tin."
    ],
    "mainGrammarFocus": [
      "Mẫu ngữ pháp chính của bài."
    ],
    "examTipSummary": "Mẹo thi vấn đáp với tranh."
  },
  "grammarBank": [
    {
      "grammarId": "g_l2_001",
      "pattern": "Mẫu câu",
      "meaning": "Nghĩa tiếng Việt",
      "usage": "Khi nào dùng mẫu này.",
      "example": {
        "ja": "Ví dụ tiếng Nhật.",
        "vi": "Dịch tiếng Việt."
      }
    }
  ],
  "pictureSets": [
    {
      "pictureId": "jpd113_l2_picture_01",
      "pictureTitle": "Tranh 1 - Tên tranh",
      "imageUrl": "/data/speaking/jpd113/qa/lesson2_picture_01.png",
      "questions": [
        {
          "questionId": "jpd113_l2_img01_q01",
          "order": 1,
          "question": {
            "ja": "Câu hỏi dựa trên tranh",
            "vi": "Dịch câu hỏi"
          },
          "answerType": "picture_topic_question_type",
          "sampleAnswers": [
            {
              "ja": "Câu trả lời theo thông tin trong tranh.",
              "vi": "Dịch câu trả lời."
            }
          ],
          "grammarIds": ["g_l2_001"],
          "relatedVocabulary": [
            {
              "word": "Từ trong câu hỏi hoặc câu trả lời",
              "reading": "Cách đọc nếu có",
              "meaning": "Nghĩa tiếng Việt"
            }
          ],
          "tips": [
            "Nhìn đúng vị trí thông tin trong tranh."
          ],
          "commonMistakes": [],
          "explanation": "Giải thích vì sao trả lời như vậy dựa trên tranh.",
          "relatedGrammar": [
            {
              "pattern": "Mẫu câu liên quan",
              "meaning": "Nghĩa mẫu câu",
              "howToUse": "Cách dùng trong câu hỏi này."
            }
          ]
        }
      ]
    }
  ]
}
```

## Nhóm `answerType` nên dùng

Đặt `answerType` theo ý nghĩa để sau này dễ lọc, thống kê hoặc sinh dữ liệu tự động:

- Tên: `name_short`, `name_full_question`, `yes_no_identity`
- Quốc gia/quốc tịch: `country_short`, `country_polite`, `yes_no_nationality`
- Công việc/trường học: `job_short`, `job_full_question`, `yes_no_job`
- Tuổi: `age_information`, `yes_no_age`
- Sinh nhật/ngày tháng: `birthday_general`, `birthday_month_day`, `yes_no_birth_month`
- Sở thích: `hobby_short`, `hobby_full_question`, `yes_no_hobby`
- Có tranh: thêm tiền tố hoặc mô tả rõ hơn nếu cần, ví dụ `picture_job_question`, `picture_hobby_yes_no`.

## Checklist trước khi đưa data vào app

- File JSON parse được, không có comment trong JSON.
- `questionMode` đúng với cấu trúc: `NO_IMAGE` dùng `sections`, `WITH_IMAGE` dùng `pictureSets`.
- Mỗi câu có `questionId` duy nhất.
- `relatedVocabulary` không nhồi toàn bộ từ của bài, chỉ giữ từ đúng chủ đề câu hỏi.
- Với câu hỏi Yes/No, nên có mẫu trả lời `はい` và/hoặc `いいえ`.
- Với câu hỏi có tranh, `imageUrl` trỏ đúng file trong `client/public`.
- Sau khi thêm file, chạy `npm run build` trong thư mục `client`.
