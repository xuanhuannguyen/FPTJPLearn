# JPLearn Grammar Module — Requirements

## 1. Context

Module ngữ pháp giúp user học ngữ pháp tiếng Nhật theo cấp độ JLPT:

```text
N5 -> N4 -> N3 -> N2 -> N1
```

Mỗi level có nhiều bài. Mỗi bài có nhiều mẫu ngữ pháp. Mỗi mẫu ngữ pháp có một page riêng gồm:

- cấu trúc
- giải nghĩa
- phạm vi sử dụng
- lưu ý
- ví dụ
- bài tập
- nút thêm vào học

Module Grammar có **SRS riêng**, không gộp chung với Vocabulary, Kanji, hoặc Static Vocabulary.

---

## 2. Product Decisions

### 2.1 Grammar là content library + SRS riêng

Grammar chia làm 2 phần:

```text
Grammar Content
├── Level
├── Lesson
├── Pattern
├── Example
└── Exercise

Grammar Study
├── Add pattern to study
├── UserGrammarProgress
├── Grammar due review
└── Grammar exercise attempts
```

Không có Study/SRS gộp chung.

```text
Vocabulary        -> UserWordProgress
Grammar           -> UserGrammarProgress
Kanji             -> UserKanjiProgress
Static Vocabulary -> UserStaticVocabularyProgress
```

### 2.2 Premium theo lesson/pattern

Grammar content cần thiết kế sẵn để sau này bán premium.

Ví dụ:

```text
N5: lesson 1-2 free, lesson 3+ premium
N4: lesson 1-3 free, lesson 4+ premium
```

Access nên đặt ở lesson trước. Pattern mặc định kế thừa access từ lesson.

### 2.3 Hết premium không xóa progress

Nếu user add một pattern premium vào học rồi sau đó hết premium:

- không xóa `UserGrammarProgress`
- không reset SRS
- không hiện pattern đó trong Grammar Review due
- pattern detail vẫn có thể hiện `In Study` nhưng `Locked`
- khi user gia hạn premium, pattern tự hiện lại trong review nếu đến hạn

Nguyên tắc:

```text
Premium expired -> lock access, keep progress
Premium renewed -> unlock access, reuse existing progress
```

### 2.4 AI đánh giá dùng API key của user

AI evaluation là optional.

- User có thể tự nhập API key của họ.
- Không có API key thì vẫn dùng được bài tập bằng `Kiểm tra` và `Xem đáp án`.
- API key không được log.
- Nếu lưu key lâu dài thì phải mã hóa.
- Phase đầu có thể chỉ giữ API key trong session/local client setting, chưa cần lưu DB.

---

## 3. Goals

- User xem Grammar theo `N5-N1`.
- User xem lesson trong từng level.
- User xem pattern trong từng lesson.
- User mở được pattern detail page.
- User làm bài tập theo từng pattern:
  - Việt -> Nhật
  - Nhật -> Việt
  - Sắp xếp câu
- User có thể dùng AI đánh giá bài làm bằng API key cá nhân.
- User có thể add pattern vào Grammar Study.
- Pattern đã add sẽ đi vào Grammar SRS riêng.
- Premium access có thể bật sau này mà không phải đổi schema lớn.
- Progress đã học được giữ lại dù premium hết hạn.

---

## 4. Non-goals

Phase đầu không làm:

- Không làm payment thật.
- Không làm subscription billing thật.
- Không gộp Grammar SRS với module khác.
- Không làm user tự tạo grammar lesson/pattern.
- Không làm admin UI nếu chưa cần.
- Không bắt buộc AI để học.

---

## 5. Main User Flows

### Flow 1: Browse Grammar

```text
/grammar
-> choose N5
-> choose Lesson 1
-> open Pattern Detail
```

Chỉ xem content không tự tạo progress.

### Flow 2: Add Pattern To Study

```text
Pattern Detail
-> Add to Grammar Study
-> Create UserGrammarProgress
-> Pattern appears in Grammar Review if due and accessible
```

Nếu pattern premium và user chưa có quyền:

```text
Add button disabled / Premium required
```

### Flow 3: Premium Expired

```text
User has premium
-> Add premium pattern to study
-> Premium expires
-> Pattern hidden from Grammar Review
-> Progress remains in DB
```

Khi gia hạn:

```text
Premium active again
-> HasAccess = true
-> Pattern appears again if nextReviewAt <= now
```

### Flow 4: Pattern Exercises

