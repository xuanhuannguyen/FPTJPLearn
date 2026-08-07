# JPD133 – Mẫu biên soạn ngữ pháp theo cấu trúc dự án

> Nguồn import chính thức: `server/JPLearn.Infrastructure/Data/Imports/grammar_jpd113.json` và `grammar_jpd123.json`.

## 1. Cấu trúc file import

```json
{
  "courseCode": "jpd133",
  "level": "N5",
  "lessons": [
    {
      "lessonNumber": 1,
      "title": "[Tên bài]",
      "description": "[Mô tả]",
      "accessTier": "free",
      "packageCode": "jpd133",
      "orderIndex": 1,
      "patterns": []
    }
  ]
}
```

## 2. Trường cấp bài

| Trường | Bắt buộc | Ghi chú |
|---|---|---|
| `courseCode` | Có | Viết thường, ví dụ `jpd133` |
| `level` | Có | `N5`–`N1` |
| `lessonNumber` | Có | Số nguyên |
| `title` | Có | Tên bài |
| `description` | Không bắt buộc | Mô tả |
| `accessTier` | Không bắt buộc | `free`/`premium` |
| `packageCode` | Không bắt buộc | Package khóa học |
| `orderIndex` | Không bắt buộc | Thứ tự hiển thị |
| `patterns` | Có | Mảng mẫu ngữ pháp |

## 3. Trường của một pattern

### JPD133-L01-G001 – `[Tên mẫu câu]`

| Trường | Nội dung |
|---|---|
| Mẫu câu | `[NはAです]` |
| Ý nghĩa | `[ ]` |
| Dịch tự nhiên | `[ ]` |
| Điều kiện dùng | `[ ]` |
| Trợ từ liên quan | `[は/が/を/に/で]` |
| Khẳng định | `[ ]` |
| Phủ định | `[ ]` |
| Quá khứ | `[ ]` |
| Nghi vấn | `[ ]` |
| Mẫu dễ nhầm | `[ ]` |
| Lỗi thường gặp | `[ ]` |

Các trường import chính xác: `pattern`, `title`, `meaning`, `structure`, `usageScope`, `formation`, `notes`, `orderIndex`, `examples`, `exercises`.

`tagsJson`, `accessTierOverride`, `packageCodeOverride` có trong entity nhưng seed hiện tại không đọc từ import; không thêm để mong hệ thống tự xử lý.

**Cấu trúc:** `[Thành phần 1] + [trợ từ] + [thành phần 2]`

**Ví dụ 1:** `[Câu tiếng Nhật]`  
**Cách đọc:** `[ ]`  
**Dịch:** `[ ]`  
**Phân tích:** `[ ]`

**Ví dụ 2 – hội thoại:**  
A: `[ ]`  
B: `[ ]`  
Dịch: `[ ]`

## 4. Trường của một example

```json
{
  "japanese": "私は学生です。",
  "reading": "わたしはがくせいです。",
  "meaning": "Tôi là sinh viên.",
  "note": "[Ghi chú nếu có]",
  "orderIndex": 1
}
```

`examples` là dữ liệu rất quan trọng vì hệ thống tự sinh bài tập `ja_to_vi`, `vi_to_ja` và `arrange` từ các ví dụ này.

## 5. Bài tập tự động sinh từ mỗi example

Với một example như sau:

```json
{
  "japanese": "車で行きます。",
  "reading": "くるまでいきます。",
  "meaning": "Tôi đi bằng ô tô。",
  "orderIndex": 1
}
```

Hệ thống tự tạo tối đa ba bài:

| `exerciseType` | Prompt | Đáp án đúng | Cách học viên làm |
|---|---|---|---|
| `ja_to_vi` | `車で行きます。` | `Tôi đi bằng ô tô。` | Nhìn tiếng Nhật, dịch sang tiếng Việt |
| `vi_to_ja` | `Tôi đi bằng ô tô。` | `車で行きます。` | Nhìn tiếng Việt, viết tiếng Nhật |
| `arrange` | `Tôi đi bằng ô tô。` | `車で行きます。` | Sắp xếp các mảnh câu |

