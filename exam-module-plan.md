# Plan: Module Luyện Thi

## Phase 1: Backend Domain Model

Tạo module backend `ExamPractice` trong Core.

Entities:

- `ExamPassage`
- `ExamQuestion`
- `ExamQuestionOption`
- `ExamAttempt`
- `ExamAttemptAnswer`

Constants:

- `ExamQuestionTypes`
- `ExamQuestionTopics`
- `ExamAttemptStatuses`
- `ExamDefaults`

DTOs:

- `ExamTopicDto`
- `ExamQuestionDto`
- `ExamQuestionDetailDto`
- `ExamAnswerResultDto`
- `StartExamAttemptDto`
- `ExamAttemptDto`
- `ExamAttemptReviewDto`

Service interface:

- `IExamPracticeService`

## Phase 2: Database + EF Config

Trong Infrastructure:

- Thêm `DbSet`.
- Thêm EF configurations:
  - `ExamPassageConfiguration`
  - `ExamQuestionConfiguration`
  - `ExamQuestionOptionConfiguration`
  - `ExamAttemptConfiguration`
  - `ExamAttemptAnswerConfiguration`
- Tạo migration.
- Bảo đảm table có index theo:
  - `topic`
  - `level`
  - `isActive`
  - `userId`
  - `attemptId`

## Phase 3: Seed Data Mẫu

Tạo seed mẫu trước, chưa nhập đủ `200-300` câu.

Seed ban đầu:

- 5 câu kanji.
- 5 câu ngữ pháp.
- 5 câu từ vựng.
- 5 câu phân biệt từ khác loại.
- 2 passage đọc hiểu, mỗi passage 2-3 câu.

Sau khi logic ổn mới mở rộng JSON/seed lên `200-300` câu.

## Phase 4: Backend Service Logic

Implement `ExamPracticeService`.

Chế độ học:

- Lấy topics.
- Lấy câu hỏi theo topic/level.
- Lấy chi tiết câu hỏi.
- Chấm câu hỏi đơn.
- Trả về đáp án đúng và giải thích.

Chế độ luyện thi:

- Start attempt:
  - random 30 câu.
  - duration 30 phút.
  - tạo `ExamAttempt`.
  - tạo danh sách `ExamAttemptAnswer` ban đầu.
- Get attempt:
  - trả về attempt hiện tại và câu hỏi.
- Save answer:
  - lưu đáp án người dùng chọn.
  - tính đúng/sai cho câu đó.
- Submit attempt:
  - khóa attempt.
  - tính tổng điểm.
  - lưu `submittedAt`.
- Review attempt:
  - trả về câu hỏi, đáp án đã chọn, đáp án đúng, giải thích.

## Phase 5: API Controller

Tạo `ExamPracticeController`.

Routes:

- `GET /api/exam/topics`
- `GET /api/exam/questions`
- `GET /api/exam/questions/{questionId}`
- `POST /api/exam/questions/{questionId}/answer`
- `POST /api/exam/attempts/start`
- `GET /api/exam/attempts/{attemptId}`
- `POST /api/exam/attempts/{attemptId}/answers`
- `POST /api/exam/attempts/{attemptId}/submit`
- `GET /api/exam/attempts/{attemptId}/review`

Controller dùng `CurrentUserId`, không dùng hard-code user.

## Phase 6: Backend Testing

Test bằng API trước UI:

- `GET /api/exam/topics`
- `GET /api/exam/questions?topic=kanji`
- `GET /api/exam/questions/{id}`
- `POST /api/exam/questions/{id}/answer`
- `POST /api/exam/attempts/start`
- `GET /api/exam/attempts/{id}`
- `POST /api/exam/attempts/{id}/answers`
- `POST /api/exam/attempts/{id}/submit`
- `GET /api/exam/attempts/{id}/review`

Build:

- `dotnet build JPLearn.slnx`

## Phase 7: Frontend Feature Skeleton

Tạo folder:

```text
client/src/features/exam/
  api/
    examApi.ts
  components/
  hooks/
  pages/
  types/
    exam.types.ts
  exam.routes.tsx
```

Routes:

- `/exam`
- `/exam/study`
- `/exam/study/:topic`
- `/exam/test`
- `/exam/test/:attemptId`
- `/exam/test/:attemptId/result`

## Phase 8: UI Chế Độ Học

Màn hình:

- Exam dashboard.
- Topic selector.
- Study question page.

Tính năng:

- Chọn topic.
- Hiển thị câu hỏi.
- Chọn đáp án.
- Hiển thị đúng/sai.
- Hiển thị giải thích.
- Next question.

## Phase 9: UI Chế Độ Luyện Thi

Màn hình:

- Start exam page.
- Exam attempt page.
- Exam result/review page.

Tính năng:

- Start random 30 câu.
- Timer 30 phút.
- Câu hỏi + options.
- Navigation câu hỏi.
- Save answer.
- Submit.
- Result + review.

## Phase 10: Mở Rộng Data

Sau khi backend và UI ổn:

- Chuẩn hóa format JSON import câu hỏi.
- Nhập đủ `200-300` câu.
- Phân bổ theo topic.
- Kiểm tra không trùng câu hỏi/options.
- Kiểm tra câu nào cũng có đáp án đúng và explanation.
