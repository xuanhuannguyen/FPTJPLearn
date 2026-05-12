# JPLearn Kanji Module — Project Plan

## 1. Scope Hiện Tại

Triển khai module Hán tự theo thứ tự:

```text
Backend Kanji content
-> Seed Kanji N5 lesson đầu
-> Frontend Kanji dashboard/level/lesson/detail
-> Frontend Kanji study mode theo từng Kanji
-> Frontend Kanji flashcard
-> Memory Kanji bridge
```

Phase đầu tập trung vào `N5` để ổn định schema và UI. N4-N1 chỉ cần hiển thị box/empty hoặc seed sau.

---

## 2. Out Of Scope Phase Này

Không làm:

- Premium thật.
- Admin UI import Kanji.
- AI chấm chữ viết.
- Nhận diện nét viết.
- Full stroke order engine.
- Check chữ viết chính xác 100% khi chưa có stroke data chuẩn.
- Full data N4-N1.
- Gộp Kanji và vocabulary vào cùng một Memory review session.
- Dùng module Vocabulary hiện tại làm dữ liệu vocabulary liên quan.

---

## 3. Kiến Trúc Mục Tiêu

### Backend

```text
server/JPLearn.Core/Kanji/
├── IKanjiService.cs
├── Entities/
│   ├── KanjiLesson.cs
│   ├── KanjiItem.cs
│   ├── KanjiVocabulary.cs
│   └── UserKanjiProgress.cs
└── DTOs/
    ├── KanjiLevelDto.cs
    ├── KanjiLessonDto.cs
    ├── KanjiLessonDetailDto.cs
    ├── KanjiItemDto.cs
    ├── KanjiDetailDto.cs
    └── KanjiVocabularyDto.cs

server/JPLearn.Infrastructure/Services/
└── KanjiService.cs

server/JPLearn.Infrastructure/Data/Configurations/
└── KanjiConfigurations.cs

server/JPLearn.Api/Controllers/
└── KanjiController.cs
```

### Frontend

```text
client/src/features/kanji/
├── kanji.routes.tsx
├── api/
│   └── kanjiApi.ts
├── types/
│   └── kanji.types.ts
├── pages/
│   ├── KanjiHomePage.tsx
│   ├── KanjiLevelPage.tsx
│   ├── KanjiLessonPage.tsx
│   ├── KanjiDetailPage.tsx
│   ├── KanjiStudyPage.tsx
│   ├── KanjiLessonFlashcardPage.tsx
│   └── KanjiVocabularyFlashcardPage.tsx
└── components/
    ├── KanjiLevelCard.tsx
    ├── KanjiLessonCard.tsx
    ├── KanjiListCard.tsx
    ├── KanjiInfoPanel.tsx
    ├── KanjiFlashcard.tsx
    ├── KanjiWritingCanvas.tsx
    ├── KanjiStrokeViewer.tsx
    ├── KanjiWritingCheckPanel.tsx
    ├── KanjiComponentMap.tsx
    └── KanjiVocabularyFlashcard.tsx
```

---

## 4. Backend Phase 1 — Domain Entities

### Mục tiêu

Tạo schema Kanji content độc lập.

### Tasks

- [ ] Tạo folder `server/JPLearn.Core/Kanji`.
- [ ] Tạo `KanjiLesson`.
- [ ] Tạo `KanjiItem`.
- [ ] Tạo `KanjiVocabulary`.
- [ ] Tạo `UserKanjiProgress`.
- [ ] Tạo DTOs.
- [ ] Add DbSet vào `AppDbContext`.
- [ ] Tạo `KanjiConfigurations`.
- [ ] Add migration `AddKanjiModule`.
- [ ] Verify migration tạo đúng bảng.

### Entity Summary

```text
KanjiLessons
├── id
├── level
├── lesson_number
├── title
├── description
├── access_tier
├── package_code
├── order_index
```

