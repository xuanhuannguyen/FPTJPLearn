# JPLearn Memory Module — Brainstorm & Requirements

## 1. Context

Module **Ghi nhớ** là nơi user ôn tập lại kiến thức đã được thêm từ các module học:

```text
Kanji
Vocabulary riêng của Memory trong tương lai
Grammar
```

Điểm chốt mới:

- Module Ghi nhớ **không liên quan đến module Vocabulary hiện tại**.
- Vocabulary hiện tại giữ nguyên workflow/list/review riêng của nó.
- Sau này nếu cần từ vựng trong Ghi nhớ thì sẽ làm **vocabulary data riêng cho Memory**.
- Khi user bấm `Thêm vào ghi nhớ` ở Grammar/Kanji/Vocabulary tương lai, module Ghi nhớ sẽ nhận dữ liệu nguồn và tạo **bản ghi ôn tập riêng cho user**.
- Mỗi loại có màn review riêng: Kanji review riêng, Từ vựng review riêng, Ngữ pháp review riêng.
- Không ôn trộn Kanji/Từ vựng/Ngữ pháp trong cùng một session.

Module này dùng **flashcard + lặp lại ngắt quãng** để đưa kiến thức từ trí nhớ ngắn hạn sang trí nhớ dài hạn.

---

## 2. Product Decisions

### 2.1 Memory là bounded context riêng

Ghi nhớ không chỉ là UI gom dữ liệu từ module khác. Nó là một domain riêng:

```text
Source module
-> Add to memory
-> Memory tạo snapshot data riêng cho user
-> User review bằng Memory SRS
```

Ví dụ với Grammar:

```text
GrammarPattern
-> user bấm "Thêm vào ghi nhớ"
-> tạo UserMemoryGrammarItem
-> review/update UserMemoryGrammarItem
```

Sau khi item đã vào Memory, việc ôn tập không update trực tiếp `UserGrammarProgress` hoặc progress của module gốc.

### 2.2 Source data và Memory data tách nhau

Source module chịu trách nhiệm:

- quản lý content gốc
- hiển thị bài học/chi tiết
- cung cấp dữ liệu khi user add vào ghi nhớ

Memory module chịu trách nhiệm:

- lưu bản ghi học riêng theo user
- lưu front/back flashcard
- lưu level/status/SRS schedule
- thống kê
- review session

Nếu content gốc thay đổi sau khi đã add vào Memory, có 2 lựa chọn:

- Phase đầu: giữ snapshot cũ trong Memory, không auto-sync.
- Phase sau: thêm nút `Cập nhật từ nội dung gốc` hoặc sync có kiểm soát.

Mặc định phase đầu chọn **snapshot cũ ổn định** để tránh làm thay đổi card user đang học.

### 2.3 Review riêng theo từng loại

Dashboard `/memory` có 3 tab:

```text
Kanji
Từ vựng
Ngữ pháp
```

Nhưng khi review thì đi vào màn riêng:

```text
/memory/kanji/review
/memory/vocabulary/review
/memory/grammar/review
```

Không có mode ôn trộn:

```text
Kanji + Từ vựng + Ngữ pháp mixed session = out of scope
```

Lý do:

- Mỗi loại card có mặt trước/mặt sau khác nhau.
- Kanji cần on/kun/stroke/example.
- Từ vựng cần word/reading/type/meaning/example.
- Ngữ pháp cần pattern/structure/meaning/usage/example.
- Review riêng giúp UI rõ ràng hơn và code ít điều kiện hơn.

---

## 3. Brainstorm Options

### Option A: Ghi nhớ chỉ aggregate progress từ module gốc

Backend giữ:

```text
UserWordProgress
UserKanjiProgress
UserGrammarProgress
```

Memory chỉ query các bảng này để hiện dashboard.

Pros:

- Ít bảng mới.
- Tận dụng code cũ.

Cons:

