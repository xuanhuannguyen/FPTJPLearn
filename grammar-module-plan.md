# JPLearn Grammar Module — Project Plan

## 1. Scope Hiện Tại

Triển khai module ngữ pháp theo thứ tự:

```text
Backend Grammar Module
-> Backend Grammar SRS
-> Backend Grammar Exercises
-> Frontend Grammar UI
-> Frontend Grammar Review/Exercises
```

Premium **chưa làm trong phase này**.

Premium sẽ làm sau khi các module chính và hệ thống user hoàn thiện. Tuy nhiên khi thiết kế backend, không được viết theo cách khiến sau này thêm premium phải refactor lớn.

---

## 2. Out Of Scope Phase Này

Không làm:

- Payment.
- Subscription.
- Entitlement.
- Premium lock thật.
- Admin UI.
- User tự tạo grammar pattern.
- Gộp SRS Grammar với Vocabulary/Kanji.

Vẫn có thể để field hoặc enum chuẩn bị cho tương lai nếu không làm tăng complexity lớn, nhưng không implement business logic premium.

---

## 3. Kiến Trúc Mục Tiêu

### Backend

```text
server/JPLearn.Core/Grammar/
├── IGrammarService.cs
├── IGrammarReviewService.cs
├── IGrammarExerciseService.cs
├── Entities/
│   ├── GrammarLesson.cs
│   ├── GrammarPattern.cs
│   ├── GrammarExample.cs
│   ├── GrammarExercise.cs
│   ├── GrammarExerciseAttempt.cs
│   └── UserGrammarProgress.cs
└── DTOs/
    ├── GrammarLevelDto.cs
    ├── GrammarLessonDto.cs
    ├── GrammarPatternDto.cs
    ├── GrammarPatternDetailDto.cs
    ├── GrammarReviewCardDto.cs
    ├── SubmitGrammarAnswerDto.cs
    ├── GrammarExerciseDto.cs
    ├── CheckGrammarExerciseDto.cs
    └── AiEvaluateGrammarExerciseDto.cs

server/JPLearn.Infrastructure/Services/
├── GrammarService.cs
├── GrammarReviewService.cs
└── GrammarExerciseService.cs

server/JPLearn.Infrastructure/Data/Configurations/
└── GrammarConfigurations.cs

server/JPLearn.Api/Controllers/
└── GrammarController.cs
```

### Frontend

```text
client/src/features/grammar/
├── grammar.routes.tsx
├── api/
│   └── grammarApi.ts
├── types/
│   └── grammar.types.ts
├── pages/
│   ├── GrammarHomePage.tsx
│   ├── GrammarLevelPage.tsx
│   ├── GrammarLessonPage.tsx
│   ├── GrammarPatternPage.tsx
│   └── GrammarReviewPage.tsx
└── components/
    ├── GrammarLevelCard.tsx
    ├── GrammarLessonCard.tsx
    ├── GrammarPatternCard.tsx
    ├── GrammarExampleList.tsx
    ├── GrammarExerciseTabs.tsx
    ├── ViToJaExerciseCard.tsx
    ├── JaToViExerciseCard.tsx
    ├── ArrangeExerciseCard.tsx
    ├── AddGrammarToStudyButton.tsx
    └── GrammarReviewCard.tsx
```

---

## 4. Backend Phase 1 — Domain Entities

### Mục tiêu

Tạo schema ngữ pháp và progress riêng cho Grammar.

### Tasks

- [ ] Tạo `GrammarLesson`.
- [ ] Tạo `GrammarPattern`.
- [ ] Tạo `GrammarExample`.
- [ ] Tạo `GrammarExercise`.
- [ ] Tạo `GrammarExerciseAttempt`.
- [ ] Tạo `UserGrammarProgress`.
- [ ] Add DbSet vào `AppDbContext`.
- [ ] Add Fluent configuration.
- [ ] Add migration.
- [ ] Verify migration tạo đúng bảng.

### Entity Summary