```text
KanjiItems
├── id
├── lesson_id
├── level
├── character
├── han_viet
├── meaning
├── stroke_count
├── kun_reading
├── on_reading
├── mnemonic
├── stroke_svg nullable
├── stroke_data_json nullable
├── component_map_json nullable
├── access_tier_override nullable
├── package_code_override nullable
├── order_index
```

```text
KanjiVocabulary
├── id
├── lesson_id
├── kanji_item_id nullable
├── level
├── word
├── reading
├── meaning
├── example_japanese nullable
├── example_reading nullable
├── example_meaning nullable
├── order_index
```

```text
UserKanjiProgress
├── id
├── user_id
├── kanji_item_id
├── is_learned
├── last_viewed_at nullable
├── writing_practice_count
├── flashcard_practice_count
```

### Verification

```text
dotnet build
dotnet ef migrations add AddKanjiModule
dotnet ef database update
```

---

## 5. Backend Phase 2 — Content API

### Mục tiêu

Frontend đọc được level, lesson, Kanji detail, vocabulary liên quan.

### Tasks

- [ ] Tạo `IKanjiService`.
- [ ] Implement `KanjiService`.
- [ ] Tạo `KanjiController`.
- [ ] Add DI registration.
- [ ] Implement level overview.
- [ ] Implement lesson list by level.
- [ ] Implement lesson detail.
- [ ] Implement Kanji detail.
- [ ] Implement search.

### Endpoints

```text
GET /api/kanji/levels
GET /api/kanji/{level}/lessons
GET /api/kanji/lessons/{lessonId}
GET /api/kanji/items/{kanjiItemId}
GET /api/kanji/search?query=
```

### Rules

- Level chỉ nhận `N5`, `N4`, `N3`, `N2`, `N1`.
- Lessons sort theo `orderIndex`, sau đó `lessonNumber`.
- Kanji items sort theo `orderIndex`.
- Vocabulary sort theo `orderIndex`.
- DTO không có `meaningDetail`.

### Verification

- [ ] Swagger gọi được `/api/kanji/levels`.
- [ ] Swagger gọi được lesson detail.
- [ ] Lesson detail trả Kanji items và vocabulary.
- [ ] Kanji detail trả đúng fields.
- [ ] Kanji detail trả `componentMapJson` nếu có.

---

## 6. Backend Phase 3 — Practice Progress

### Mục tiêu

Lưu progress nhẹ trong Kanji module, không phải SRS dài hạn.

### Tasks

- [ ] Implement `POST /api/kanji/items/{kanjiItemId}/view`.
- [ ] Implement `POST /api/kanji/items/{kanjiItemId}/writing-practice`.
- [ ] Implement `POST /api/kanji/items/{kanjiItemId}/flashcard-practice`.
- [ ] Nếu progress chưa có thì tạo mới.
- [ ] Increment count tương ứng.
- [ ] Update `lastViewedAt`.

### Verification

- [ ] View detail tạo hoặc update progress.
- [ ] Writing practice increment count.
- [ ] Flashcard practice increment count.
- [ ] Không ảnh hưởng Memory SRS.

---

## 7. Backend Phase 4 — Seed Data

### Mục tiêu

Có data thật để build UI.

### Minimum Seed

```text
Kanji N5
├── Lesson 1
│   ├── 10 Kanji
│   └── 10-20 vocabulary
```

Ví dụ Kanji:

```text
一
二
三
四
五
六
七
八
九
十
```

Mỗi Kanji cần:

```text
character
hanViet
meaning
strokeCount
kunReading
onReading
mnemonic
strokeDataJson optional
componentMapJson optional
```

Mỗi vocabulary cần:

```text
word
reading
meaning
exampleJapanese
exampleReading
exampleMeaning
```

### Tasks

- [ ] Tạo migration seed N5 lesson 1.
- [ ] Seed 10 Kanji đầu.
- [ ] Seed vocabulary liên quan.
- [ ] Verify API trả đủ data.

