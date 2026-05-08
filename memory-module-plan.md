# JPLearn Memory Module — Project Plan

## 1. Scope Hiện Tại

Triển khai module **Ghi nhớ** theo hướng bounded context riêng:

```text
Backend Memory Core
-> Backend Memory Grammar Snapshot
-> Backend Memory SRS
-> Frontend Memory Dashboard
-> Frontend Memory Grammar Review
-> Later: Memory Kanji
-> Later: Memory Vocabulary riêng
```

Điểm bắt buộc:

- Memory không dùng Vocabulary module hiện tại.
- Memory không dùng `UserWordProgress` hiện tại.
- Memory review không update `UserGrammarProgress`.
- Khi add từ Grammar/Kanji/Vocabulary tương lai, Memory tạo data snapshot riêng cho user.
- Mỗi loại review riêng, không gộp chung session.

Phase đầu chỉ làm:

```text
Memory Core + Grammar Snapshot + Grammar Review
```

Kanji và Vocabulary riêng của Memory làm sau.

---

## 2. Out Of Scope Phase Này

Không làm:

- Không gộp review Kanji/Từ vựng/Ngữ pháp trong một session.
- Không dùng Vocabulary list hiện tại.
- Không sửa workflow SRS của Vocabulary hiện tại.
- Không dùng `UserWordProgress` làm data cho Memory.
- Không tạo Memory Vocabulary source trong phase đầu.
- Không làm Kanji source nếu Kanji module chưa sẵn sàng.
- Không làm payment/premium.
- Không auto-sync Memory snapshot khi content gốc thay đổi.
- Không hard delete Memory item khi remove khỏi Ghi nhớ.
- Không làm biểu đồ 21 ngày ở phase đầu.

Có thể tạo sẵn enum/string constants và DTO để sau này thêm Kanji/Vocabulary không phải refactor lớn.

---

## 3. Kiến Trúc Mục Tiêu

### Backend

```text
server/JPLearn.Core/Memory/
├── IMemoryService.cs
├── IMemoryGrammarService.cs
├── IMemorySrsService.cs
├── MemoryConstants.cs
├── Entities/
│   ├── UserMemoryGrammarItem.cs
│   ├── UserMemoryKanjiItem.cs              // later
│   ├── UserMemoryVocabularyItem.cs         // later
│   └── MemoryReviewSession.cs
└── DTOs/
    ├── MemorySummaryDto.cs
    ├── MemoryTypeSummaryDto.cs
    ├── MemoryCardDto.cs
    ├── AddGrammarToMemoryResultDto.cs
    ├── SubmitMemoryAnswerDto.cs
    └── MemoryAnswerResultDto.cs

server/JPLearn.Infrastructure/Services/
├── MemoryService.cs
├── MemoryGrammarService.cs
└── MemorySrsService.cs

server/JPLearn.Infrastructure/Data/Configurations/
└── MemoryConfigurations.cs

server/JPLearn.Api/Controllers/
└── MemoryController.cs
```

### Frontend

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
│   ├── MemoryKanjiReviewPage.tsx          // placeholder/later
│   └── MemoryVocabularyReviewPage.tsx     // placeholder/later
└── components/
    ├── MemoryTabs.tsx
    ├── MemoryStatsBar.tsx
    ├── MemoryDuePanel.tsx
    ├── MemoryNextReviewPanel.tsx
    ├── GrammarMemoryFlashcard.tsx
    ├── MemoryRatingButtons.tsx
    └── MemorySessionSummary.tsx