- Không đúng yêu cầu tách khỏi Vocabulary hiện tại.
- Memory bị phụ thuộc lifecycle của từng module.
- Khó tạo vocabulary riêng cho Memory trong tương lai.
- Khi review ở Memory vẫn có thể làm thay đổi progress module gốc.

Effort: Low

---

### Option B: Một bảng polymorphic chung `UserMemoryItems`

Tạo một bảng chung:

```text
UserMemoryItems
├── user_id
├── item_type        // kanji | vocabulary | grammar
├── source_item_id
├── front_json
├── back_json
├── level
├── status
├── next_review_at
```

Pros:

- Tách khỏi module gốc.
- Query summary chung đơn giản.
- Dễ thêm loại item mới.

Cons:

- `front_json/back_json` dễ lỏng schema.
- Mỗi loại card vẫn cần logic render riêng.
- Khó validate required fields bằng database.
- Khi cần query sâu từng loại sẽ phải parse JSON hoặc branch nhiều.

Effort: Medium

---

### Option C: Memory sở hữu bảng riêng cho từng loại

Tạo data riêng trong Memory:

```text
UserMemoryKanjiItems
UserMemoryVocabularyItems
UserMemoryGrammarItems
```

Mỗi bảng lưu snapshot content + SRS state riêng.

Pros:

- Đúng yêu cầu tách biệt module.
- Không phụ thuộc Vocabulary hiện tại.
- Mỗi loại có schema rõ ràng.
- Mỗi màn review riêng, ít conditional logic.
- Dễ tối ưu query/statistics từng loại.

Cons:

- Nhiều bảng hơn.
- Có một ít duplication SRS fields.
- Cần service add-to-memory riêng cho từng loại.

Effort: Medium

---

## 4. Recommendation

Chọn **Option C: Memory sở hữu bảng riêng cho từng loại**.

Đây là hướng đúng với yêu cầu hiện tại:

- Ghi nhớ không phụ thuộc module Vocabulary hiện tại.
- Mỗi user có bản ghi ôn tập riêng.
- Mỗi loại có review screen riêng.
- Source module chỉ là nơi phát sinh dữ liệu ban đầu.
- Sau này thêm Vocabulary riêng của Memory không cần đụng vào Vocabulary cũ.

Kiến trúc mục tiêu:

```text
GrammarPattern
  -> AddToMemoryGrammar
  -> UserMemoryGrammarItem
  -> /memory/grammar/review

KanjiItem
  -> AddToMemoryKanji
  -> UserMemoryKanjiItem
  -> /memory/kanji/review

MemoryVocabularySource
  -> AddToMemoryVocabulary
  -> UserMemoryVocabularyItem
  -> /memory/vocabulary/review
```

---

## 5. Main Screen

Route:

```text
/memory
```

Tên hiển thị:

```text
Ghi nhớ
```

Màn chính gồm:

```text
Header
├── Title: Thống kê học tập
├── Subtitle: Theo dõi tiến độ và kế hoạch ôn tập của bạn

Tabs
├── Kanji
├── Từ vựng
└── Ngữ pháp

Stats của tab active
├── Cần ôn
├── Mới thêm
├── Đang học (< 6 phút)
├── Mới thuộc (ngắn hạn < 21 ngày)
├── Đã thuộc (dài hạn)
└── Tổng đã học qua

Panels
├── [Loại item] cần ôn hôm nay
└── Lượt ôn tập tiếp theo
```

Badge trên tab = số item `due` của loại đó trong bảng Memory tương ứng.

---

## 6. Memory Categories

### 6.1 Kanji

Memory table:

```text
UserMemoryKanjiItems
```

Card front:

```text
日
Onyomi: ニチ / ジツ
Kunyomi: ひ / か
```

Card back:

```text
Nghĩa: ngày, mặt trời
Ví dụ: 日本, 今日
Ghi chú: ...
```

Source reference:

```text
sourceKanjiId nullable
sourceType = kanji
```

