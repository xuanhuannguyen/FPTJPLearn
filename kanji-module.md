# JPLearn Kanji Module — Requirements

## 1. Context

Module Hán tự giúp user học Kanji theo cấp độ JLPT:

```text
N5 -> N4 -> N3 -> N2 -> N1
```

Mỗi level có nhiều bài. Ví dụ `Kanji N5` có khoảng 11 bài. Mỗi bài có:

- khoảng 10 Kanji chính
- khoảng 10-20 từ vựng liên quan có chứa Kanji trong bài

Module này là **content source** cho Kanji. Khi user muốn ôn dài hạn, Kanji sẽ được thêm vào module **Ghi nhớ** dưới dạng snapshot riêng.

---

## 2. Product Decisions

### 2.1 Kanji là content library + practice theo bài

Kanji module gồm:

```text
Kanji Content
├── Level
├── Lesson
├── Kanji item
└── Related vocabulary

Kanji Practice
├── List box
├── Study mode theo từng Kanji
├── Kanji flashcard 10 chữ
└── Vocabulary flashcard
```

Module Kanji không tự ôm toàn bộ SRS dài hạn. SRS dài hạn sẽ dùng module Ghi nhớ:

```text
KanjiItem
-> user bấm "Thêm vào ghi nhớ"
-> Memory tạo UserMemoryKanjiItem
-> review/update UserMemoryKanjiItem
```

### 2.2 Kanji chính và từ vựng liên quan là hai loại content khác nhau

Trong mỗi lesson:

- Kanji chính dùng cho học chữ, luyện viết, flashcard.
- Từ vựng liên quan dùng để học cách Kanji xuất hiện trong từ thực tế.

Không gộp Kanji chính và vocabulary thành một bảng vì UI và mode học khác nhau.

### 2.3 Không dùng `meaningDetail`

Kanji item chỉ cần các field nội dung chính:

```text
Kanji
Nghĩa Hán Việt
Nghĩa
Số nét
Âm Kun
Âm On
Gợi ý cách nhớ
```

Không tạo field `meaningDetail` hoặc `Ý nghĩa` riêng để tránh data thừa.

### 2.4 Writing practice phase đầu là luyện viết cơ bản

Phase đầu không cần AI chấm chữ viết. Nếu có `strokeDataJson`, hệ thống có thể check cơ bản theo nét/đường guide; nếu chưa có data thì fallback về self-check.

Writing practice cần:

- canvas để user viết
- nút xóa
- nút xem nét vẽ
- hiệu ứng vẽ từng nét khi user chạm vào `Xem nét vẽ`
- hiển thị stroke guide nếu có data
- trạng thái đúng/sai nếu hệ thống đủ stroke data để check

Nếu chưa có stroke order data thật, UI vẫn hoạt động ở mức luyện viết tự do.

### 2.6 Lesson có hai mode chính cho 10 Kanji

Khi user mở lesson và chọn nhóm 10 Kanji chính, UI hiển thị list 10 Kanji và 2 option:

```text
Học
Flashcard
```

`Học` là flow tuần tự từng Kanji:

```text
Kanji 1 -> Kanji 2 -> ... -> Kanji 10
```

Mỗi page Kanji trong mode Học gồm:

- box thông tin ý nghĩa
- box xem nét vẽ/stroke animation
- box luyện viết và check đúng/sai
- box map bộ thủ/thành phần cấu tạo
- nút thêm vào Ghi nhớ
- nút chuyển Kanji trước/sau

`Flashcard` chỉ học nhanh 10 Kanji trong lesson.

### 2.7 Từ vựng liên quan chỉ có list và flashcard

Khi user bấm khu vực từ vựng liên quan, UI hiển thị danh sách từ vựng trong lesson và button ôn flashcard.

Từ vựng liên quan không có:

- luyện viết
- map bộ thủ
- stroke animation

### 2.5 Premium để sau

Thiết kế dữ liệu có thể chuẩn bị field access về sau, nhưng phase đầu không chặn premium.

Có thể để:

```text
accessTier = free
packageCode = kanji_n5
isLocked = false
```

Không làm payment, license, subscription trong phase này.

---

## 3. User Stories

### K1: Xem dashboard Kanji

User muốn mở `/kanji` và thấy các box:

```text
Kanji N5
Kanji N4
Kanji N3
Kanji N2
Kanji N1
```

Mỗi box hiển thị:

- level
- số bài
- số Kanji
- số từ vựng liên quan
- tiến độ học nếu có

### K2: Xem danh sách bài trong level

User mở `Kanji N5` và thấy khoảng 11 bài.

Mỗi bài hiển thị:

- bài số mấy
- tiêu đề
- số Kanji
- số từ vựng
- trạng thái học

### K3: Xem lesson Kanji

User mở một lesson và thấy:

- list box 10 Kanji chính
- mỗi box hiển thị Kanji, Hán Việt, nghĩa, số nét, âm On/Kun ngắn gọn
- khu vực từ vựng liên quan
- nhóm 10 Kanji chính có 2 option: `Học`, `Flashcard`
- từ vựng liên quan có option: `Flashcard`