```text
Pattern Detail
-> Exercises tab
-> Choose Viet -> Japanese / Japanese -> Viet / Arrange
-> Submit answer
-> Check with sample answer
-> Optional AI evaluation
```

---

## 6. Information Architecture

```text
/grammar
  Grammar home, list all JLPT levels

/grammar/:level
  Lessons in selected level

/grammar/:level/lessons/:lessonId
  Patterns in selected lesson

/grammar/patterns/:patternId
  Pattern detail + examples + exercises + add to study

/grammar/review
  Grammar SRS review only
```

---

## 7. Access And Premium Requirements

### FR-ACCESS-1: Content access fields

`GrammarLesson` must include:

```text
accessTier   // free | premium
packageCode  // grammar_n5, grammar_n4, grammar_all...
```

`GrammarPattern` can optionally override lesson access:

```text
accessTierOverride    // null | free | premium
packageCodeOverride   // null | grammar_n5...
```

Access resolution:

```text
pattern access = pattern override if set
else lesson access
```

### FR-ACCESS-2: Free lesson rule

The system must support configurable free lessons per level.

Example seed rule:

```text
N5 lesson 1-2 = free
N5 lesson 3+  = premium

N4 lesson 1-3 = free
N4 lesson 4+  = premium
```

This should be stored as data, not hard-coded in frontend.

### FR-ACCESS-3: Locked content response

APIs returning grammar content must include access state:

```json
{
  "accessTier": "premium",
  "packageCode": "grammar_n5",
  "isLocked": true,
  "lockReason": "premium_required"
}
```

### FR-ACCESS-4: Add to study access check

When user adds a pattern to study:

```text
if pattern is free -> allow
if pattern is premium and user has access -> allow
if pattern is premium and user has no access -> reject
```

### FR-ACCESS-5: Review access check

Grammar Review must only return due cards that are currently accessible:

```text
UserGrammarProgress.nextReviewAt <= now
and HasAccess(grammarPattern) = true
```

Premium expired items remain in progress but are excluded from review.

---

## 8. Premium Future Data Model

Payment can be implemented later. For now, structure should support it.

### UserSubscription

```text
id
userId
planCode          // premium_monthly | premium_yearly
status            // active | expired | cancelled
startedAt
expiresAt
createdAt
updatedAt
```

### UserEntitlement

Use this if later selling specific packages.

```text
id
userId
packageCode       // grammar_n5, grammar_n4, grammar_all
source            // subscription | purchase | admin_grant
startsAt
expiresAt
createdAt
updatedAt
```

Access check:

```text
free content -> accessible
premium content -> accessible if active subscription OR active entitlement for packageCode
```

---

## 9. Grammar Content Requirements

### FR1: Grammar Levels

- Must support `N5`, `N4`, `N3`, `N2`, `N1`.
- Grammar home shows all levels.
- Each level card shows:
  - level
  - lesson count
  - pattern count
  - free count
  - premium count
  - in-study count
  - mastered count
  - due count

### FR2: Grammar Lessons

Each lesson has:

```text
level
lessonNumber
title
description
accessTier
packageCode
orderIndex
```

Sorting:

```text
orderIndex ASC, lessonNumber ASC
```

### FR3: Grammar Patterns

Each pattern has:

```text
lessonId
level
pattern
title
meaning
structure
usageScope
formation
notes
tagsJson
accessTierOverride
packageCodeOverride
orderIndex
```

Pattern detail must show:

- pattern
- title
- structure
- meaning
- usage scope
- formation
- notes
- examples
- exercises
- add-to-study state
- access state

### FR4: Grammar Examples

Each example has:

```text
patternId
japanese
reading
meaning
note
orderIndex
```

---

## 10. Exercise Requirements

Each grammar pattern can have many exercises.

Exercise types:

```text
vi_to_ja
ja_to_vi
arrange
```

### FR-EX1: Vietnamese -> Japanese

User sees Vietnamese sentence and writes Japanese answer.

UI behavior:

- show Vietnamese prompt
- textarea/input for Japanese answer
- button `Kiểm tra`
- button `AI đánh giá` if API key configured
- button `Xem đáp án`
- optional hint

Example:

```text
Prompt: Tôi là Michael.
Expected: 私はマイケルです。
```

### FR-EX2: Japanese -> Vietnamese

User sees Japanese sentence and writes Vietnamese translation.

UI behavior:

- show Japanese prompt
- show reading optionally
- textarea/input for Vietnamese answer
- check against sample meaning
- optional AI evaluation

### FR-EX3: Arrange Sentence

User arranges given parts to complete a sentence.

