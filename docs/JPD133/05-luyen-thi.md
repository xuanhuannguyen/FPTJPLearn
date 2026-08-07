# JPD133 – Mẫu biên soạn luyện thi theo cấu trúc dự án

> Nguồn import chính thức: `server/JPLearn.Infrastructure/Data/Imports/exam/jpd113.questions.json` và `jpd123.questions.json`. Không dùng trực tiếp ngân hàng cũ trong `material/Luyện Thi` nếu chưa chuyển schema.

## 1. Cấu trúc file import

```json
{
  "courseCode": "jpd133",
  "level": "N5",
  "topics": [
    { "code": "kanji", "label": "Hán tự", "orderIndex": 1 },
    { "code": "vocabulary", "label": "Từ vựng", "orderIndex": 2 },
    { "code": "grammar", "label": "Ngữ pháp", "orderIndex": 3 },
    { "code": "reading", "label": "Đọc hiểu", "orderIndex": 4 }
  ],
  "passages": [],
  "questions": []
}
```

## 2. Cấu hình ôn thi trong ứng dụng

| Trường | Nội dung |
|---|---|
| Trường | Nội dung |
|---|---|
| `courseCode` | `jpd133` |
| `level` | `N5` |
| `topics` | Danh sách chủ đề |
| `passages` | Đoạn đọc |
| `questions` | Câu hỏi |
| Thời gian blueprint hiện tại | 60 phút |
| Số câu mỗi topic trong blueprint hiện tại | 5 câu |

## 3. Cấu trúc topic

```json
{
  "code": "kanji",
  "label": "Hán tự",
  "orderIndex": 1
}
```

Topic được chuẩn hóa bằng chữ thường và thay khoảng trắng bằng `_`.

## 4. Cấu trúc question

```json
{
  "id": 1,
  "passageId": null,
  "type": "standalone",
  "topic": "kanji",
  "level": "N5",
  "questionText": "[Câu hỏi]",
  "explanation": "[Giải thích]",
  "orderIndex": 1,
  "options": []
}
```

`id`, `questionText`, `explanation`, `options` là các trường cốt lõi. `passageId` dùng cho câu đọc hiểu; `type` dùng `reading` để được chuyển thành loại passage, còn lại là `standalone`.

## 5. Cấu trúc option

```json
{
  "label": "A",
  "text": "[Đáp án]",
  "isCorrect": false
}
```

Mỗi câu phải có ít nhất một option `isCorrect: true`; thực tế nên có đúng một đáp án đúng.

## 6. Mẫu trình bày câu hỏi

**Câu hỏi:** `[ ]`

- A. `[ ]`
- B. `[ ]`
- C. `[ ]`
- D. `[ ]`

**Đáp án:** `[ ]`  
**Giải thích:** `[ ]`  
**Kiến thức liên quan:** `[mã từ/ngữ pháp/Kanji]`

## 7. Các dạng câu cần có

### Từ vựng

Chọn nghĩa, điền từ, đồng nghĩa/trái nghĩa, chọn cách dùng đúng.

### Ngữ pháp

Chọn trợ từ, chia đúng thể, sắp xếp câu, chọn đáp án hội thoại, tìm và sửa lỗi.

### Kanji

Chọn cách đọc, chọn chữ Hán từ hiragana, chọn từ ghép, điền Kanji.

### Đọc hiểu

Thông tin trực tiếp, ý chính, người/thời gian/địa điểm, suy luận, tiêu đề.

### Nghe hiểu

Chọn tranh, hành động tiếp theo, thời gian/số lượng/địa điểm, phản hồi phù hợp.

## 8. Ví dụ tham chiếu

**Kanji – JPD113**

`[誕生日] は いつですか。`

- A. たんじょうび
- B. たんじょび
- C. おたんじょう
- D. たんび

Đáp án: A. `誕生日` đọc là `たんじょうび`.

**Ngữ pháp – JPD123**

`うちからFPT大学まで＿＿＿＿です。`

- A. バスで一時間
- B. バスに一時間
- C. バスを一時間
- D. バスが一時間

Đáp án: A. Phương tiện dùng trợ từ `で`.

## 9. Ma trận nội dung

| Kỹ năng | Dễ | Trung bình | Khó | Tổng |
|---|---:|---:|---:|---:|
| Từ vựng | `[ ]` | `[ ]` | `[ ]` | `[ ]` |
| Ngữ pháp | `[ ]` | `[ ]` | `[ ]` | `[ ]` |
| Kanji | `[ ]` | `[ ]` | `[ ]` | `[ ]` |
| Đọc hiểu | `[ ]` | `[ ]` | `[ ]` | `[ ]` |
| Nghe hiểu | `[ ]` | `[ ]` | `[ ]` | `[ ]` |

## 10. Checklist trước khi import

- [ ] Đủ số câu và thời lượng.
- [ ] Mỗi câu chỉ có một đáp án đúng.
- [ ] Phương án nhiễu hợp lý.
- [ ] Có đáp án và giải thích tất cả câu.
- [ ] Câu hỏi phủ đều nội dung đã dạy.
- [ ] `courseCode` viết thường và có trong danh sách course được code hỗ trợ.
- [ ] `topics[].code` khớp với `questions[].topic`.
- [ ] `question.id` và `passage.id` không trùng trong cùng loại.
- [ ] Mỗi question có options và đúng một option đúng.
- [ ] `passageId` trỏ đến passage tồn tại nếu là câu đọc.
- [ ] `type: reading` dùng cho câu hỏi passage.
- [ ] Không đưa `answer` dạng chữ cái thay cho `isCorrect`.
- [ ] Không đưa `question` thay cho `questionText`.
- [ ] Không đưa `options[].answer` thay cho `options[].isCorrect`.
