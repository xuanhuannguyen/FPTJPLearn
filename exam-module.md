# Module Luyện Thi

## Mục tiêu

Xây dựng module `Luyện thi` cho phép người học luyện câu hỏi tiếng Nhật theo 2 chế độ:

- `Chế độ học`: học từng câu hỏi theo chủ đề, có đáp án và giải thích.
- `Chế độ luyện thi`: làm bài thi ngẫu nhiên `30 câu` trong `30 phút`, có tính điểm và xem lại đáp án.

Module cần hỗ trợ ngân hàng khoảng `200-300` câu hỏi và có thể mở rộng thêm.

## Loại câu hỏi

### Standalone multiple choice

Câu hỏi trắc nghiệm độc lập.

Ví dụ:

- Chọn kanji đúng.
- Chọn nghĩa đúng.
- Chọn ngữ pháp phù hợp.
- Chọn từ khác loại.

### Reading passage

Một đoạn văn đi kèm `2-3` câu hỏi trắc nghiệm.

Yêu cầu:

- Hiển thị đoạn văn trước.
- Bên dưới có các câu hỏi liên quan đến đoạn văn.
- Mỗi câu hỏi có đáp án riêng.
- Mỗi câu hỏi có giải thích riêng.

## Chủ đề câu hỏi

Mỗi câu hỏi cần có `topic` để học/lọc theo nhóm:

- `kanji`
- `grammar`
- `vocabulary`
- `odd_one_out`
- `reading`

Có thể mở rộng thêm chủ đề sau này.

## Chế độ học

Người dùng có thể:

- Chọn học theo chủ đề (topic) của từng Course (JPD113, JPD123).
- Học câu hỏi **tuần tự** (không xáo trộn) để có thể theo dõi tiến độ.
- Hệ thống tự động ghi nhớ vị trí câu hỏi cuối cùng đã học (thông qua bảng `ExamPracticeProgress`). Lần sau học tiếp sẽ tự động bắt đầu từ câu hỏi tiếp theo.
- Chọn đáp án.
- Sau khi trả lời, xem đáp án đúng, đáp án đã chọn, kết quả đúng/sai và giải thích.

Với câu đọc hiểu:

- Hiển thị đoạn văn.
- Hiển thị các câu hỏi thuộc đoạn văn.
- Trả lời từng câu.
- Xem giải thích từng câu.

## Chế độ luyện thi

Khi bắt đầu:

- Hệ thống dựa vào **Khuôn mẫu đề thi (Exam Blueprint)** để tạo đề.
- Các topic (kanji, grammar, vocabulary, reading...) sẽ được random với **tỷ lệ cố định** (được định nghĩa trong `ExamBlueprintRule`, ví dụ: mỗi topic lấy 5 câu).
- Thời gian làm bài theo `TimeLimit` được định nghĩa trong Blueprint.
- Có cả câu thường và câu đọc hiểu nếu dữ liệu có.

Trong khi làm:

- Hiển thị timer.
- Cho phép chọn đáp án.
- Cho phép chuyển câu.
- Cho phép nộp bài sớm.
- Hết giờ thì tự nộp.

Sau khi nộp:

- Hiển thị số câu đúng `/30`.
- Hiển thị phần trăm đúng.
- Lưu lịch sử lần làm bài.
- Cho phép xem lại câu hỏi, đáp án người dùng chọn, đáp án đúng, giải thích và đoạn văn nếu có.

## Dữ liệu cần lưu

### Câu hỏi

- `id`
- `questionType`: `standalone` hoặc `passage`
- `topic`
- `level`
- `questionText`
- `explanation`
- `passageId`
- `orderIndex`
- `isActive`

### Đáp án

- `id`
- `questionId`
- `label`
- `text`
- `isCorrect`
- `orderIndex`

### Đoạn văn

- `id`
- `title`
- `content`
- `level`
- `topic`
- `orderIndex`
- `isActive`

### Lần luyện thi

- `attemptId`
- `userId`
- `blueprintId` (Thêm FK map với Blueprint)
- `startedAt`
- `submittedAt`
- `expiresAt`
- `durationMinutes`
- `totalQuestions`
- `correctCount`
- `scorePercent`
- `status`

### Câu trả lời

- `attemptAnswerId`
- `attemptId`
- `questionId`
- `selectedOptionId`
- `isCorrect`
- `answeredAt`

### Khuôn mẫu đề thi (Blueprint)
- `id`
- `courseCode`
- `title`
- `timeLimitMinutes`
- `isActive`

### Quy tắc tạo đề (Blueprint Rule)
- `id`
- `blueprintId`
- `topic`
- `questionCount`

### Tiến độ luyện tập (Practice Progress)
- `id`
- `userId`
- `courseCode`
- `topic`
- `totalCompleted`

## API yêu cầu

### Chế độ học

- `GET /api/exam/topics`
- `GET /api/exam/questions?topic={topic}&level={level}`
- `GET /api/exam/questions/{questionId}`
- `POST /api/exam/questions/{questionId}/answer`

### Chế độ luyện thi

- `POST /api/exam/attempts/start`
- `GET /api/exam/attempts/{attemptId}`
- `POST /api/exam/attempts/{attemptId}/answers`
- `POST /api/exam/attempts/{attemptId}/submit`
- `GET /api/exam/attempts/{attemptId}/review`

## Frontend routes

- `/exam`
- `/exam/study`
- `/exam/study/:topic`
- `/exam/test`
- `/exam/test/:attemptId`
- `/exam/test/:attemptId/result`

## Acceptance Criteria

Module đạt yêu cầu khi:

- Có thể seed/lưu câu hỏi thường và câu đọc hiểu.
- Có thể học câu hỏi theo chủ đề.
- Có thể trả lời câu hỏi và xem giải thích.
- Có thể bắt đầu bài luyện thi `30 câu / 30 phút`.
- Có thể lưu câu trả lời.
- Có thể nộp bài và tính điểm.
- Có thể xem lại đáp án sau khi nộp.
- API backend build pass và test được bằng dữ liệu mẫu.
- Frontend không hard-code câu hỏi.