Supports the style:

```text
あのひと [____] [____] [★] [____] です。

Options:
1 の
2 たなかさん
3 ともだち
4 は
```

Required data:

- sentence template
- blank count
- options
- correct order
- star position
- correct star answer

### FR-EX4: Exercise count per pattern

Recommended default per pattern:

```text
10 vi_to_ja
10 ja_to_vi
10 arrange
```

But schema must allow any count.

### FR-EX5: Exercise attempts

System should store attempts for analytics.

```text
GrammarExerciseAttempts
├── id
├── userId
├── exerciseId
├── answerText
├── selectedOptionOrderJson
├── isCorrect
├── score
├── feedback
├── checkedBy       // system | ai
├── createdAt
```

---

## 11. AI Evaluation Requirements

### FR-AI1: User-owned API key

User can provide their own AI API key.

Initial options:

```text
Do not store key
Store temporarily in browser/session
```

Later option:

```text
Store encrypted API key server-side
```

If stored server-side:

- encrypt at rest
- never log raw key
- mask in UI
- allow delete key

### FR-AI2: AI evaluation button

`AI đánh giá` button is enabled only if:

```text
user has configured API key
and answer is not empty
```

If no key:

```text
show "Add API key to use AI evaluation"
```

### FR-AI3: AI evaluation output

AI evaluation should return:

```json
{
  "score": 8,
  "isAcceptable": true,
  "correctedAnswer": "私はマイケルです。",
  "feedback": "Câu đúng. Có thể dùng マイケル for Michael.",
  "grammarNotes": ["Mẫu N は N です được dùng đúng."]
}
```

### FR-AI4: AI should evaluate only the current pattern

Prompt must include:

- grammar pattern
- structure
- expected answer
- user answer
- language direction
- instruction to focus on this pattern

### FR-AI5: AI is optional

The exercise must work without AI:

- `Kiểm tra` compares with sample answer.
- `Xem đáp án` reveals answer.
- AI only gives richer feedback.

---

## 12. Grammar SRS Requirements

### FR-SRS1: Add to Grammar Study

When user adds a pattern:

```text
Create UserGrammarProgress if not exists
Do not duplicate
```

Initial values:

```text
level = 0
status = new
repetitions = 0
easeFactor = 2.5
intervalDays = 0
nextReviewAt = now
lapseCount = 0
learningStepIndex = 0
```

### FR-SRS2: Grammar due

Due grammar cards:

```text
nextReviewAt <= now
and current user has access
```

Overdue cards do not auto-demote.

### FR-SRS3: Grammar levels

Grammar uses 4 levels:

```text
Level 0 = new
Level 1 = learning
Level 2 = review
Level 3 = mastered
```

Statuses:

```text
new
learning
review
mastered
relearning
```

### FR-SRS4: Rating

Grammar Review supports:

```text
Again -> quality 1
Hard  -> quality 3
Good  -> quality 5
```

### FR-SRS5: Remove from study

Removing from Grammar Study should not delete grammar content.

Recommended:

```text
UserGrammarProgress.isActive = false
```

Do not hard delete unless explicitly requested.

---

## 13. Data Model

### GrammarLesson

```text
id
level
lessonNumber
title
description
accessTier
packageCode
orderIndex
createdAt
updatedAt
```

### GrammarPattern

```text
id
lessonId
level
pattern
title
meaning
structure
usageScope
formation
notes
tagsJson
accessTierOverride
packageCodeOverride
orderIndex
createdAt
updatedAt
```

### GrammarExample

```text
id
patternId
japanese
reading
meaning
note
orderIndex
createdAt
updatedAt
```

### GrammarExercise

```text
id
patternId
exerciseType        // vi_to_ja | ja_to_vi | arrange
prompt
promptReading
expectedAnswer
acceptableAnswersJson
hint
explanation
orderIndex
createdAt
updatedAt
```

For arrange exercises:

```text
templateText
optionsJson
correctOrderJson
starPosition
starAnswer
```

Can be nullable for non-arrange exercises.

### UserGrammarProgress

```text
id
userId
grammarPatternId
level
status
repetitions
easeFactor
intervalDays
nextReviewAt
lastReviewedAt
lapseCount
learningStepIndex
isActive
addedAt
createdAt
updatedAt
```

Unique:

```text
userId + grammarPatternId
```

### GrammarExerciseAttempt

```text
id
userId
grammarExerciseId
answerText
selectedOptionOrderJson
isCorrect
score
feedback
checkedBy
createdAt
```