### Verification

- [ ] `/api/kanji/levels` có N5.
- [ ] `/api/kanji/N5/lessons` có lesson 1.
- [ ] `/api/kanji/lessons/{id}` có 10 Kanji và vocabulary.

---

## 8. Frontend Phase 1 — Routes & API

### Tasks

- [ ] Tạo `features/kanji`.
- [ ] Tạo `kanji.routes.tsx`.
- [ ] Tạo `kanjiApi.ts`.
- [ ] Tạo `kanji.types.ts`.
- [ ] Add routes vào `Router.tsx`.
- [ ] Add Kanji vào Sidebar nếu chưa có link đúng.

### Routes

```text
/kanji
/kanji/:level
/kanji/:level/lessons/:lessonId
/kanji/items/:kanjiItemId
/kanji/:level/lessons/:lessonId/study
/kanji/:level/lessons/:lessonId/flashcards
/kanji/:level/lessons/:lessonId/vocabulary-flashcards
```

### Verification

- [ ] Route `/kanji` hoạt động.
- [ ] Route level/lesson/detail hoạt động.
- [ ] `npm run build` pass.

---

## 9. Frontend Phase 2 — Kanji Dashboard

### Tasks

- [ ] Build `KanjiHomePage`.
- [ ] Build `KanjiLevelCard`.
- [ ] Load `/api/kanji/levels`.
- [ ] Render N5-N1 cards.
- [ ] Empty state cho level chưa có data.

### UI Rules

- Không làm landing page.
- Cards scan nhanh.
- Mỗi card có số bài, số Kanji, số vocabulary.

### Verification

- [ ] User mở `/kanji` thấy N5-N1.
- [ ] Click N5 đi `/kanji/N5`.

---

## 10. Frontend Phase 3 — Level & Lesson Pages

### Tasks

- [ ] Build `KanjiLevelPage`.
- [ ] Build `KanjiLessonCard`.
- [ ] Build `KanjiLessonPage`.
- [ ] Build `KanjiListCard`.
- [ ] Build related vocabulary section.
- [ ] Add Kanji group actions: `Học`, `Flashcard`.
- [ ] Add per-Kanji actions: `Chi tiết`, `Thêm vào Ghi nhớ`.
- [ ] Add related vocabulary action: `Vocabulary flashcard`.

### Lesson Page Layout

```text
Header
Actions for 10 Kanji: Học | Flashcard
Kanji chính grid/list
Vocabulary liên quan list + Vocabulary flashcard
```

### Verification

- [ ] N5 page hiển thị lessons.
- [ ] Lesson page hiển thị 10 Kanji.
- [ ] Lesson page hiển thị vocabulary liên quan.
- [ ] Button `Học` đi study mode.
- [ ] Button `Flashcard` chỉ học 10 Kanji.
- [ ] Vocabulary section chỉ có list + flashcard.

---

## 11. Frontend Phase 4 — Kanji Study Mode

### Tasks

- [ ] Build `KanjiStudyPage`.
- [ ] Load lesson detail.
- [ ] Dùng 10 Kanji items trong lesson.
- [ ] Hiển thị current index `1 / 10`.
- [ ] Build `KanjiInfoPanel`.
- [ ] Build `KanjiStrokeViewer`.
- [ ] Build `KanjiWritingCanvas`.
- [ ] Build `KanjiWritingCheckPanel`.
- [ ] Build `KanjiComponentMap`.
- [ ] Add actions: `Thêm vào Ghi nhớ`, `Trước`, `Tiếp`.
- [ ] Khi qua Kanji tiếp theo, giữ layout ổn định.

### Study Page Layout