Memory vẫn lưu snapshot:

```text
character
onyomi
kunyomi
meaning
examplesJson
notes
```

---

### 6.2 Từ vựng

Memory table:

```text
UserMemoryVocabularyItems
```

Lưu ý:

```text
Không dùng VocabularyItem/UserWordProgress hiện tại.
```

Card front:

```text
行きます
いきます
Động từ nhóm 1
```

Card back:

```text
Đi
学校に行きます。
Tôi đi đến trường.
```

Memory vocabulary source sẽ làm sau. Khi có source đó, Memory lưu snapshot:

```text
word
reading
wordType
meaning
exampleSentence
exampleMeaning
sourceVocabularyId nullable
```

---

### 6.3 Ngữ pháp

Memory table:

```text
UserMemoryGrammarItems
```

Card front:

```text
～てもいいです
Cấu trúc: Vてもいいです
```

Card back:

```text
Nghĩa: Có thể / được phép làm gì
Ví dụ: 写真を撮ってもいいです。
Bạn có thể chụp ảnh.
```

Source reference:

```text
sourceGrammarPatternId nullable
sourceType = grammar
```

Memory vẫn lưu snapshot:

```text
pattern
title
meaning
structure
usageScope
formation
exampleJapanese
exampleReading
exampleMeaning
notes
```

---

## 7. SRS Rating Buttons

Flashcard dùng 4 mức giống ảnh:

| Button | Quality | Next review mặc định | Ý nghĩa |
|---|---:|---|---|
| Quên rồi | 1 | 1 phút | Không nhớ, cần học lại ngay |
| Khó | 3 | 6 phút | Nhớ mơ hồ, cần lặp lại ngắn |
| Tốt | 4 | 1 ngày | Nhớ được, chuyển sang review |
| Dễ | 5 | 4 ngày | Nhớ rõ, tăng interval nhanh |

UI:

```text
[Quên rồi] 1 phút
[Khó]      6 phút
[Tốt]      1 ngày
[Dễ]       4 ngày
```

Màu đề xuất:

```text
Quên rồi -> red/pink
Khó      -> orange
Tốt      -> green
Dễ       -> blue
```

---

## 8. SRS Levels

Memory dùng level thống nhất `0..5` cho cả 3 bảng:

| Level | Status | Nhóm hiển thị | Ý nghĩa |
|---:|---|---|---|
| 0 | new | Mới thêm | Vừa được thêm vào ghi nhớ |
| 1 | learning | Đang học | Vừa quên/khó, interval rất ngắn |
| 2 | learning | Đang học | Nhớ sơ bộ, còn trong giai đoạn ngắn |
| 3 | review | Mới thuộc | Đã qua learning, interval < 21 ngày |
| 4 | review | Mới thuộc | Đang ổn định |
| 5 | mastered | Đã thuộc | Trí nhớ dài hạn |

Mapping thống kê:

```text
Cần ôn                    -> nextReviewAt <= now and isActive = true
Mới thêm                  -> level = 0
Đang học (< 6 phút)       -> level in (1, 2) and intervalMinutes <= 6
Mới thuộc (< 21 ngày)     -> level in (3, 4) and intervalDays < 21
Đã thuộc (dài hạn)        -> level = 5 or intervalDays >= 21
Tổng đã học qua           -> repetitions > 0
```

---

## 9. Scheduling Rules

### 9.1 Initial Add

Khi item được thêm vào Memory:

```text
level = 0
status = new
repetitions = 0
easeFactor = 2.5
intervalMinutes = 0
intervalDays = 0
nextReviewAt = now
lastReviewedAt = null
lapseCount = 0
learningStepIndex = 0
isActive = true
addedAt = now
```

Không tạo duplicate nếu cùng user đã có item cùng loại từ cùng source:

```text
unique(userId, sourceType, sourceItemId)
```

Nếu item được add thủ công trong tương lai, dùng unique theo normalized content:

```text
unique(userId, normalizedKey)
```

---

### 9.2 Answer Rules

#### Quên rồi

```text
quality = 1
nextReviewAt = now + 1 minute
level = max(1, currentLevel - 2)
status = relearning if currentLevel >= 3 else learning
lapseCount += 1
```

Card quên được đưa lại cuối queue trong cùng session.

#### Khó

```text
quality = 3
nextReviewAt = now + 6 minutes
level = max(1, currentLevel)
status = learning if level <= 2 else review
```

#### Tốt

```text
quality = 4
nextReviewAt = now + recommendedInterval
level = min(5, currentLevel + 1)
status = review/mastered theo level
```

Với item mới:

```text
level 0 -> level 2
nextReviewAt = now + 1 day
```

#### Dễ

```text
quality = 5
nextReviewAt = now + longerInterval
level = min(5, currentLevel + 2)
status = review/mastered theo level
```

Với item mới:

```text
level 0 -> level 3
nextReviewAt = now + 4 days
```

---

### 9.3 Default Intervals

| Rating | New/Learning interval |
|---|---|
| Quên rồi | 1 phút |
| Khó | 6 phút |
| Tốt | 1 ngày |
| Dễ | 4 ngày |

Sau khi item đã vào review:

```text
Tốt -> intervalDays * easeFactor
Dễ  -> intervalDays * easeFactor * 1.3
Khó -> intervalDays * 0.6, tối thiểu 6 phút hoặc 1 ngày tùy level
Quên rồi -> learning step ngắn
```

Không auto-demote item chỉ vì quá hạn. Chỉ thay đổi level khi user trả lời.

---

## 10. Review Session Flow

```text
/memory
-> chọn tab Kanji / Từ vựng / Ngữ pháp
-> bấm Ôn tập ngay
-> điều hướng sang review screen riêng
-> load due cards của loại đó từ bảng Memory riêng
-> flashcard front
-> user bấm Lật card
-> flashcard back
-> user chọn Quên rồi / Khó / Tốt / Dễ
-> backend update Memory item
-> next card
-> session summary
```

Routes:

```text
/memory/kanji/review
/memory/vocabulary/review
/memory/grammar/review
```

Session summary:

```text
Tổng card
Số lần Quên rồi
Số lần Khó
Số lần Tốt
Số lần Dễ
Thời gian học
Lượt ôn tiếp theo gần nhất
```

---

## 11. Filters

Trong mỗi tab:

```text
Cần ôn hôm nay
Mới thêm
Đang học
Mới thuộc
Đã thuộc
Tất cả
```

Phase đầu chỉ cần:

```text
Cần ôn hôm nay
Tất cả
```

---

## 12. Backend Design

### 12.1 Domain Structure

```text
server/JPLearn.Core/Memory/
├── IMemoryService.cs
├── IMemoryGrammarService.cs
├── IMemoryKanjiService.cs
├── IMemoryVocabularyService.cs
├── Entities/
│   ├── UserMemoryGrammarItem.cs
│   ├── UserMemoryKanjiItem.cs
│   ├── UserMemoryVocabularyItem.cs
│   └── MemoryReviewSession.cs
└── DTOs/
    ├── MemorySummaryDto.cs
    ├── MemoryCardDto.cs
    ├── AddGrammarToMemoryDto.cs
    ├── AddKanjiToMemoryDto.cs
    ├── AddVocabularyToMemoryDto.cs
    ├── SubmitMemoryAnswerDto.cs
    └── MemoryAnswerResultDto.cs
```

### 12.2 Shared SRS Fields

Mỗi Memory item table có các field SRS giống nhau:

```text
id
user_id
level
status
repetitions
ease_factor
interval_minutes
interval_days
next_review_at
last_reviewed_at
lapse_count
learning_step_index
is_active
added_at
updated_at
```

