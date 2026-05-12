# Quy Tắc Định Dạng Dữ Liệu Đầu Vào (Data Generation Rules)

Để hệ thống AI có thể xử lý, import và chuyển đổi dữ liệu thành các file JSON chuẩn một cách nhanh chóng và chính xác nhất, người dùng cần chuẩn bị dữ liệu theo các quy tắc và định dạng (format) dưới đây.

Định dạng **dễ nhận diện nhất là Text dạng Bảng (copy trực tiếp từ Excel/Google Sheets)** hoặc **Markdown**.

---

## 1. Module Từ vựng (Vocabulary)
**Cách tốt nhất:** Làm trên Google Sheets/Excel và copy/paste thẳng vào khung chat.
**Cấu trúc các cột cần có:**
`Bài học (Lesson)` | `Từ vựng (Kanji)` | `Cách đọc (Hiragana)` | `Ý nghĩa` | `Ví dụ (Tiếng Nhật)` | `Nghĩa ví dụ`

**Ví dụ định dạng:**
```text
Lesson 1    学生    がくせい    Học sinh, sinh viên    私は学生です。    Tôi là học sinh.
Lesson 1    先生    せんせい    Giáo viên              彼は先生です。    Anh ấy là giáo viên.
```

---

## 2. Module Hán tự (Kanji)
**Cách tốt nhất:** Làm trên Google Sheets/Excel và copy/paste.
**Cấu trúc các cột cần có:**
`Bài học` | `Kanji` | `Hán Việt` | `Ý nghĩa` | `Onyomi (Âm On)` | `Kunyomi (Âm Kun)` | `Từ vựng chứa Kanji (VD1, VD2)`

**Ví dụ định dạng:**
```text
Lesson 1    日    NHẬT    Mặt trời, ngày    ニチ, ジツ    ひ, -び, -か    毎日 (まいにち - mỗi ngày), 日曜日 (にちようび - chủ nhật)
Lesson 1    月    NGUYỆT  Mặt trăng, tháng  ゲツ, ガツ    つき            今月 (こんげつ - tháng này), 月 (つき - mặt trăng)
```

---

## 3. Module Ngữ pháp (Grammar)
Ngữ pháp có cấu trúc dài hơn (gồm giải thích và nhiều ví dụ), nên viết dưới dạng **Block Text (Markdown)** là tốt nhất.

**Ví dụ định dạng:**
```markdown
## Lesson 1
**Cấu trúc:** N1 は N2 です
**Ý nghĩa:** N1 là N2
**Giải thích:** Trợ từ "は" biểu thị N1 là chủ đề của câu. Đọc là "wa". "です" thể hiện sự khẳng định và tôn trọng.
**Ví dụ:**
- 私はマイク・ミラーです。 (Tôi là Mike Miller.)
- 彼は学生です。 (Anh ấy là học sinh.)

**Cấu trúc:** N1 は N2 じゃありません
**Ý nghĩa:** N1 không phải là N2
**Giải thích:** Phủ định của "です". Trong văn nói dùng "じゃありません", văn viết dùng "ではありません".
**Ví dụ:**
- 私は学生じゃありません。 (Tôi không phải là sinh viên.)
```

---

## 4. Module Luyện thi (Exam Practice)
Vì câu hỏi trắc nghiệm có đáp án đúng/sai, format dễ nhất là dạng **Q&A List** có đánh dấu đáp án đúng bằng dấu `(*)` hoặc `[x]`.

**Quy tắc:**
- Cần ghi rõ `Topic` (kanji, vocabulary, grammar, reading, odd_one_out) và `Level` (N5, N4).
- Nếu là bài đọc (Reading), cung cấp đoạn văn trước, sau đó là các câu hỏi.

**Ví dụ định dạng:**
```text
Khóa: JPD113
Chủ đề: kanji
Level: N5

[Q1] Kanji nào có nghĩa là 'người'?
A. 日
B. 月
C. 火
D. 人 (*)
Giải thích: 人 đọc là ひと hoặc ジン/ニン và có nghĩa là người.

Chủ đề: grammar
[Q2] Chọn câu lịch sự đúng với nghĩa 'Tôi là học sinh'.
A. 私は学生です。 (*)
B. 私は学生ます。
C. 私は学生をです。
D. 私は学生がます。
Giải thích: Danh từ + です dùng để nói 'là...' một cách lịch sự.

Chủ đề: reading
[Passage 1]
Title: Một ngày của Tanaka
Content: 田中さんは毎朝六時に起きます。七時に朝ごはんを食べます。それから電車で学校へ行きます。...
[Q3] 田中さんは何時に起きますか。
A. 六時 (*)
B. 七時
C. 八時半
D. 九時
Giải thích: Đoạn văn có câu 田中さんは毎朝六時に起きます。
```

---

## 🛠 Quy trình làm việc (Workflow) khi có data:
1. Soạn data theo format trên (gõ thô, hoặc copy từ Excel).
2. Dán data đó vào chat kèm yêu cầu: *"Tạo data cho khóa [Khóa học] module [Tên Module]"*.
3. AI sẽ tự động phân tích, map các trường thông tin và sinh ra file `.json` chuẩn (ví dụ `jpd113.questions.json`).
4. AI sử dụng script để lưu file JSON vào thư mục `server/JPLearn.Infrastructure/Data/Imports/` và chạy lệnh import trực tiếp vào Database.