Mỗi pattern nên có ít nhất 3–5 `examples` chất lượng. Nếu chỉ nhập lý thuyết mà không có ví dụ, hệ thống sẽ không tạo đủ bài tập.

## 6. Bài tập tùy chọn khai báo trực tiếp

Dùng `patterns[].exercises` khi cần bài tập riêng, câu có đáp án nhiễu, nhiều đáp án chấp nhận được hoặc sắp xếp câu đặc biệt.

### A. Dịch Việt → Nhật (`vi_to_ja`)

```json
{
  "exerciseType": "vi_to_ja",
  "prompt": "Tôi đi bằng tàu điện.",
  "promptReading": null,
  "expectedAnswer": "電車で行きます。",
  "acceptableAnswers": ["でんしゃでいきます。"],
  "hint": "Phương tiện dùng trợ từ で.",
  "explanation": "電車 + で + 行きます.",
  "templateText": null,
  "options": [],
  "correctOrder": [],
  "starPosition": null,
  "starAnswer": null,
  "orderIndex": 1
}
```

### B. Dịch Nhật → Việt (`ja_to_vi`)

```json
{
  "exerciseType": "ja_to_vi",
  "prompt": "電車で行きます。",
  "promptReading": "でんしゃでいきます。",
  "expectedAnswer": "Tôi đi bằng tàu điện.",
  "acceptableAnswers": [],
  "hint": "Chú ý trợ từ で.",
  "explanation": "で chỉ phương tiện.",
  "orderIndex": 2
}
```

### C. Sắp xếp câu (`arrange`)

```json
{
  "exerciseType": "arrange",
  "prompt": "Tôi đi bằng tàu điện.",
  "expectedAnswer": "電車で行きます。",
  "options": ["行きます。", "電車", "で"],
  "correctOrder": ["電車", "で", "行きます。"],
  "hint": "Phương tiện + で + động từ.",
  "explanation": "電車で行きます。",
  "orderIndex": 3
}
```

`options` là các mảnh hiển thị; `correctOrder` là thứ tự đúng; `expectedAnswer` là câu hoàn chỉnh.

## 7. Mẫu một pattern hoàn chỉnh có bài tập

```json
{
  "pattern": "Nで行きます／来ます／帰ります。",
  "title": "Đi bằng phương tiện",
  "meaning": "Đi/đến/về bằng N",
  "structure": "N（phương tiện）+ で + 行きます／来ます／帰ります",
  "usageScope": "Dùng để nói phương tiện di chuyển.",
  "formation": "Tên phương tiện + で + động từ di chuyển.",
  "notes": "Không dùng に để chỉ phương tiện.",
  "orderIndex": 1,
  "examples": [
    {
      "japanese": "車で行きます。",
      "reading": "くるまでいきます。",
      "meaning": "Tôi đi bằng ô tô.",
      "orderIndex": 1
    },
    {
      "japanese": "ひこうきで行きます。",
      "reading": "ひこうきでいきます。",
      "meaning": "Tôi đi bằng máy bay.",
      "orderIndex": 2
    }
  ],
  "exercises": [
    {
      "exerciseType": "vi_to_ja",
      "prompt": "Tôi đi bằng tàu điện.",
      "expectedAnswer": "電車で行きます。",
      "acceptableAnswers": ["でんしゃでいきます。"],
      "hint": "Dùng trợ từ で.",
      "explanation": "電車で行きます。",
      "orderIndex": 1
    }
  ]
}
```

## 8. Schema exercise đầy đủ để đối chiếu

```json
{
  "exerciseType": "vi_to_ja",
  "prompt": "Tôi là sinh viên.",
  "promptReading": null,
  "expectedAnswer": "私は学生です。",
  "acceptableAnswers": ["わたしはがくせいです。"],
  "hint": "[Gợi ý]",
  "explanation": "[Giải thích]",
  "templateText": null,
  "options": [],
  "correctOrder": [],
  "starPosition": null,
  "starAnswer": null,
  "orderIndex": 1
}
```