```text
GrammarLessons
├── id
├── level
├── lesson_number
├── title
├── description
├── access_tier        // free now, premium later
├── package_code       // nullable/optional now
├── order_index

GrammarPatterns
├── id
├── lesson_id
├── level
├── pattern
├── title
├── meaning
├── structure
├── usage_scope
├── formation
├── notes
├── tags_json
├── access_tier_override
├── package_code_override
├── order_index

GrammarExamples
├── id
├── pattern_id
├── japanese
├── reading
├── meaning
├── note
├── order_index

GrammarExercises
├── id
├── pattern_id
├── exercise_type      // vi_to_ja | ja_to_vi | arrange
├── prompt
├── prompt_reading
├── expected_answer
├── acceptable_answers_json
├── hint
├── explanation
├── template_text
├── options_json
├── correct_order_json
├── star_position
├── star_answer
├── order_index

UserGrammarProgress
├── id
├── user_id
├── grammar_pattern_id
├── level              // 0..3
├── status
├── repetitions
├── ease_factor
├── interval_days
├── next_review_at
├── last_reviewed_at
├── lapse_count
├── learning_step_index
├── is_active
├── added_at

GrammarExerciseAttempts
├── id
├── user_id
├── grammar_exercise_id
├── answer_text
├── selected_option_order_json
├── is_correct
├── score
├── feedback
├── checked_by         // system | ai
```

### Verification

```text
dotnet build
dotnet ef migrations add AddGrammarModule
dotnet ef database update
```

---

## 5. Backend Phase 2 — Grammar Content API

### Mục tiêu

Cho frontend đọc level, lesson, pattern detail, examples, exercises.

### Tasks

- [ ] Tạo DTOs cho level overview.
- [ ] Tạo DTOs cho lesson list/detail.
- [ ] Tạo DTOs cho pattern list/detail.
- [ ] Implement `IGrammarService`.
- [ ] Implement `GrammarService`.
- [ ] Implement `GrammarController` content endpoints.
- [ ] Add DI registration.

### Endpoints

```text
GET /api/grammar/levels
GET /api/grammar/{level}/lessons
GET /api/grammar/lessons/{lessonId}
GET /api/grammar/patterns/{patternId}
GET /api/grammar/search?query=
```

### Rules

- Level chỉ nhận `N5`, `N4`, `N3`, `N2`, `N1`.
- Lesson sort theo `orderIndex`, sau đó `lessonNumber`.
- Pattern sort theo `orderIndex`.
- Pattern detail trả examples và exercises.
- Phase này chưa khóa premium, nhưng DTO có thể trả:

```json
{
  "accessTier": "free",
  "isLocked": false
}
```

### Verification

- [ ] Swagger gọi được tất cả endpoints.
- [ ] Pattern detail trả đủ examples/exercises.
- [ ] Search hoạt động theo pattern/title/meaning.

---

## 6. Backend Phase 3 — Grammar Study/SRS

### Mục tiêu

User có thể thêm pattern vào học và ôn bằng SRS riêng của Grammar.

### Tasks

- [ ] Tạo `IGrammarReviewService`.
- [ ] Implement add-to-study.
- [ ] Implement remove/deactivate study.
- [ ] Implement get due grammar cards.
- [ ] Implement submit answer.
- [ ] Tạo grammar SRS calculator hoặc wrapper riêng.
- [ ] Lưu attempt/result nếu cần.

### Endpoints

```text
POST   /api/grammar/patterns/{patternId}/study
DELETE /api/grammar/patterns/{patternId}/study
GET    /api/grammar/review/due
POST   /api/grammar/review/answer
GET    /api/grammar/progress
```

### Add To Study Rule

```text
if UserGrammarProgress exists:
  set isActive = true
  return existing progress
else:
  create new progress
```

Initial progress:

```text
level = 0
status = new
repetitions = 0
easeFactor = 2.5
intervalDays = 0
nextReviewAt = now
lapseCount = 0
learningStepIndex = 0
isActive = true
```

### Grammar SRS Levels

```text
Level 0 = new
Level 1 = learning
Level 2 = review
Level 3 = mastered
```

### Rating

```text
Again -> quality = 1
Hard  -> quality = 3
Good  -> quality = 5
```

### Due Query

```text
userId = current user
isActive = true
nextReviewAt <= now
```

Premium access check sẽ thêm sau. Phase này không chặn premium.

### Verification

- [ ] Add pattern không tạo trùng.
- [ ] Due trả đúng pattern đã add và đến hạn.
- [ ] Submit answer update level/status/nextReviewAt.
- [ ] Grammar progress không ảnh hưởng Vocabulary progress.

