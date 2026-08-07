# JPD133 – Mẫu biên soạn từ vựng theo cấu trúc dự án

> Quan trọng: file nhập chính thức của từ vựng phải có cấu trúc tương đương `server/JPLearn.Infrastructure/Data/Imports/vocabulary/jpd113.lessons.json` và `jpd123.lessons.json`. Không dùng trực tiếp cấu trúc trong `material/VOCAB/*.json` để import.

## 1. Cấu trúc file import chính thức

```json
{
  "courseCode": "jpd133",
  "title": "Tiếng Nhật Sơ Cấp 3",
  "description": "Từ vựng cho khóa JPD133",
  "lessons": [
    {
      "id": 1,
      "title": "3-1: [Tên bài]",
      "description": "[Mô tả bài học]",
      "accessTier": "free",
      "packageCode": "jpd133",
      "orderIndex": 1,
      "items": []
    }
  ]
}
```

## 2. Thông tin cấp khóa

| Trường | Nội dung |
|---|---|
| Trường JSON | Bắt buộc | Ví dụ |
|---|---|---|
| `courseCode` | Có | `jpd133` |
| `title` | Có | `Tiếng Nhật Sơ Cấp 3` |
| `description` | Không bắt buộc nhưng nên có | `Từ vựng cho khóa JPD133` |

## 3. Cấu trúc một lesson

| Trường JSON | Bắt buộc | Kiểu | Ví dụ/ghi chú |
|---|---|---|---|
| `id` | Có | Số nguyên | `1`, `2`, `3`; không dùng `1-1` |
| `title` | Có | Chuỗi | `3-1: Đời sống hằng ngày` |
| `description` | Không bắt buộc | Chuỗi/null | Mô tả ngắn |
| `accessTier` | Không bắt buộc | Chuỗi | `free` hoặc `premium`; mặc định `free` |
| `packageCode` | Không bắt buộc | Chuỗi/null | Thường là `jpd133` |
| `orderIndex` | Không bắt buộc | Số nguyên/null | Nếu bỏ trống, hệ thống dùng `id` |
| `items` | Có | Mảng | Phải có ít nhất một phần tử |

## 4. Cấu trúc một vocabulary item

| Trường JSON | Bắt buộc | Kiểu | Giới hạn/ghi chú |
|---|---|---|---|
| `id` | Không bắt buộc | GUID string | Nếu có phải là GUID hợp lệ; nên để hệ thống tự sinh |
| `word` | Có | Chuỗi | Tối đa 100 ký tự; không trùng trong cùng lesson |
| `reading` | Có | Chuỗi | Tối đa 200 ký tự |
| `wordType` | Có | Chuỗi | Tối đa 100 ký tự |
| `meaning` | Có | Chuỗi | Tối đa 500 ký tự |
| `exampleJapanese` | Không bắt buộc | Chuỗi/null | Tối đa 1000 ký tự |
| `exampleReading` | Không bắt buộc | Chuỗi/null | Tối đa 1000 ký tự |
| `exampleMeaning` | Không bắt buộc | Chuỗi/null | Tối đa 1000 ký tự |
| `notes` | Không bắt buộc | Chuỗi/null | Tối đa 1000 ký tự |
| `accessTierOverride` | Không bắt buộc | Chuỗi/null | Ghi đè quyền truy cập của lesson |
| `packageCodeOverride` | Không bắt buộc | Chuỗi/null | Ghi đè package của lesson |
| `orderIndex` | Không bắt buộc | Số nguyên/null | Thứ tự trong lesson; nếu bỏ trống dùng vị trí mảng + 1 |

### Mẫu item nhập được

```json
{
  "word": "私",
  "reading": "わたし",
  "wordType": "Danh từ",
  "meaning": "Tôi",
  "exampleJapanese": "私は学生です。",
  "exampleReading": "わたしはがくせいです。",
  "exampleMeaning": "Tôi là học sinh.",
  "notes": "Dùng làm đại từ ngôi thứ nhất.",
  "orderIndex": 1
}
```

Không thêm các trường sau vào item import nếu chưa sửa code seed: `hánViệt`, `jlpt`, `sốNét`, `âmOn`, `âmKun`, `mẹoNhớ`, `level`, `mãNộiBộ`.

## 5. Bảng nhập nội dung

| STT | `word` | `reading` | `wordType` | `meaning` | `exampleJapanese` | `exampleReading` | `exampleMeaning` | `notes` | `orderIndex` |
|---:|---|---|---|---|---|---|---|---|---:|
| 1 | 私 | わたし | Danh từ | Tôi | 私は学生です。 | わたしはがくせいです。 | Tôi là học sinh. |  | 1 |
| 2 | 車 | くるま | Danh từ | Ô tô | 車で行きます。 | くるまでいきます。 | Đi bằng ô tô. | Dùng で cho phương tiện. | 2 |
| 3 | `[ ]` | `[ ]` | `[ ]` | `[ ]` | `[ ]` | `[ ]` | `[ ]` | `[ ]` | 3 |

## 6. Thông tin biên soạn mở rộng (không đưa thẳng vào import)

### Từ: 私

| Trường | Nội dung |
|---|---|
| Cách đọc | わたし |
| Loại từ | Danh từ/đại từ |
| Nghĩa | Tôi, mình |
| Cấp độ | N5 / `[điền]` |
| Trợ từ thường gặp | は, の, が |
| Từ ghép/cụm | 私の名前 |
| Từ dễ nhầm | `[điền]` |
| Ghi chú lịch sự | `[điền]` |
| Hán Việt | TƯ |
| JLPT | N5 |
| Âm On/Kun | シ / わたし、わたくし |
| Mẹo nhớ | `[điền]` |

**Ví dụ:** `私は学生です。`  
**Cách đọc:** `わたしはがくせいです。`  
**Dịch:** Tôi là học sinh.  
**Phân tích:** `私` làm chủ đề, đi với trợ từ `は`.

### Ví dụ tham chiếu JPD123 – 車

`車で行きます。` → `くるまでいきます。` → Đi bằng ô tô.  
Ghi chú: phương tiện đi với trợ từ `で`.

## 7. Bài tập

### A. Nối từ và nghĩa

| Từ | Nghĩa |
|---|---|
| 1. `[từ]` | a. `[nghĩa]` |
| 2. `[từ]` | b. `[nghĩa]` |

Đáp án: `[1-__, 2-__]`

### B. Chọn cách đọc

`[Kanji/từ]`

- A. `[ ]`
- B. `[ ]`
- C. `[ ]`
- D. `[ ]`

Đáp án: `[ ]` – Giải thích: `[ ]`

### C. Điền từ vào câu

`[Câu có chỗ trống]`  
Đáp án: `[ ]` – Dịch: `[ ]`

## 8. Quy tắc kiểm tra trước khi import

- [ ] `courseCode` viết thường: `jpd133`.
- [ ] Mỗi `lesson.id` là số nguyên dương và không trùng.
- [ ] Mỗi lesson có `title` và ít nhất một item.
- [ ] Mỗi item có `word`, `reading`, `wordType`, `meaning`.
- [ ] Không trùng `word` trong cùng lesson.
- [ ] `orderIndex` của lesson/item là số nguyên nếu có.
- [ ] `accessTier` chỉ dùng `free` hoặc `premium`.
- [ ] Nếu dùng `id` cho item, đó phải là GUID hợp lệ.
- [ ] Các trường mở rộng chỉ giữ trong tài liệu giảng dạy, không đưa vào JSON import.
- [ ] Kiểm tra lỗi chính tả trong `word`, `reading`, câu ví dụ và bản dịch.