```

---

## 4. Backend Phase 1 — Memory Domain Entities

### Mục tiêu

Tạo schema riêng cho Memory, bắt đầu với Grammar snapshot.

### Tasks

- [ ] Tạo folder `server/JPLearn.Core/Memory`.
- [ ] Tạo `MemoryConstants`.
- [ ] Tạo `UserMemoryGrammarItem`.
- [ ] Tạo `MemoryReviewSession`.
- [ ] Tạo DTO summary/card/answer.
- [ ] Add `DbSet<UserMemoryGrammarItem>` vào `AppDbContext`.
- [ ] Add `DbSet<MemoryReviewSession>` vào `AppDbContext`.
- [ ] Tạo `MemoryConfigurations`.
- [ ] Add migration `AddMemoryModule`.
- [ ] Verify migration tạo đúng bảng.

### Entity Summary

```text
UserMemoryGrammarItems
├── id
├── user_id
├── source_grammar_pattern_id nullable
├── source_version nullable
├── pattern
├── title
├── meaning
├── structure
├── usage_scope
├── formation
├── example_japanese
├── example_reading
├── example_meaning
├── notes
├── tags_json
├── level                  // 0..5
├── status                 // new|learning|review|mastered|relearning
├── repetitions
├── ease_factor
├── interval_minutes
├── interval_days
├── next_review_at
├── last_reviewed_at
├── lapse_count
├── learning_step_index
├── is_active
├── added_at
├── updated_at
```

```text
MemoryReviewSessions
├── id
├── user_id
├── item_type              // grammar|kanji|vocabulary
├── scope                  // due|all|new|learning|short_term|long_term
├── total_cards
├── again_count
├── hard_count
├── good_count
├── easy_count
├── duration_seconds
├── started_at
├── completed_at
```

### Constraints

```text
unique(user_id, source_grammar_pattern_id) where source_grammar_pattern_id is not null
index(user_id, is_active, next_review_at)
index(user_id, level)
```

### Verification

```text
dotnet build
dotnet ef migrations add AddMemoryModule
dotnet ef database update
```

---

## 5. Backend Phase 2 — Add Grammar To Memory

### Mục tiêu

Từ Grammar detail, user bấm `Thêm vào ghi nhớ` và Memory tạo snapshot riêng.

### Tasks

- [ ] Tạo `IMemoryGrammarService`.
- [ ] Implement `MemoryGrammarService`.
- [ ] Inject `AppDbContext`.
- [ ] Load `GrammarPattern` + examples.
- [ ] Copy fields từ `GrammarPattern` sang `UserMemoryGrammarItem`.
- [ ] Nếu user đã add pattern đó, không tạo duplicate.
- [ ] Nếu item đang `isActive = false`, bật lại `isActive = true`.
- [ ] Tạo endpoint add vào `MemoryController`.
- [ ] Update Grammar frontend button gọi Memory API thay vì Grammar SRS API.

### Endpoint

```text
POST /api/memory/grammar/from-pattern/{patternId}
```

### Rules

- Source `GrammarPattern` chỉ dùng để tạo snapshot.
- Sau khi snapshot được tạo, review trong Memory không update `UserGrammarProgress`.
- Nếu pattern có nhiều examples, phase đầu lấy example đầu tiên theo `orderIndex`.
- `nextReviewAt = now` để item xuất hiện ngay ở `/memory`.

### Response

```json
{
  "memoryItemId": "uuid",
  "sourceGrammarPatternId": "uuid",
  "alreadyExists": false,
  "isActive": true
}
```

### Verification

```text
Open grammar pattern
-> click Thêm vào ghi nhớ
-> DB có UserMemoryGrammarItem
-> bấm lại không tạo duplicate
-> UserGrammarProgress không bị tạo/sửa bởi flow này
```

---

## 6. Backend Phase 3 — Memory Summary API

### Mục tiêu

Dashboard `/memory` hiển thị thống kê theo tab, phase đầu có data thật cho Grammar.

### Tasks

- [ ] Tạo `IMemoryService`.
- [ ] Implement `MemoryService`.
- [ ] Query summary từ `UserMemoryGrammarItems`.
- [ ] Trả Kanji/Vocabulary summary là zero placeholder trong phase đầu.
- [ ] Tạo endpoint `GET /api/memory/summary`.
- [ ] Tạo endpoint `GET /api/memory/grammar/summary`.

### Endpoints

```text
GET /api/memory/summary
GET /api/memory/grammar/summary
```

### Summary Rules

```text
due          -> nextReviewAt <= now and isActive = true
new          -> level = 0 and isActive = true
learning     -> level in (1, 2) and isActive = true
shortTerm    -> level in (3, 4) and intervalDays < 21 and isActive = true
longTerm     -> level = 5 or intervalDays >= 21 and isActive = true
totalStudied -> repetitions > 0 and isActive = true
nextReviewAt -> min(nextReviewAt) where isActive = true
```

### Response

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
    "totalStudied": 0,
    "nextReviewAt": "2026-05-08T05:00:00Z"
  }
}
```

### Verification

```text
Add 1 grammar item to memory
-> GET /api/memory/summary returns grammar.due = 1
-> vocabulary and kanji remain 0
```

---

## 7. Backend Phase 4 — Memory Grammar Cards API

### Mục tiêu

Review screen lấy card từ `UserMemoryGrammarItems`.

### Tasks