## 9. Ví dụ tham chiếu JPD113/JPD123

### `NをVます` – JPD113

Ý nghĩa: thực hiện hành động lên một đối tượng.  
Ví dụ: `私は毎朝、コーヒーを飲みます。`  
Dịch: Mỗi sáng tôi uống cà phê.  
Phân tích: `コーヒー` là tân ngữ, dùng trợ từ `を`.

### `Nで行きます／来ます／帰ります` – JPD123

Ý nghĩa: đi/đến/về bằng phương tiện N.  
Ví dụ: `ひこうきで行きます。`  
Dịch: Tôi đi bằng máy bay.  
Lỗi cần tránh: không dùng `に` để chỉ phương tiện.

### `N1からN2までどのくらいですか` – JPD123

Ví dụ: `うちからFPT大学までどのくらいですか。`  
Dịch: Từ nhà đến Đại học FPT mất bao lâu?

## 10. Bảng phân biệt

| Mẫu | Ý nghĩa | Ví dụ | Ghi nhớ |
|---|---|---|---|
| `[Nで]` | `[phương tiện/địa điểm hành động]` | `[ ]` | `[ ]` |
| `[Nに]` | `[thời điểm/nơi tồn tại/mục đích]` | `[ ]` | `[ ]` |
| `[Nを]` | `[tân ngữ]` | `[ ]` | `[ ]` |

## 11. Bài tập trình bày cho học viên

### A. Chọn đáp án

`[Câu có chỗ trống]`

- A. `[ ]`
- B. `[ ]`
- C. `[ ]`
- D. `[ ]`

Đáp án: `[ ]`  
Giải thích: `[Vì sao đúng và vì sao đáp án khác sai]`

### B. Sắp xếp câu

Các từ: `[ ] / [ ] / [ ] / [ ]`  
Đáp án: `[ ]`

### C. Dịch Việt – Nhật

Đề: `[ ]`  
Đáp án tham khảo: `[ ]`

### D. Chọn trợ từ

`うち＿＿＿大学までバスで一時間です。`

- A. は
- B. から
- C. を
- D. が

Đáp án: B  
Giải thích: `N1からN2まで` nghĩa là “từ N1 đến N2”.

### E. Chia dạng câu

Đề: Chuyển `[車で行きます。]` sang dạng phủ định hoặc quá khứ theo mục tiêu bài.  
Đáp án: `[ ]`  
Giải thích: `[ ]`

## 12. Checklist

- [ ] Có cấu trúc và cách dùng rõ ràng.
- [ ] Có khẳng định, phủ định hoặc biến đổi cần thiết.
- [ ] Có tối thiểu 2 ví dụ.
- [ ] Có phân tích trợ từ và lỗi thường gặp.
- [ ] Bài tập kiểm tra đúng mẫu vừa dạy.
- [ ] Mỗi pattern có tối thiểu 3 ví dụ để hệ thống tự sinh bài.
- [ ] Có đủ `ja_to_vi`, `vi_to_ja`, `arrange` nếu bài cần luyện toàn diện.
- [ ] Bài `vi_to_ja` có `expectedAnswer` và `acceptableAnswers` khi cần.
- [ ] Bài `arrange` có đủ `options`, `correctOrder`, `expectedAnswer`.
- [ ] Bài tập có `hint`, `explanation`, `orderIndex`.
- [ ] `courseCode` và `level` có ở cấp file.
- [ ] `lessonNumber` là số nguyên, không dùng dạng `4-1`.
- [ ] Mỗi pattern có đủ `pattern`, `title`, `meaning`, `structure`.
- [ ] Mỗi example có `japanese` và `meaning`.
- [ ] `orderIndex` không trùng trong cùng lesson/pattern.
- [ ] Không dùng tên field của từ vựng như `exampleJapanese` cho grammar.
- [ ] Nếu có exercise tùy chọn, dùng đúng `exerciseType`: `vi_to_ja`, `ja_to_vi`, `arrange`.