Không dùng `UserWordProgress`, `UserGrammarProgress`, hoặc `UserKanjiProgress` làm nguồn review cho Memory.

### 12.3 UserMemoryGrammarItems

```text
id
user_id
source_grammar_pattern_id nullable
source_version nullable
pattern
title
meaning
structure
usage_scope
formation
example_japanese
example_reading
example_meaning
notes
tags_json
SRS fields...
```

### 12.4 UserMemoryKanjiItems

```text
id
user_id
source_kanji_id nullable
source_version nullable
character
onyomi
kunyomi
meaning
stroke_count
examples_json
notes
SRS fields...
```

### 12.5 UserMemoryVocabularyItems

```text
id
user_id
source_vocabulary_id nullable
source_version nullable
word
reading
word_type
meaning
example_sentence
example_meaning
notes
SRS fields...
```

Phase đầu có thể tạo bảng này nhưng chưa dùng, hoặc để Phase Vocabulary riêng làm sau.

### 12.6 Shared DTO

```csharp
public sealed class MemoryCardDto
{
    public Guid Id { get; set; }                  // Memory item id
    public string ItemType { get; set; } = "";    // kanji | vocabulary | grammar
    public string FrontPrimary { get; set; } = "";
    public string? FrontSecondary { get; set; }
    public string? FrontMeta { get; set; }
    public string BackPrimary { get; set; } = "";
    public string? BackSecondary { get; set; }
    public string? Example { get; set; }
    public string? ExampleMeaning { get; set; }
    public int Level { get; set; }
    public string Status { get; set; } = "";
    public DateTime NextReviewAt { get; set; }
}
```

### 12.7 API Endpoints

Dashboard:

```text
GET /api/memory/summary
GET /api/memory/grammar/summary
GET /api/memory/kanji/summary
GET /api/memory/vocabulary/summary
```

Add to Memory:

```text
POST /api/memory/grammar/from-pattern/{patternId}
POST /api/memory/kanji/from-kanji/{kanjiId}
POST /api/memory/vocabulary
```

Review:

```text
GET  /api/memory/grammar/cards?scope=due
GET  /api/memory/kanji/cards?scope=due
GET  /api/memory/vocabulary/cards?scope=due

POST /api/memory/grammar/answer
POST /api/memory/kanji/answer
POST /api/memory/vocabulary/answer
```

Reset/remove:

```text
POST   /api/memory/grammar/reset
POST   /api/memory/kanji/reset
POST   /api/memory/vocabulary/reset
DELETE /api/memory/grammar/{memoryItemId}
DELETE /api/memory/kanji/{memoryItemId}
DELETE /api/memory/vocabulary/{memoryItemId}
```

Delete mặc định là soft delete:

```text
isActive = false
```

Answer payload:

```json
{
  "memoryItemId": "uuid",
  "quality": 1,
  "sessionId": "uuid"
}
```

Response:

```json
{
  "memoryItemId": "uuid",
  "itemType": "grammar",
  "level": 2,
  "status": "learning",
  "intervalMinutes": 6,
  "intervalDays": 0,
  "nextReviewAt": "2026-05-08T05:06:00Z",
  "message": "Hẹn ôn lại sau 6 phút"
}
```

---

## 13. Frontend Design

### 13.1 File Structure

```text
client/src/features/memory/
├── memory.routes.tsx
├── api/
│   └── memoryApi.ts
├── types/
│   └── memory.types.ts
├── pages/
│   ├── MemoryDashboardPage.tsx
│   ├── MemoryGrammarReviewPage.tsx
│   ├── MemoryKanjiReviewPage.tsx
│   └── MemoryVocabularyReviewPage.tsx
└── components/
    ├── MemoryTabs.tsx
    ├── MemoryStatsBar.tsx
    ├── MemoryDuePanel.tsx
    ├── MemoryNextReviewPanel.tsx
    ├── GrammarMemoryFlashcard.tsx
    ├── KanjiMemoryFlashcard.tsx
    ├── VocabularyMemoryFlashcard.tsx
    └── MemoryRatingButtons.tsx
```