### K4: Học 10 Kanji theo từng page

User bấm `Học` trong lesson và đi qua từng Kanji.

Mỗi page Kanji hiển thị:

```text
Kanji lớn
Box thông tin:
  Nghĩa Hán Việt
  Nghĩa
  Trình độ JLPT
  Số nét
  Âm Kun
  Âm On
  Gợi ý cách nhớ

Box nét vẽ:
  Nút xem nét vẽ
  Số nét
  Hiệu ứng vẽ từng nét khi chạm vào

Box luyện viết:
  Canvas viết
  Clear
  Check đúng/sai nếu có stroke data

Box bộ thủ/thành phần:
  Map các bộ thủ hoặc thành phần cấu tạo nên Kanji

Actions:
  Thêm vào Ghi nhớ
  Kanji trước
  Kanji tiếp theo
```

### K5: Xem chi tiết Kanji độc lập

User click một Kanji từ list hoặc search và thấy page chi tiết giống nội dung của mode Học, nhưng không bắt buộc nằm trong flow 10 Kanji.

### K6: Học bằng flashcard Kanji

User có thể học 10 Kanji trong lesson bằng flashcard.

Mặt trước:

```text
Kanji lớn
```

Mặt sau:

```text
Hán Việt
Nghĩa
Số nét
Âm Kun
Âm On
Gợi ý cách nhớ
```

### K7: Luyện viết Kanji

User mở writing practice cho lesson hoặc Kanji detail.

Yêu cầu:

- hiển thị Kanji mẫu
- có vùng canvas để viết
- có nút xóa
- có nút xem nét vẽ
- có check đúng/sai nếu có stroke data
- có thể chuyển Kanji tiếp theo

### K8: Học từ vựng liên quan bằng flashcard

User có thể học từ vựng liên quan trong lesson bằng flashcard.

Mặt trước:

```text
word
reading
```

Mặt sau:

```text
meaning
exampleJapanese
exampleReading
exampleMeaning
```

Từ vựng liên quan phase đầu không cần luyện viết.

### K9: Thêm Kanji vào Ghi nhớ

User có thể bấm `Thêm vào ghi nhớ` ở Kanji detail hoặc lesson list.

Kết quả:

- tạo Memory Kanji snapshot
- không duplicate nếu bấm nhiều lần
- `/memory` tab Kanji cập nhật số due/new
- `/memory/kanji/review` ôn bằng SRS riêng của Memory

---

## 4. Data Model

### 4.1 KanjiLesson

```text
id
level                 // N5 | N4 | N3 | N2 | N1
lessonNumber
title
description
accessTier            // free now, premium later
packageCode           // kanji_n5, kanji_n4...
orderIndex
createdAt
updatedAt
```

### 4.2 KanjiItem

```text
id
lessonId
level
character             // 一
hanViet               // Nhất
meaning               // Một
strokeCount           // 1
kunReading            // ひと, ひとつ
onReading             // イチ, イツ
mnemonic              // 1 nét ngang...
strokeSvg             // optional
strokeDataJson        // optional later
componentMapJson      // optional: bộ thủ/thành phần cấu tạo
accessTierOverride
packageCodeOverride
orderIndex
createdAt
updatedAt
```

### 4.3 KanjiVocabulary

```text
id
lessonId
kanjiItemId nullable
level
word                  // 一人
reading               // ひとり
meaning               // một người
exampleJapanese
exampleReading
exampleMeaning
orderIndex
createdAt
updatedAt
```

### 4.4 UserKanjiProgress

Progress nội bộ của Kanji module dùng cho trạng thái học trong content module.

```text
id
userId
kanjiItemId
isLearned
lastViewedAt
writingPracticeCount
flashcardPracticeCount
createdAt
updatedAt
```

SRS dài hạn không lưu ở đây. SRS dài hạn thuộc module Ghi nhớ.

### 4.5 KanjiComponent / Component Map

Phase đầu có thể lưu component map bằng JSON trong `KanjiItem.componentMapJson` để giảm số bảng.

Format gợi ý:

```json
[
  {
    "component": "亻",
    "name": "nhân đứng",
    "meaning": "người",
    "position": "left",
    "note": "gợi nhớ liên quan đến người"
  }
]
```

Nếu sau này cần search/filter theo bộ thủ, tách thành bảng riêng:

```text
KanjiComponents
├── id
├── kanji_item_id
├── component
├── name
├── meaning
├── position
├── note
├── order_index
```

---

## 5. API Requirements

### Content APIs

```text
GET /api/kanji/levels
GET /api/kanji/{level}/lessons
GET /api/kanji/lessons/{lessonId}
GET /api/kanji/items/{kanjiItemId}
GET /api/kanji/search?query=
```

### Practice APIs

```text
POST /api/kanji/items/{kanjiItemId}/view
POST /api/kanji/items/{kanjiItemId}/writing-practice
POST /api/kanji/items/{kanjiItemId}/flashcard-practice
```

### Memory bridge APIs

```text
POST /api/memory/kanji/from-kanji/{kanjiItemId}
GET  /api/memory/kanji/from-kanji/{kanjiItemId}/status
GET  /api/memory/kanji/cards?scope=due
POST /api/memory/kanji/answer
POST /api/memory/kanji/reset
```