- [ ] Tạo `MemoryCardDto`.
- [ ] Implement get grammar cards by scope.
- [ ] Support `scope=due`.
- [ ] Support `scope=all`.
- [ ] Sort due cards by `nextReviewAt`, then `addedAt`.
- [ ] Ignore inactive items.

### Endpoint

```text
GET /api/memory/grammar/cards?scope=due
```

### Card DTO

```json
{
  "id": "memory-item-id",
  "itemType": "grammar",
  "frontPrimary": "～てもいいです",
  "frontSecondary": "Vてもいいです",
  "frontMeta": "Có thể / được phép làm gì",
  "backPrimary": "Có thể / được phép làm gì",
  "backSecondary": "Vてもいいです",
  "example": "写真を撮ってもいいです。",
  "exampleMeaning": "Bạn có thể chụp ảnh.",
  "level": 0,
  "status": "new",
  "nextReviewAt": "2026-05-08T05:00:00Z"
}
```

### Verification

```text
Add grammar item
-> GET /api/memory/grammar/cards?scope=due returns card
-> set nextReviewAt future
-> due no longer returns card
-> scope=all still returns card
```

---

## 8. Backend Phase 5 — Memory SRS Answer

### Mục tiêu

Submit answer từ Memory review và update chỉ `UserMemoryGrammarItem`.

### Tasks

- [ ] Tạo `IMemorySrsService`.
- [ ] Implement `MemorySrsService`.
- [ ] Support 4 quality levels: 1, 3, 4, 5.
- [ ] Update `level/status/repetitions/easeFactor/intervalMinutes/intervalDays/nextReviewAt`.
- [ ] Increment `lapseCount` khi quality = 1.
- [ ] Set `lastReviewedAt = now`.
- [ ] Return readable message.
- [ ] Tạo endpoint `POST /api/memory/grammar/answer`.

### Endpoint

```text
POST /api/memory/grammar/answer
```

### Payload

```json
{
  "memoryItemId": "uuid",
  "quality": 4,
  "sessionId": "uuid"
}
```

### Scheduling Rules

```text
Quên rồi quality=1 -> now + 1 minute
Khó      quality=3 -> now + 6 minutes
Tốt      quality=4 -> now + 1 day for new/learning
Dễ       quality=5 -> now + 4 days for new/learning
```

Level changes:

```text
quality=1 -> level = max(1, currentLevel - 2)
quality=3 -> level = max(1, currentLevel)
quality=4 -> level = min(5, currentLevel + 1)
quality=5 -> level = min(5, currentLevel + 2)
```

Status mapping:

```text
level 0 -> new
level 1 -> learning
level 2 -> learning
level 3 -> review
level 4 -> review
level 5 -> mastered
quality=1 and old level >= 3 -> relearning
```

### Verification

```text
New item + Quên rồi -> nextReviewAt about now + 1 minute
New item + Khó      -> nextReviewAt about now + 6 minutes
New item + Tốt      -> level 1 or 2, nextReviewAt about now + 1 day
New item + Dễ       -> level 2 or 3, nextReviewAt about now + 4 days
Grammar UserGrammarProgress unchanged
```

---

## 9. Backend Phase 6 — Remove/Reset

### Mục tiêu

User có thể remove/reset Memory item hoặc reset toàn bộ Grammar Memory.

### Tasks

- [ ] Implement soft remove by memory item id.
- [ ] Implement reset all grammar memory progress.
- [ ] Implement reset selected grammar memory items.
- [ ] Do not delete source GrammarPattern.
- [ ] Do not touch `UserGrammarProgress`.

### Endpoints

```text
DELETE /api/memory/grammar/{memoryItemId}
POST   /api/memory/grammar/reset
```

### Reset Payload

```json
{
  "scope": "all",
  "memoryItemIds": []
}
```