---

## 7. Backend Phase 4 — Grammar Exercises

### Mục tiêu

Mỗi pattern có bài tập:

```text
Việt -> Nhật
Nhật -> Việt
Sắp xếp câu
```

### Tasks

- [ ] Tạo `IGrammarExerciseService`.
- [ ] Implement get exercises by pattern.
- [ ] Implement check answer không dùng AI.
- [ ] Implement reveal answer.
- [ ] Lưu `GrammarExerciseAttempt`.

### Endpoints

```text
GET  /api/grammar/patterns/{patternId}/exercises
POST /api/grammar/exercises/{exerciseId}/check
GET  /api/grammar/exercises/{exerciseId}/answer
```

### Check Rules Phase Đầu

`vi_to_ja`:

- normalize whitespace
- exact match với `expectedAnswer`
- allow list trong `acceptableAnswersJson`

`ja_to_vi`:

- normalize lowercase tiếng Việt
- exact/contains basic check với expected answer
- AI sẽ đánh giá linh hoạt hơn sau

`arrange`:

- compare selected option order với `correctOrderJson`
- check `starAnswer`

### Verification

- [ ] Mỗi exercise type check được đúng/sai.
- [ ] Attempt được lưu.
- [ ] Reveal answer trả đáp án mẫu.

---

## 8. Backend Phase 5 — AI Evaluation

### Mục tiêu

Cho user dùng API key của họ để AI đánh giá bài làm.

### Important

Phase này làm sau exercise cơ bản. Không bắt buộc để module Grammar chạy.

### Tasks

- [ ] Tạo AI evaluation DTO.
- [ ] Tạo endpoint AI evaluate.
- [ ] API key truyền per request.
- [ ] Không log API key.
- [ ] Tạo prompt đánh giá tập trung vào pattern hiện tại.
- [ ] Lưu attempt với `checkedBy = ai`.

### Endpoint

```text
POST /api/grammar/exercises/{exerciseId}/ai-evaluate
```

### Payload

```json
{
  "provider": "gemini",
  "apiKey": "user-api-key",
  "answerText": "私はマイケルです。",
  "selectedOptionOrder": []
}
```

### Response

```json
{
  "score": 8,
  "isAcceptable": true,
  "correctedAnswer": "私はマイケルです。",
  "feedback": "Câu đúng.",
  "grammarNotes": ["Mẫu N は N です được dùng đúng."]
}
```

### Verification

- [ ] Không có API key thì trả lỗi rõ ràng.
- [ ] Có API key thì gọi provider thành công.
- [ ] AI feedback lưu vào attempt.
- [ ] Raw API key không xuất hiện trong logs.

---

## 9. Backend Phase 6 — Seed Data

### Mục tiêu

Có data thật để test UI.

### Tasks

- [ ] Tạo seed JSON format.
- [ ] Seed tối thiểu N5 Lesson 1.
- [ ] Mỗi lesson có ít nhất 3 patterns.
- [ ] Mỗi pattern có examples.
- [ ] Mỗi pattern có 3 loại exercise.

### Minimum Seed

```text
N5 Lesson 1
├── 〜です
├── 〜ではありません
└── 〜があります
```

### Verification

- [ ] `/api/grammar/levels` có N5 data.
- [ ] `/api/grammar/lessons/{id}` có patterns.
- [ ] `/api/grammar/patterns/{id}` có examples/exercises.

---

## 10. Frontend Phase 1 — Grammar Base UI

Chỉ bắt đầu sau khi content API ổn.

### Tasks

- [ ] Tạo `features/grammar`.
- [ ] Tạo `grammar.routes.tsx`.
- [ ] Tạo `grammarApi.ts`.
- [ ] Tạo `grammar.types.ts`.
- [ ] Add routes vào `Router.tsx`.
- [ ] Add link Grammar vào Sidebar.

### Pages

```text
/grammar
/grammar/:level
/grammar/:level/lessons/:lessonId
/grammar/patterns/:patternId
```

### Verification

- [ ] Route hoạt động.
- [ ] Sidebar mở được Grammar.
- [ ] Build frontend pass.

---

## 11. Frontend Phase 2 — Grammar Content Pages

### Tasks