Vocabulary liên quan trong Memory làm sau hoặc đi qua Memory vocabulary riêng:

```text
POST /api/memory/vocabulary/from-kanji-vocabulary/{kanjiVocabularyId}
```

---

## 6. Frontend Routes

```text
/kanji
/kanji/:level
/kanji/:level/lessons/:lessonId
/kanji/items/:kanjiItemId
/kanji/:level/lessons/:lessonId/study
/kanji/:level/lessons/:lessonId/flashcards
/kanji/:level/lessons/:lessonId/vocabulary-flashcards
```

Memory routes:

```text
/memory/kanji/review
/memory/vocabulary/review
```

---

## 7. UI Requirements

### 7.1 Kanji Home

Không làm landing page. Màn đầu là dashboard học Kanji.

Nội dung:

- header ngắn
- level cards N5-N1
- mỗi card có stats
- click vào level để xem lessons

### 7.2 Kanji Level Page

Hiển thị list lessons theo level.

Card lesson gồm:

- lesson number
- title
- số Kanji
- số vocabulary
- progress

### 7.3 Kanji Lesson Page

Gồm 2 section chính:

```text
Kanji chính
Từ vựng liên quan
```

Kanji chính có list box 10 item. Mỗi item hiển thị:

- Kanji lớn
- Hán Việt
- nghĩa
- số nét
- On/Kun ngắn

Action:

- Học
- Flashcard
- Chi tiết từng Kanji
- Thêm vào Ghi nhớ từng Kanji
- Xem từ vựng liên quan

### 7.4 Kanji Study Page

Route:

```text
/kanji/:level/lessons/:lessonId/study
```

Hiển thị tuần tự 10 Kanji trong lesson.

Layout mỗi Kanji:

- header tiến độ `1 / 10`
- Kanji lớn
- box thông tin
- box xem nét vẽ
- box luyện viết
- box map bộ thủ/thành phần
- actions: thêm vào Ghi nhớ, trước, tiếp

Không dùng layout nhiều page browser khác nhau cho từng Kanji trong lesson. Đây là một route study có current index để chuyển Kanji.

### 7.5 Kanji Detail Page

Layout dựa trên ảnh mẫu:

- Kanji lớn
- button `Xem nét vẽ`
- stroke animation khi chạm vào button
- thông tin chính bên phải hoặc dưới tùy viewport
- writing canvas
- check đúng/sai nếu có data
- component map/bộ thủ
- button `Thêm vào ghi nhớ`
- danh sách từ vựng liên quan

### 7.6 Flashcard

Kanji flashcard dùng visual tương tự Memory flashcard nhưng không cần SRS rating.

Controls:

- click/Space để lật
- next/previous
- mark learned optional

### 7.7 Writing Practice

Canvas không nằm trong card lồng card.

Controls:

- clear
- undo optional
- show/hide guide
- next Kanji
- check đúng/sai nếu có stroke data

### 7.8 Related Vocabulary

Related vocabulary section chỉ có:

- list từ vựng
- button flashcard
- không có writing practice
- không có component map

---

## 8. Integration With Memory

Module Kanji là source. Memory là nơi ôn dài hạn.

Khi add Kanji vào Memory:

```text
KanjiItem
-> UserMemoryKanjiItem snapshot
```

Snapshot cần lưu:

```text
sourceKanjiItemId
character
hanViet
meaning
strokeCount
kunReading
onReading
mnemonic
level
```

Memory Kanji review card:

Front:

```text
character
```

Back:

```text
hanViet
meaning
strokeCount
kunReading
onReading
mnemonic
```

---

## 9. Out Of Scope Phase Đầu

Không làm:

- premium thật
- AI chấm chữ viết
- nhận diện nét viết tự động
- stroke order data đầy đủ cho toàn bộ Kanji nếu chưa có nguồn
- check đúng/sai chính xác 100% khi chưa có stroke data chuẩn
- import admin UI
- Kanji N4-N1 đầy đủ ngay từ đầu
- gộp Kanji và vocabulary vào cùng một memory review session

---

## 10. Acceptance Criteria

- `/kanji` hiển thị level cards N5-N1.
- `/kanji/N5` hiển thị lessons N5.
- Một lesson N5 có khoảng 10 Kanji và 10-20 vocabulary.
- Kanji item hiển thị đủ: Kanji, Hán Việt, nghĩa, số nét, Kun, On, mnemonic.
- Không có field `meaningDetail`.
- User mở được Kanji detail.
- User học được Kanji flashcard trong lesson.
- User học được 10 Kanji theo mode Học tuần tự.
- User xem được stroke animation khi có stroke data.
- User luyện viết bằng canvas và check được nếu có stroke data.
- User xem được map bộ thủ/thành phần cấu tạo.
- User học được vocabulary flashcard trong lesson.
- User thêm Kanji vào Ghi nhớ.
- Memory Kanji summary/review chuẩn bị được nối sau.
- `npm run build` pass.
- `dotnet build` pass.