---

## 14. API Requirements

### Grammar Content

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/grammar/levels` | Level overview with access/progress |
| GET | `/api/grammar/{level}/lessons` | Lessons by level |
| GET | `/api/grammar/lessons/{lessonId}` | Lesson detail with patterns |
| GET | `/api/grammar/patterns/{patternId}` | Pattern detail |
| GET | `/api/grammar/search?query=` | Search grammar |

### Grammar Study/SRS

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/grammar/patterns/{patternId}/study` | Add pattern to study |
| DELETE | `/api/grammar/patterns/{patternId}/study` | Remove/deactivate study |
| GET | `/api/grammar/review/due` | Due grammar cards |
| POST | `/api/grammar/review/answer` | Submit SRS answer |
| GET | `/api/grammar/progress` | Grammar progress summary |

### Grammar Exercises

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/grammar/patterns/{patternId}/exercises` | Get exercises for pattern |
| POST | `/api/grammar/exercises/{exerciseId}/check` | Check answer without AI |
| POST | `/api/grammar/exercises/{exerciseId}/ai-evaluate` | AI evaluate answer |
| GET | `/api/grammar/exercises/{exerciseId}/answer` | Reveal sample answer |

### Future Premium

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/me/subscription` | Current subscription state |
| GET | `/api/me/entitlements` | Current package access |

Payment endpoints can be added later.

---

## 15. DTO Requirements

### GrammarPatternDetailDto

```json
{
  "id": "uuid",
  "lessonId": "uuid",
  "level": "N5",
  "pattern": "〜です",
  "title": "Câu khẳng định lịch sự",
  "meaning": "là...",
  "structure": "Noun + です",
  "usageScope": "Dùng để nói A là B một cách lịch sự.",
  "formation": "学生 + です = 学生です",
  "notes": "Không dùng trực tiếp sau động từ.",
  "accessTier": "free",
  "packageCode": "grammar_n5",
  "isLocked": false,
  "isInStudy": true,
  "progress": {
    "level": 1,
    "status": "learning",
    "nextReviewAt": "2026-05-07T10:00:00Z"
  },
  "examples": [],
  "exercises": []
}
```

### CheckExerciseAnswerDto

```json
{
  "answerText": "私はマイケルです。",
  "selectedOptionOrder": ["4", "2", "3", "1"]
}
```

### AiEvaluateExerciseDto

```json
{
  "answerText": "私はマイケルです。",
  "apiKey": "user-api-key",
  "provider": "gemini"
}
```

Phase đầu có thể send key per request. Later should support saved encrypted key.

---

## 16. Frontend Structure

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
├── components/
│   ├── GrammarLevelCard.tsx
│   ├── GrammarLessonCard.tsx
│   ├── GrammarPatternCard.tsx
│   ├── GrammarExampleList.tsx
│   ├── GrammarExerciseTabs.tsx
│   ├── ViToJaExerciseCard.tsx
│   ├── JaToViExerciseCard.tsx
│   ├── ArrangeExerciseCard.tsx
│   ├── AiEvaluationPanel.tsx
│   ├── AddGrammarToStudyButton.tsx
│   ├── GrammarProgressBadge.tsx
│   └── GrammarReviewCard.tsx
```

---

## 17. Backend Structure

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
├── DTOs/

server/JPLearn.Infrastructure/Services/
├── GrammarService.cs
├── GrammarReviewService.cs
└── GrammarExerciseService.cs

server/JPLearn.Api/Controllers/
└── GrammarController.cs
```

Premium future:

```text
server/JPLearn.Core/Premium/
├── Entities/
│   ├── UserSubscription.cs
│   └── UserEntitlement.cs
└── IPremiumAccessService.cs
```

Phase đầu có thể mock `IPremiumAccessService`:

```text
free -> true
premium -> false unless dev flag/user has fake entitlement
```

---

## 18. Validation Rules

- `level` must be one of `N5`, `N4`, `N3`, `N2`, `N1`.
- `lessonNumber` must be positive.
- `accessTier` must be `free` or `premium`.
- `pattern`, `title`, `meaning`, `structure` are required.
- Pattern must belong to an existing lesson.
- Exercise must belong to an existing pattern.
- Exercise type must be valid.
- User cannot add the same pattern twice.
- Premium pattern cannot be added without access.
- Locked premium progress must not be returned in review due.
- AI evaluation requires non-empty answer and API key.

---

## 19. Seed JSON Format