```text
Header: Lesson title + 1/10
Kanji large

Info box:
  Hán Việt
  Nghĩa
  JLPT level
  Số nét
  Âm Kun
  Âm On
  Gợi ý cách nhớ

Stroke box:
  Xem nét vẽ
  Số nét
  Stroke animation nếu có data

Writing box:
  Canvas
  Clear
  Check đúng/sai nếu có stroke data

Component map:
  Bộ thủ/thành phần cấu tạo

Actions:
  Thêm vào Ghi nhớ
  Trước
  Tiếp
```

### Writing Check Rule

Phase đầu:

```text
if strokeDataJson exists:
  check cơ bản theo guide/stroke data
else:
  show self-check state, không báo chắc chắn đúng/sai
```

Không dùng AI hoặc nhận diện handwriting phức tạp trong phase đầu.

### Verification

- [ ] User đi từ Kanji 1 đến Kanji 10.
- [ ] Info box render đủ fields.
- [ ] Stroke animation chạy nếu có data.
- [ ] Writing canvas hoạt động.
- [ ] Check đúng/sai hoạt động ở mức có data.
- [ ] Component map render nếu có data.
- [ ] Add to Memory action hiển thị đúng trạng thái.

---

## 12. Frontend Phase 5 — Kanji Detail Page

### Tasks

- [ ] Build `KanjiDetailPage`.
- [ ] Build `KanjiInfoPanel`.
- [ ] Reuse `KanjiStrokeViewer`.
- [ ] Reuse `KanjiWritingCanvas`.
- [ ] Reuse `KanjiWritingCheckPanel`.
- [ ] Reuse `KanjiComponentMap`.
- [ ] Render fields:
  - Kanji
  - Hán Việt
  - Nghĩa
  - JLPT level
  - Số nét
  - Âm Kun
  - Âm On
  - Gợi ý cách nhớ
- [ ] Add `Xem nét vẽ` button.
- [ ] Show stroke animation on click/touch.
- [ ] Add writing box with check.
- [ ] Add component map.
- [ ] Add related vocabulary list.
- [ ] Call `POST /api/kanji/items/{id}/view`.

### Verification

- [ ] Detail page giống hướng UI ảnh mẫu.
- [ ] Không render `meaningDetail`.
- [ ] Detail page không khác logic dữ liệu với study page.
- [ ] View detail không lỗi API.

---

## 13. Frontend Phase 6 — Kanji Flashcards

### Tasks

- [ ] Build `KanjiLessonFlashcardPage`.
- [ ] Build `KanjiFlashcard`.
- [ ] Load lesson detail.
- [ ] Use 10 Kanji items.
- [ ] Click/Space để lật.
- [ ] Next/previous.
- [ ] Mark flashcard practice after card completed.

### Front Side

```text
Kanji large
```

### Back Side

```text
Hán Việt
Nghĩa
Số nét
Âm Kun
Âm On
Gợi ý cách nhớ
```

### Verification

- [ ] Flashcard lật được.
- [ ] Next/previous hoạt động.
- [ ] Không chồng text trên mobile/desktop.

---

## 14. Frontend Phase 7 — Vocabulary List & Flashcards

### Tasks

- [ ] Lesson page hiển thị related vocabulary list.
- [ ] Vocabulary list item hiển thị word, reading, meaning.
- [ ] Build `KanjiVocabularyFlashcardPage`.
- [ ] Build `KanjiVocabularyFlashcard`.
- [ ] Load vocabulary từ lesson detail.
- [ ] Click/Space để lật.
- [ ] Next/previous.

### Front Side

```text
word
reading
```

### Back Side

```text
meaning
exampleJapanese
exampleReading
exampleMeaning
```

### Verification

- [ ] Bấm vocabulary section hiển thị list từ liên quan.
- [ ] Vocabulary flashcard hiển thị đúng.
- [ ] Không dùng API của module Vocabulary hiện tại.
- [ ] Vocabulary liên quan không có writing/check/component map.

---

## 15. Backend Phase 5 — Memory Kanji Bridge

### Mục tiêu

Cho Kanji add vào module Ghi nhớ.

