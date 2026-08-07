# JPD133 – Mẫu biên soạn Kanji theo cấu trúc dự án

> Kanji JPD133 dùng cùng pipeline với JPD113/JPD123. Nguồn biên soạn là `material/KANJI/jpd133_core.json` và `material/KANJI/jpd133_vocab.json`; dữ liệu server đồng bộ qua `server/JPLearn.Infrastructure/Data/Imports/kanji/jpd133.json`.

## 1. Nguồn dữ liệu hiện tại

| Thành phần | Nguồn | Vai trò |
|---|---|---|
| Danh sách bài/Kanji | `material/KANJI/jpd123_core.json` | `lessonNumber`, `lessonTitle`, `level`, `kanjiItems` |
| Từ vựng Kanji | `material/KANJI/jpd123_vocab.json` | Mảng `vocabulary` theo `lessonNumber` |
| ID và dữ liệu nét | `server/.../KanjiSeedData.cs` | Đối chiếu `Character`, ID, `StrokeDataJson`, `ComponentMapJson` |
| Dữ liệu client sinh ra | `client/public/data/kanji/{course}/lessons/*.json` | Dữ liệu hiển thị tĩnh |
| Dữ liệu vẽ local | `client/public/data/kanji/strokes-jp/{character}.json` | `strokes`, `medians`, số nét cho Study Mode |

## 2. Cấu trúc `*_core.json`

```json
[
  {
    "level": "N5",
    "lessonNumber": 1,
    "lessonTitle": "Giới thiệu bản thân và Trường học",
    "accessTier": "free",
    "kanjiItems": [
      {
        "character": "私",
        "hanViet": "TƯ",
        "meaning": "Tôi, riêng tư",
        "strokeCount": 7,
        "kunReading": "わたし、わたくし",
        "onReading": "シ",
        "mnemonic": "Giữ bó lúa...",
        "orderIndex": 1
      }
    ]
  }
]
```

## 3. Thông tin bài

| Trường | Nội dung |
|---|---|
| Trường | Nội dung |
|---|---|
| `level` | `N5`, `N4`, `N3`, `N2` hoặc `N1` |
| `lessonNumber` | Số nguyên, ví dụ `1` |
| `lessonTitle` | Tên bài |
| `accessTier` | `free` hoặc `premium` |
| Mục tiêu giảng dạy | `[đọc/viết/nhận diện/từ ghép]` |

## 4. Trường của một `kanjiItem`

### JPD133-L01-K001 – 私

| Trường | Nội dung |
|---|---|
| Hán Việt | TƯ |
| Nghĩa | Tôi; riêng tư |
| JLPT | N5 |
| Số nét | 7 |
| Âm On | シ |
| Âm Kun | わたし、わたくし |
| Thành phần | 禾 + 厶 |
| Từ ghép 1 | 私たち（わたしたち）– chúng tôi |
| Từ ghép 2 | 私の名前（わたしのなまえ）– tên của tôi |
| Kanji dễ nhầm | `[điền]` |

Các trường được hệ thống sử dụng để tạo dữ liệu client: `character`, `hanViet`, `meaning`, `strokeCount`, `kunReading`, `onReading`, `mnemonic`, `strokeSvg` (nếu có), `strokeDataJson`, `componentMapJson`, `orderIndex`.

**Mẹo nhớ:** Giữ bó lúa (禾) cho riêng mình (厶) tạo thành chữ tôi (私).

**Câu ví dụ:** `私は学生です。`  
`わたしはがくせいです。` – Tôi là học sinh.

**Luyện viết:** `私 私 私 私 私` – số lần: `[ ]`

## 5. Danh sách Kanji bài học

| STT | Mã | Kanji | Hán Việt | Nghĩa | Âm On | Âm Kun | Số nét | Từ ghép bắt buộc |
|---:|---|---|---|---|---|---|---:|---|
| 1 | JPD133-L01-K001 | `[ ]` | `[ ]` | `[ ]` | `[ ]` | `[ ]` | `[ ]` | `[ ]` |
| 2 | JPD133-L01-K002 | `[ ]` | `[ ]` | `[ ]` | `[ ]` | `[ ]` | `[ ]` | `[ ]` |

## 6. Cấu trúc `*_vocab.json`

```json
[
  {
    "lessonNumber": 1,
    "vocabulary": [
      {
        "word": "私",
        "reading": "わたし",
        "meaning": "tôi",
        "exampleJapanese": "私は学生です。",
        "exampleReading": "わたしはがくせいです。",
        "exampleMeaning": "Tôi là học sinh."
      }
    ]
  }
]
```

Từ vựng Kanji hiện không có `wordType`, `notes` hoặc `orderIndex` bắt buộc như từ vựng khóa học; thứ tự được lấy theo vị trí trong mảng nếu không có trường riêng.

## 7. Bài tập Kanji

### A. Chọn cách đọc

`[Kanji]`

- A. `[ ]`
- B. `[ ]`
- C. `[ ]`
- D. `[ ]`

Đáp án: `[ ]` – Giải thích: `[ ]`

### B. Chọn chữ Hán đúng

`[Câu có từ hiragana]`

- A. `[ ]`
- B. `[ ]`
- C. `[ ]`
- D. `[ ]`

Đáp án: `[ ]` – Giải thích: `[ ]`

### C. Điền Kanji

`[Câu tiếng Nhật]`  
Đáp án: `[ ]` – Dịch: `[ ]`

## 8. Checklist trước khi cập nhật Kanji

- [ ] Nghĩa, âm On/Kun và số nét chính xác.
- [ ] Có ít nhất 2 từ ghép/cụm từ.
- [ ] Có mẹo nhớ và câu ví dụ.
- [ ] Có bài đọc Kanji và chọn chữ Hán.
- [ ] `lessonNumber` khớp giữa file core và file vocab.
- [ ] `character` khớp với Kanji tương ứng trong `KanjiSeedData.cs`.
- [ ] Không tự tạo ID Kanji nếu chưa cập nhật seed/generator.
- [ ] Dữ liệu nét và component là JSON string hợp lệ nếu có.
- [ ] Nếu thêm JPD133, phải cập nhật mảng course trong `tools/generate-static-data.mjs`.
