# JPD133 – Mẫu biên soạn luyện đọc theo cấu trúc dự án

> Dự án hiện không có file import luyện đọc độc lập. Luyện đọc được lưu trong `server/JPLearn.Infrastructure/Data/Imports/exam/jpd113.questions.json` hoặc `jpd123.questions.json`, tại mảng `passages`; câu hỏi liên kết bằng `passageId`.

## 1. Cấu trúc passage trong file luyện thi

```json
{
  "id": 1,
  "title": "Đoạn văn 1",
  "content": "はじめまして、わたしはすずきです。",
  "topic": "reading",
  "level": "N5",
  "orderIndex": 1
}
```

## 2. Trường bắt buộc/khuyến nghị

| Trường | Nội dung |
|---|---|
| Trường | Bắt buộc | Ghi chú |
|---|---|---|
| `id` | Có | Số nguyên duy nhất trong file |
| `title` | Có | Tên đoạn |
| `content` | Có | Nội dung tiếng Nhật |
| `topic` | Có | Thường dùng `reading` |
| `level` | Không bắt buộc | Mặc định theo cấp file |
| `orderIndex` | Không bắt buộc | Schema seed có hỗ trợ |

## 3. Thông tin sư phạm nội bộ

| Từ khóa | Cách đọc | Nghĩa | Vai trò trong bài |
|---|---|---|---|
| `[ ]` | `[ ]` | `[ ]` | `[ ]` |

Dự đoán nội dung từ tiêu đề: `[ ]`  
Cần chú ý: `[thời gian/địa điểm/người/số lượng/lý do]`

## 4. Đoạn văn import

```text
[Đoạn văn tiếng Nhật]
```

## 5. Bản hỗ trợ đọc và dịch

```text
[Đoạn văn có hiragana/furigana nếu cần]
```

```text
[Bản dịch tiếng Việt]
```

## 6. Phân tích

| Câu | Từ vựng | Ngữ pháp | Ý chính |
|---|---|---|---|
| 1 | `[ ]` | `[ ]` | `[ ]` |
| 2 | `[ ]` | `[ ]` | `[ ]` |

## 7. Câu hỏi liên kết với passage

Trong file exam, câu hỏi đọc hiểu phải có `passageId` trùng với `passages[].id` ban đầu. Khi `type` là `reading`, hệ thống chuyển thành loại câu hỏi passage.

### Câu 1 – Thông tin trực tiếp

`[Câu hỏi]`

- A. `[ ]`
- B. `[ ]`
- C. `[ ]`
- D. `[ ]`

Đáp án: `[ ]`  
Giải thích: `[Đoạn/câu chứa thông tin và lý do đáp án khác sai]`

### Câu 2 – Suy luận

`[Câu hỏi]`

- A. `[ ]`
- B. `[ ]`
- C. `[ ]`
- D. `[ ]`

Đáp án: `[ ]`  
Giải thích: `[ ]`

## 8. Gợi ý theo JPD123

Với chủ đề phương hướng/phương tiện, có thể dùng các từ `北`, `南`, `駅`, `車`, `電車` và hỏi: đi bằng gì, địa điểm ở đâu, mất bao lâu.

## 9. Checklist

- [ ] Độ khó phù hợp kiến thức đã học.
- [ ] Đoạn văn đủ thông tin để trả lời.
- [ ] Mỗi câu chỉ có một đáp án đúng.
- [ ] Có câu hỏi trực tiếp và suy luận.
- [ ] Có bản dịch và giải thích.
- [ ] `passage.id` là số nguyên và không trùng.
- [ ] `topic` viết nhất quán, nên dùng `reading`.
- [ ] Câu hỏi liên kết dùng đúng `passageId`, không dùng ID GUID client tự sinh.
- [ ] Không tạo file `reading/*.json` riêng nếu chưa bổ sung module mới.