### Backend Tasks

- [ ] Tạo `UserMemoryKanjiItem`.
- [ ] Add DbSet/config/migration.
- [ ] Tạo `IMemoryKanjiService`.
- [ ] Implement add from Kanji item.
- [ ] Implement status.
- [ ] Implement cards.
- [ ] Implement answer bằng `IMemorySrsService`.
- [ ] Implement reset.
- [ ] Update `MemoryService.GetSummaryAsync` để trả Kanji summary thật.
- [ ] Update `MemoryController` endpoints Kanji.

### Endpoints

```text
POST /api/memory/kanji/from-kanji/{kanjiItemId}
GET  /api/memory/kanji/from-kanji/{kanjiItemId}/status
GET  /api/memory/kanji/cards?scope=due
POST /api/memory/kanji/answer
POST /api/memory/kanji/reset
```

### Snapshot Fields

```text
source_kanji_item_id
character
han_viet
meaning
stroke_count
kun_reading
on_reading
mnemonic
level
```

### Verification

- [ ] Add Kanji vào Memory không duplicate.
- [ ] `/memory` tab Kanji có summary thật.
- [ ] `/memory/kanji/review` lấy cards thật.

---

## 16. Frontend Phase 8 — Memory Kanji Review

### Tasks

- [ ] Tạo `MemoryKanjiReviewPage`.
- [ ] Reuse `useMemoryReviewSession`.
- [ ] Tạo `KanjiMemoryFlashcard`.
- [ ] Reuse `MemoryRatingButtons`.
- [ ] Update memory routes từ placeholder sang page thật.
- [ ] Update dashboard Kanji reset nếu backend reset đã có.

### Verification

- [ ] Kanji memory card render.
- [ ] Flip works.
- [ ] Rating calls `/api/memory/kanji/answer`.
- [ ] Completed session summary works.

---

## 17. Testing Plan

### Backend

- [ ] `dotnet build`
- [ ] Migration runs.
- [ ] Kanji content APIs return data.
- [ ] Search finds character, Hán Việt, meaning, reading.
- [ ] Practice progress endpoints work.
- [ ] Memory Kanji endpoints work after bridge phase.

### Frontend

- [ ] `npm run build`
- [ ] `/kanji` renders.
- [ ] `/kanji/N5` renders.
- [ ] Lesson page renders Kanji + vocabulary.
- [ ] Detail page renders all fields.
- [ ] Kanji flashcard works.
- [ ] Writing canvas works.
- [ ] Vocabulary flashcard works.

---

## 18. Recommended Build Order

```text
1. Backend Kanji entities + config + migration
2. Backend Kanji content API
3. Seed N5 lesson 1
4. Frontend routes + API + types
5. Kanji dashboard
6. Level page + lesson page
7. Kanji study mode
8. Kanji detail page
9. Kanji flashcard
10. Vocabulary flashcard
11. Memory Kanji backend bridge
12. Memory Kanji review frontend
13. Expand seed data to N5 11 lessons
```

---

## 19. Final Acceptance Criteria

- [ ] Kanji module có dashboard N5-N1.
- [ ] N5 có lesson list.
- [ ] Lesson detail có Kanji chính và vocabulary liên quan.
- [ ] Kanji detail không có `meaningDetail`.
- [ ] Kanji lesson có 2 option cho 10 Kanji: `Học`, `Flashcard`.
- [ ] Study mode hiển thị từng Kanji theo thứ tự.
- [ ] Study mode có info box, stroke box, writing check, component map.
- [ ] Flashcard Kanji hoạt động.
- [ ] Vocabulary liên quan chỉ có list + flashcard.
- [ ] Vocabulary flashcard hoạt động.
- [ ] Kanji có thể thêm vào Ghi nhớ.
- [ ] Memory Kanji review hoạt động sau bridge phase.
- [ ] `dotnet build` pass.
- [ ] `npm run build` pass.