```json
{
  "level": "N5",
  "lessonNumber": 1,
  "title": "Câu cơ bản",
  "description": "Các mẫu câu nền tảng trong N5.",
  "accessTier": "free",
  "packageCode": "grammar_n5",
  "patterns": [
    {
      "pattern": "〜です",
      "title": "Câu khẳng định lịch sự",
      "meaning": "là...",
      "structure": "Noun + です",
      "usageScope": "Dùng để nói A là B một cách lịch sự.",
      "formation": "学生 + です = 学生です",
      "notes": "Không dùng trực tiếp sau động từ.",
      "tags": ["noun", "basic", "polite"],
      "examples": [
        {
          "japanese": "私は学生です。",
          "reading": "わたしは がくせいです。",
          "meaning": "Tôi là học sinh."
        }
      ],
      "exercises": [
        {
          "exerciseType": "vi_to_ja",
          "prompt": "Tôi là Michael.",
          "expectedAnswer": "私はマイケルです。",
          "hint": "Dùng mẫu N は N です。"
        },
        {
          "exerciseType": "ja_to_vi",
          "prompt": "私は学生です。",
          "expectedAnswer": "Tôi là học sinh."
        },
        {
          "exerciseType": "arrange",
          "prompt": "Chọn thứ tự đúng để hoàn thành câu.",
          "templateText": "あのひと ____ ____ ★ ____ です。",
          "options": ["の", "たなかさん", "ともだち", "は"],
          "correctOrder": ["は", "たなかさん", "の", "ともだち"],
          "starPosition": 3,
          "starAnswer": "の"
        }
      ]
    }
  ]
}
```

---

## 20. Acceptance Criteria

- [ ] User can browse grammar levels N5-N1.
- [ ] User can open lessons by level.
- [ ] User can open pattern detail.
- [ ] Free lessons are accessible without premium.
- [ ] Premium lessons return locked state if user has no access.
- [ ] User can add accessible pattern to Grammar Study.
- [ ] User cannot add locked premium pattern.
- [ ] Existing premium progress is preserved after premium expires.
- [ ] Expired premium progress does not show in Grammar Review.
- [ ] Renewed premium access makes due progress appear again.
- [ ] Pattern page shows exercises.
- [ ] User can do vi_to_ja exercise.
- [ ] User can do ja_to_vi exercise.
- [ ] User can do arrange exercise.
- [ ] User can reveal sample answer.
- [ ] User can use AI evaluation with their own API key.
- [ ] Grammar SRS does not touch Vocabulary SRS.
- [ ] `npm run build` passes.
- [ ] `dotnet build` passes.

---

## 21. Implementation Plan

### Phase 1: Grammar Content + Access Metadata

- [ ] Add Grammar entities.
- [ ] Add `accessTier` and `packageCode`.
- [ ] Add DbSets and EF configurations.
- [ ] Add migration.
- [ ] Seed N5 sample data.
- [ ] Implement content endpoints.

### Phase 2: Grammar UI

- [ ] Create `features/grammar`.
- [ ] Add routes.
- [ ] Build grammar home, level, lesson, pattern pages.
- [ ] Show locked/free states.
- [ ] Add sidebar link.

### Phase 3: Grammar Study/SRS

- [ ] Add `UserGrammarProgress`.
- [ ] Add add-to-study endpoint.
- [ ] Add due endpoint with access check.
- [ ] Add submit SRS answer.
- [ ] Build Grammar Review page.

### Phase 4: Exercises

- [ ] Add `GrammarExercise`.
- [ ] Add `GrammarExerciseAttempt`.
- [ ] Build exercise tabs.
- [ ] Build vi_to_ja card.
- [ ] Build ja_to_vi card.
- [ ] Build arrange card.
- [ ] Add check/reveal answer endpoints.

### Phase 5: AI Evaluation

- [ ] Add AI evaluate endpoint.
- [ ] Support user-provided API key per request.
- [ ] Add AI evaluation panel.
- [ ] Store attempts with `checkedBy = ai`.

### Phase 6: Future Premium

- [ ] Add subscription/entitlement tables.
- [ ] Add `IPremiumAccessService`.
- [ ] Replace mock access with real subscription check.

---

## 22. Open Questions

- Phase đầu API key nên chỉ nhập mỗi lần hay lưu local setting?
- AI provider đầu tiên là Gemini hay OpenAI?
- Arrange exercise có cần drag-and-drop hay click option là đủ?
- Premium ban đầu là toàn bộ app hay theo package như `grammar_n5`?
- Pattern premium khi locked có cho xem preview một phần không?