- [ ] Build `GrammarHomePage`.
- [ ] Build `GrammarLevelPage`.
- [ ] Build `GrammarLessonPage`.
- [ ] Build `GrammarPatternPage`.
- [ ] Build level/lesson/pattern cards.
- [ ] Build example list.

### UI Requirements

- Level page phải scan nhanh.
- Lesson page hiển thị progress và số pattern.
- Pattern detail tập trung vào nội dung học.
- Pattern detail có tabs/sections:
  - Overview
  - Examples
  - Exercises

### Verification

- [ ] User đi được từ Grammar -> Level -> Lesson -> Pattern.
- [ ] Pattern detail hiển thị đầy đủ structure/meaning/usage/notes/examples.

---

## 12. Frontend Phase 3 — Add To Study + Grammar Review

### Tasks

- [ ] Build `AddGrammarToStudyButton`.
- [ ] Show state:
  - Add to Study
  - In Study
  - Loading
- [ ] Build `GrammarReviewPage`.
- [ ] Build `GrammarReviewCard`.
- [ ] Connect SRS submit answer.

### Verification

- [ ] Add pattern thành công.
- [ ] Pattern xuất hiện trong Grammar Review.
- [ ] Submit answer cập nhật card.
- [ ] Hoàn thành review session không lỗi.

---

## 13. Frontend Phase 4 — Exercises

### Tasks

- [ ] Build `GrammarExerciseTabs`.
- [ ] Build `ViToJaExerciseCard`.
- [ ] Build `JaToViExerciseCard`.
- [ ] Build `ArrangeExerciseCard`.
- [ ] Add check answer button.
- [ ] Add reveal answer button.
- [ ] Show feedback.

### UI Similar To Target

Tabs:

```text
Việt -> Nhật (count)
Nhật -> Việt (count)
Sắp xếp (count)
```

Vietnamese -> Japanese:

```text
Prompt
Textarea
Kiểm tra
AI đánh giá
Xem đáp án
```

Arrange:

```text
Sentence template with blanks
Option buttons
Reset
Kiểm tra
```

### Verification

- [ ] User làm được bài Việt -> Nhật.
- [ ] User làm được bài Nhật -> Việt.
- [ ] User làm được bài sắp xếp.
- [ ] Check/reveal answer hoạt động.

---

## 14. Frontend Phase 5 — AI Evaluation UI

### Tasks

- [ ] Add API key input/settings.
- [ ] Disable AI button nếu chưa có key.
- [ ] Call AI evaluation endpoint.
- [ ] Show score/feedback/corrected answer.
- [ ] Do not display raw API key after entry.

### Verification

- [ ] Không có key thì UI hướng dẫn nhập key.
- [ ] Có key thì AI evaluate chạy.
- [ ] Feedback hiển thị rõ ràng.

---

## 15. Deferred Premium Phase

Premium không làm bây giờ.

Sau khi hoàn thiện user system và các module chính, thêm:

- `UserSubscription`
- `UserEntitlement`
- `IPremiumAccessService`
- access check thật trong Grammar content/study/review
- locked UI
- payment flow

Hiện tại chỉ giữ design không chặn đường:

- `accessTier`
- `packageCode`
- `isLocked = false` trong DTO phase đầu

---

## 16. Final Acceptance Criteria

Backend:

- [ ] `dotnet build` pass.
- [ ] Migration chạy được.
- [ ] Swagger test content APIs pass.
- [ ] Swagger test grammar SRS APIs pass.
- [ ] Swagger test exercise APIs pass.
- [ ] Grammar progress độc lập với Vocabulary.

Frontend:

- [ ] `npm run build` pass.
- [ ] Grammar route hoạt động.
- [ ] User browse được N5 -> lesson -> pattern.
- [ ] User add pattern vào học.
- [ ] User review grammar due.
- [ ] User làm được 3 dạng bài tập.
- [ ] AI evaluation optional hoạt động khi có API key.

---

## 17. Recommended Build Order

```text
1. Backend entities + migration
2. Backend content API
3. Backend SRS API
4. Backend exercise API
5. Seed data
6. Frontend routes/base pages
7. Frontend content pages
8. Frontend add-to-study/review
9. Frontend exercises
10. AI evaluation
11. Premium later
```