### 13.2 UI States

Dashboard:

```text
loading
empty
has due cards
no due cards
error
```

Review:

```text
loading cards
front side
back side
submitting answer
completed
empty due queue
```

### 13.3 Empty States

Nếu tab không có item:

```text
Chưa có ngữ pháp trong Ghi nhớ
Hãy mở một mẫu ngữ pháp và bấm "Thêm vào ghi nhớ".
```

```text
Chưa có Kanji trong Ghi nhớ
Hãy mở một chữ Kanji và bấm "Thêm vào ghi nhớ".
```

```text
Chưa có từ vựng trong Ghi nhớ
Từ vựng cho Ghi nhớ sẽ được thêm ở module Vocabulary riêng sau.
```

---

## 14. Data Aggregation

`GET /api/memory/summary` query trực tiếp các bảng Memory:

```text
UserMemoryKanjiItems
UserMemoryVocabularyItems
UserMemoryGrammarItems
```

Response:

```json
{
  "kanji": {
    "due": 0,
    "new": 0,
    "learning": 0,
    "shortTerm": 0,
    "longTerm": 0,
    "totalStudied": 0,
    "nextReviewAt": null
  },
  "vocabulary": {
    "due": 0,
    "new": 0,
    "learning": 0,
    "shortTerm": 0,
    "longTerm": 0,
    "totalStudied": 0,
    "nextReviewAt": null
  },
  "grammar": {
    "due": 1,
    "new": 1,
    "learning": 0,
    "shortTerm": 0,
    "longTerm": 0,
    "totalStudied": 1,
    "nextReviewAt": "2026-05-08T05:01:00Z"
  }
}
```

Frontend tính tab badge từ `due`.

---

## 15. Implementation Phases

### Phase 1: Memory core + Grammar snapshot

Lý do: Grammar đã có content/detail page và ảnh mẫu đang active tab Ngữ pháp.

Tasks:

- [ ] Tạo `server/JPLearn.Core/Memory`.
- [ ] Tạo `UserMemoryGrammarItem`.
- [ ] Tạo `MemoryReviewSession`.
- [ ] Tạo migration cho Memory tables.
- [ ] Tạo endpoint `POST /api/memory/grammar/from-pattern/{patternId}`.
- [ ] Khi add, copy snapshot từ `GrammarPattern` sang `UserMemoryGrammarItem`.
- [ ] Không update `UserGrammarProgress` khi review trong Memory.
- [ ] Tạo `/memory`.
- [ ] Tạo `/memory/grammar/review`.
- [ ] Flashcard Ngữ pháp dùng 4 nút Quên rồi/Khó/Tốt/Dễ.

Verification:

```text
Open grammar pattern
-> Add to memory
-> UserMemoryGrammarItem được tạo
-> /memory tab Ngữ pháp badge tăng
-> /memory/grammar/review
-> chọn rating
-> UserMemoryGrammarItem.nextReviewAt thay đổi
-> UserGrammarProgress không bị thay đổi
```

---

### Phase 2: Memory Kanji snapshot

Tasks:

- [ ] Tạo `UserMemoryKanjiItem`.
- [ ] Tạo endpoint `POST /api/memory/kanji/from-kanji/{kanjiId}`.
- [ ] Khi add, copy snapshot từ Kanji source sang `UserMemoryKanjiItem`.
- [ ] Tạo `/memory/kanji/review`.
- [ ] Tạo `KanjiMemoryFlashcard`.

Verification:

```text
Open kanji detail
-> Add to memory
-> UserMemoryKanjiItem được tạo
-> /memory tab Kanji hiện due
-> review update UserMemoryKanjiItem
```

---

### Phase 3: Memory Vocabulary riêng

Tasks:

- [ ] Thiết kế source vocabulary riêng cho Memory.
- [ ] Tạo `UserMemoryVocabularyItem`.
- [ ] Không dùng `VocabularyItem`/`UserWordProgress` hiện tại.
- [ ] Tạo endpoint add vocabulary vào Memory.
- [ ] Tạo `/memory/vocabulary/review`.
- [ ] Tạo `VocabularyMemoryFlashcard`.

Verification:

```text
Add vocabulary từ source Memory vocabulary
-> UserMemoryVocabularyItem được tạo
-> /memory tab Từ vựng hiện due
-> review update UserMemoryVocabularyItem
-> Vocabulary module hiện tại không đổi
```

---

### Phase 4: Full dashboard and history

Tasks:

- [ ] Tổng hợp summary từ 3 Memory tables.
- [ ] Lưu `MemoryReviewSession` theo item type.
- [ ] Hiển thị lịch sử phiên ôn.
- [ ] Hiển thị biểu đồ 21 ngày.

Verification:

```text
Complete review session
-> MemoryReviewSession saved
-> dashboard stats refresh
-> 21-day chart cập nhật
```

---

## 16. Acceptance Criteria

- [ ] User vào `/memory` thấy 3 tab Kanji/Từ vựng/Ngữ pháp.
- [ ] Mỗi tab hiển thị badge số Memory item cần ôn.
- [ ] User thấy thống kê: cần ôn, mới thêm, đang học, mới thuộc, đã thuộc, tổng đã học qua.
- [ ] User bấm `Ôn tập ngay` ở tab Ngữ pháp thì vào `/memory/grammar/review`.
- [ ] User bấm `Ôn tập ngay` ở tab Kanji thì vào `/memory/kanji/review`.
- [ ] User bấm `Ôn tập ngay` ở tab Từ vựng thì vào `/memory/vocabulary/review`.
- [ ] Không có session ôn trộn 3 loại.
- [ ] Flashcard có mặt trước, mặt sau, nút lật card.
- [ ] Sau khi lật card, hiện 4 nút: Quên rồi, Khó, Tốt, Dễ.
- [ ] `Quên rồi` hẹn lại sau 1 phút.
- [ ] `Khó` hẹn lại sau 6 phút.
- [ ] `Tốt` hẹn lại sau 1 ngày ở learning stage.
- [ ] `Dễ` hẹn lại sau 4 ngày ở learning stage.
- [ ] Add từ Grammar tạo `UserMemoryGrammarItem` riêng cho user.
- [ ] Review trong Memory Grammar chỉ update `UserMemoryGrammarItem`.
- [ ] Review trong Memory Kanji chỉ update `UserMemoryKanjiItem`.
- [ ] Review trong Memory Vocabulary chỉ update `UserMemoryVocabularyItem`.
- [ ] Memory không dùng `UserWordProgress` của Vocabulary hiện tại.
- [ ] Memory không tạo/sửa/xóa Vocabulary list hiện tại.
- [ ] Không tạo duplicate Memory item khi user bấm Add nhiều lần.
- [ ] Remove khỏi Ghi nhớ là soft delete `isActive = false`.

---

## 17. Open Questions

- Khi content gốc Grammar/Kanji thay đổi, Memory snapshot có cần sync thủ công không?
- Có cần tạo bảng base/abstract để reuse SRS fields, hay chỉ duplicate fields trong 3 entity cho đơn giản?
- Phase 1 có cần tạo sẵn bảng Vocabulary Memory không, hay đợi module Vocabulary riêng?
- `Add to study` ở Grammar hiện tại có đổi thành `Add to memory` luôn không?

---

## 18. Recommended Next Step

Làm trước **Phase 1: Memory core + Grammar snapshot**.

Lý do:

- Đúng hướng tách domain ngay từ đầu.
- Grammar đã có content để copy sang Memory.
- Có thể kiểm chứng luồng snapshot riêng cho user trước.
- Tránh phụ thuộc vào Vocabulary module hiện tại.