### Reset Rule

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
```

### Verification

```text
Remove item -> isActive = false
Reset item -> returns to new/due
Source GrammarPattern still exists
UserGrammarProgress unchanged
```

---

## 10. Frontend Phase 1 — Memory Routes & API

### Mục tiêu

Thêm feature `memory` vào frontend mà không đụng Vocabulary hiện tại.

### Tasks

- [ ] Tạo `client/src/features/memory`.
- [ ] Tạo `memory.types.ts`.
- [ ] Tạo `memoryApi.ts`.
- [ ] Tạo `memory.routes.tsx`.
- [ ] Add route `/memory`.
- [ ] Add route `/memory/grammar/review`.
- [ ] Add menu item `Ghi nhớ` vào Sidebar/Navbar.
- [ ] Không import hoặc reuse `features/vocabulary` review workspace.

### Routes

```text
/memory
/memory/grammar/review
/memory/kanji/review          // placeholder later
/memory/vocabulary/review     // placeholder later
```

### Verification

```text
npm run build
Open /memory
Open /memory/grammar/review
No import from client/src/features/vocabulary review code
```

---

## 11. Frontend Phase 2 — Memory Dashboard UI

### Mục tiêu

Tạo màn giống ảnh: header, tabs, stats, due panel, next review panel.

### Tasks

- [ ] Tạo `MemoryDashboardPage`.
- [ ] Tạo `MemoryTabs`.
- [ ] Tạo `MemoryStatsBar`.
- [ ] Tạo `MemoryDuePanel`.
- [ ] Tạo `MemoryNextReviewPanel`.
- [ ] Load `GET /api/memory/summary`.
- [ ] Tab active mặc định là `grammar` nếu grammar có due item.
- [ ] Tab Kanji/Vocabulary phase đầu hiển thị empty/coming soon.
- [ ] Button `Ôn tập ngay` điều hướng sang review route riêng.

### UI Rules

- Stats chỉ hiển thị cho tab active.
- Badge tab = due count.
- Grammar button đi `/memory/grammar/review`.
- Kanji/Vocabulary button disabled hoặc hiện empty state trong phase đầu.
- Không tạo landing page.
- Không dùng card lồng card.

### Verification

```text
No memory item -> empty state
Add grammar item -> grammar due panel shows count
Click Ôn tập ngay -> /memory/grammar/review
```

---

## 12. Frontend Phase 3 — Add Grammar To Memory Button

### Mục tiêu

Grammar detail dùng Memory API để add snapshot.

### Tasks

- [ ] Tìm button hiện tại `Thêm vào ghi nhớ | lặp lại ngắt quãng`.
- [ ] Đổi API call sang `POST /api/memory/grammar/from-pattern/{patternId}`.
- [ ] Update label:

```text
Thêm vào ghi nhớ
Đã thêm vào ghi nhớ
```

- [ ] Sau khi add thành công, giữ user ở Grammar detail.
- [ ] Optional: thêm CTA nhỏ `Ôn trong Ghi nhớ`.
- [ ] Không gọi Grammar Review/SRS API cũ trong flow này.

### Verification

```text
Open GrammarPatternDetailPage
-> click Thêm vào ghi nhớ
-> success state
-> refresh /memory summary
-> UserMemoryGrammarItem exists
```

---

## 13. Frontend Phase 4 — Grammar Memory Review

### Mục tiêu

Màn review riêng cho Memory Grammar bằng flashcard.

### Tasks

- [ ] Tạo `MemoryGrammarReviewPage`.
- [ ] Tạo `GrammarMemoryFlashcard`.
- [ ] Tạo `MemoryRatingButtons`.
- [ ] Load `GET /api/memory/grammar/cards?scope=due`.
- [ ] Render front side.
- [ ] Flip to back side.
- [ ] Show 4 buttons sau khi flip.
- [ ] Submit answer qua `POST /api/memory/grammar/answer`.
- [ ] Sau submit, chuyển card tiếp theo.
- [ ] Nếu `Quên rồi`, đưa card về cuối queue trong session.
- [ ] Hiển thị session summary khi xong.

### Flashcard Front

```text
Pattern
Structure
Meaning short
```

### Flashcard Back

```text
Meaning
Structure
Usage/Formation
Example Japanese
Example Meaning
Notes
```

### Rating Buttons

```text
Quên rồi  1 phút
Khó       6 phút
Tốt       1 ngày
Dễ        4 ngày
```

### Verification

```text
Due grammar cards render
Flip works
Each rating submits correct quality
After answer card advances
Completed state appears
Dashboard summary updates after session
```

---

## 14. Frontend Phase 5 — Placeholder Kanji/Vocabulary Screens

### Mục tiêu

Cho user thấy 3 phần đúng concept nhưng không làm nhầm là dùng Vocabulary hiện tại.

### Tasks

- [ ] Tạo placeholder `/memory/kanji/review`.
- [ ] Tạo placeholder `/memory/vocabulary/review`.
- [ ] Kanji placeholder: `Kanji trong Ghi nhớ sẽ được thêm sau khi Kanji module sẵn sàng`.
- [ ] Vocabulary placeholder: `Từ vựng trong Ghi nhớ sẽ dùng source riêng, không dùng Vocabulary hiện tại`.
- [ ] Button back về `/memory`.

### Verification

```text
Open /memory/kanji/review -> placeholder
Open /memory/vocabulary/review -> placeholder
No Vocabulary module API calls
```

---

## 15. Testing Plan

### Backend Tests

- [ ] Add grammar to memory creates snapshot.
- [ ] Add same grammar twice does not duplicate.
- [ ] Add inactive same grammar reactivates item.
- [ ] Summary counts due/new/learning correctly.
- [ ] Cards due only returns active due cards.
- [ ] Answer quality 1 schedules 1 minute.
- [ ] Answer quality 3 schedules 6 minutes.
- [ ] Answer quality 4 schedules 1 day for learning.
- [ ] Answer quality 5 schedules 4 days for learning.
- [ ] Memory answer does not update `UserGrammarProgress`.
- [ ] Soft remove sets `isActive = false`.

### Frontend Tests / Manual QA

- [ ] `/memory` loads without data.
- [ ] Add grammar to memory then `/memory` shows badge.
- [ ] Review page loads card.
- [ ] Flip card shows answer.
- [ ] Rating buttons call correct endpoint.
- [ ] Completion summary appears.
- [ ] Kanji/Vocabulary placeholders do not call old Vocabulary APIs.

### Commands

```text
dotnet build
dotnet test
npm run build
```

---

## 16. Implementation Order

Recommended order:

```text
1. Backend Memory entities + migration
2. Backend add grammar snapshot
3. Backend summary/cards/answer APIs
4. Frontend memory route + API client
5. Frontend dashboard
6. Grammar detail Add to Memory integration
7. Frontend grammar review screen
8. Remove/reset endpoints
9. Placeholder Kanji/Vocabulary screens
10. Manual QA + build/test
```

Không làm Kanji/Vocabulary implementation thật trước khi Grammar Memory snapshot chạy ổn.

---

## 17. Risk Notes

### Risk 1: Trùng logic SRS với code cũ

Memory có thể reuse công thức SRS nếu tách được pure function, nhưng không reuse service cũ nếu service đó phụ thuộc Vocabulary/Grammar progress.

Rule:

```text
Reuse algorithm pure function: OK
Reuse old progress service: No
```

### Risk 2: Add to study cũ của Grammar gây nhầm

Hiện Grammar có flow SRS riêng. Khi làm Memory, cần quyết định:

```text
Grammar SRS cũ giữ lại nhưng không dùng trong UI
hoặc
đổi button chính sang Memory
```

Phase này nên đổi button chính sang Memory để đúng yêu cầu.

### Risk 3: Snapshot bị stale

Nếu Grammar content sửa sau khi user add vào Memory, Memory card không tự đổi.

Phase đầu chấp nhận. Phase sau thêm:

```text
sourceVersion
syncFromSource
outdatedBadge
```

---

## 18. Acceptance Criteria

- [ ] `/memory` tồn tại và hiển thị 3 tab Kanji/Từ vựng/Ngữ pháp.
- [ ] Dashboard summary lấy từ Memory tables, không lấy từ Vocabulary hiện tại.
- [ ] Grammar detail có thể add pattern vào Memory.
- [ ] Add Grammar tạo `UserMemoryGrammarItem` snapshot riêng cho user.
- [ ] Add cùng pattern nhiều lần không duplicate.
- [ ] `/memory/grammar/review` chỉ load `UserMemoryGrammarItem`.
- [ ] Review Grammar Memory có flashcard front/back.
- [ ] Review Grammar Memory có 4 nút: Quên rồi, Khó, Tốt, Dễ.
- [ ] `Quên rồi` hẹn lại 1 phút.
- [ ] `Khó` hẹn lại 6 phút.
- [ ] `Tốt` hẹn lại 1 ngày ở learning stage.
- [ ] `Dễ` hẹn lại 4 ngày ở learning stage.
- [ ] Memory review không update `UserGrammarProgress`.
- [ ] Memory không gọi Vocabulary API hiện tại.
- [ ] Remove khỏi Ghi nhớ là soft delete.
- [ ] `dotnet build` pass.
- [ ] `npm run build` pass.

---

## 19. Next Step

Bắt đầu với:

```text
Backend Phase 1 — Memory Domain Entities
```

Sau khi migration ổn, làm tiếp:

```text
Backend Phase 2 — Add Grammar To Memory
```
