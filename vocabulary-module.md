# Vocabulary Module — Plan & Requirements

## Goal

Xây dựng module **Import JSON → Quản lý list từ vựng → 3 chế độ ôn tập** (SRS Flashcard / Multiple Choice / Typing Test).

**Nguyên tắc cốt lõi của module này:**
- Một list như `N4_Bài 1` là **một chủ đề cố định**
- Các từ trong list đó **luôn nằm trong chính list đó**
- Khi ôn tập, hệ thống **không tạo list mới** từ list cũ
- Tiến độ học được lưu ở **từng từ** bằng `level / status / next_review_at`
- User học trong page của chính list đó và nhìn thấy các từ tăng/giảm level theo thời gian

---

## Requirements

### R1: JSON Import

**User story:** Tôi muốn paste JSON vào form → đặt tên chủ đề (VD: "N4_Bài 1") → hệ thống tạo list từ vựng với ~10-50 từ.

**Cấu trúc mỗi từ:**

```json
{
  "name": "N4_Bài 1",
  "words": [
    {
      "word": "行きます",
      "reading": "いきます",
      "type": "Động từ nhóm 1",
      "meaning": "Đi",
      "example": "学校に行きます。",
      "exampleMeaning": "Tôi đi đến trường."
    },
    {
      "word": "来ます",
      "reading": "きます",
      "type": "Động từ nhóm 3",
      "meaning": "Đến",
      "example": "友達が来ます。",
      "exampleMeaning": "Bạn bè đến."
    },
    {
      "word": "帰ります",
      "reading": "かえります",
      "type": "Động từ nhóm 1",
      "meaning": "Về (nhà)",
      "example": "うちに帰ります。",
      "exampleMeaning": "Tôi về nhà."
    }
  ]
}
```

**Validation rules:**
- `word` (bắt buộc) — Kanji form: 行きます
- `reading` (bắt buộc) — Hiragana: いきます
- `type` (bắt buộc) — Từ loại: Động từ nhóm 1, 名詞, い形容詞...
- `meaning` (bắt buộc) — Nghĩa tiếng Việt
- `example` (optional) — Câu ví dụ tiếng Nhật
- `exampleMeaning` (optional) — Dịch câu ví dụ
- Max **200 từ/list**
- JSON phải valid → hiện lỗi rõ ràng (dòng nào, field nào thiếu)

**UI flow:**
```
[Bước 1] Nhập tên: "N4_Bài 1"
[Bước 2] Paste JSON vào textarea
[Bước 3] Click "Preview" → Bảng preview hiện ra (word | reading | type | meaning)
[Bước 4] Kiểm tra → Click "Import" → Tạo list → Redirect về danh sách
```

---

### R2: Quản lý Vocabulary Lists

**User story:** Tôi muốn xem tất cả lists của mình, click vào xem chi tiết, sửa/xóa.

**Trang danh sách (VocabularyListsPage):**
- Grid cards hiển thị: Tên list, số từ, % đã thuộc, ngày tạo
- Nút "Import mới" 
- Click card → vào trang chi tiết

**Trang chi tiết (VocabularyDetailPage):**
- Bảng hiển thị tất cả từ trong list:

| # | Từ vựng | Đọc | Từ loại | Nghĩa | Ví dụ | Trạng thái |
|---|---------|-----|---------|--------|-------|------------|
| 1 | 行きます | いきます | Động từ nhóm 1 | Đi | 学校に行きます。| 🟢 Thuộc |
| 2 | 来ます | きます | Động từ nhóm 3 | Đến | 友達が来ます。| 🟡 Đang học |
| 3 | 帰ります | かえります | Động từ nhóm 1 | Về (nhà) | うちに帰ります。| 🔴 Chưa học |

- 3 nút review: **Flashcard** | **Trắc nghiệm** | **Gõ từ**
- Nút sửa/xóa list
- Nút xóa từng item

**Quan trọng:** mọi chế độ học chỉ dùng từ vựng trong **list đang chọn**.  
Ví dụ học `N4_Bài 1` thì:
- Flashcard chỉ lấy cards của `N4_Bài 1`
- Multiple choice chỉ lấy đáp án nhiễu từ `N4_Bài 1`
- Typing chỉ lấy từ của `N4_Bài 1`
- Không trộn từ từ list khác vào session

**Quan trọng hơn nữa:** review **không tạo thêm vocabulary list mới**.  
Ví dụ:
- User import list `N4_Bài 1`
- Trong list có 30 từ
- Sau khi học, vẫn chỉ có **1 list là `N4_Bài 1`**
- Chỉ có trạng thái/level của từng từ trong list đó thay đổi

---

### R3: Flashcard — Ôn ngắt quãng (SRS)

**User story:** Tôi muốn ôn từ vựng bằng flashcard, hệ thống tự lên lịch ôn dựa trên mức độ nhớ.

**Cách hoạt động:**

```
[Mặt trước]  行きます (いきます)
              Động từ nhóm 1

              [Lật card]

[Mặt sau]    Đi
              VD: 学校に行きます。
              (Tôi đi đến trường.)

              [🔴 Quên]  [🟡 Khó]  [🟢 Nhớ]
```

**Workflow SRS/Leveling được chọn:**
- Mỗi session chỉ học **1 list**
- Mỗi **từ trong list** có level riêng
- Khi trả lời đúng/sai, **level của chính từ đó** tăng/giảm
- Không tạo list ôn tập mới
- User có thể chọn học theo từng nhóm level ngay trong page của list

**Mô hình level đề xuất:**

| Level | Ý nghĩa | Trạng thái |
|------|---------|------------|
| 0 | Chưa học | `new` |
| 1 | Mới nhớ sơ bộ | `learning` |
| 2 | Nhớ cơ bản | `learning` |
| 3 | Đang ôn ổn định | `review` |
| 4 | Nhớ tốt | `review` |
| 5 | Đã thuộc | `mastered` |

**Khi import list mới:**
- Tất cả từ trong list bắt đầu ở:
  - `level = 0`
  - `status = new`
  - `next_review_at = now`

**Nguyên tắc nâng/hạ level:**
- Trả lời đúng ở flashcard / typing / quiz → tăng level
- Trả lời sai → giảm level
- Một từ đã thuộc (`level 5`) nếu trả lời sai thì tụt xuống level thấp hơn để ôn lại
- Level không áp dụng cho cả list, mà áp dụng cho **từng item**

**Ví dụ mong muốn đúng yêu cầu:**
- User import `N4_Bài 1`
- Từ `行きます` ban đầu ở `level 0`
- Học flashcard và trả lời đúng → lên `level 1`
- Lần sau tiếp tục đúng → lên `level 2`
- Khi đủ số lần đúng và đủ lịch ôn → lên dần `level 3`, `4`, `5`
- Nếu sau này quên → tụt từ `level 5` xuống `level 3` hoặc `2` tùy rule
- Tất cả vẫn nằm trong **list `N4_Bài 1`**

**Cách học trong page của một list:**

1. **Học tất cả từ chưa thuộc**
   - Lấy các từ level thấp trong list
   - Dùng khi mới import hoặc đang học bài mới

2. **Ôn theo due**
   - Lấy các từ có `next_review_at <= now` trong list đó

3. **Ôn theo level**
   - User chọn học các từ ở:
     - level 0-1
     - level 2-3
     - level 4-5
     - hoặc toàn bộ list

4. **Kiểm tra lại từ đã thuộc**
   - Lấy các từ `level 5`
   - Nếu user làm sai thì tụt level để quay lại ôn

5. **Reset tiến độ của list**
   - Không tạo list mới
   - Chỉ reset `level/status/schedule` của các từ trong chính list đó

**Card states:**
- `new` — chưa học
- `learning` — level 1-2
- `review` — level 3-4
- `mastered` — level 5
- `relearning` — từng ở level cao nhưng vừa bị tụt xuống để học lại

**Learning/Relearning steps mặc định đề xuất:**
- Learning: `1m -> 10m`
- Relearning: `10m`

**Logic chấm điểm theo level:**
- **Quên** (`quality=1`)
  - Level giảm
  - Nếu đang level cao thì chuyển sang `relearning`
  - `next_review_at` được kéo gần lại
- **Khó** (`quality=3`)
  - Có thể giữ level hoặc tăng chậm
  - `next_review_at` ngắn hơn bình thường
- **Nhớ** (`quality=5`)
  - Level tăng
  - `next_review_at` dài hơn theo scheduler

**Rule đơn giản, đúng tinh thần yêu cầu:**
- `level 0` đúng → `level 1`
- `level 1` đúng → `level 2`
- `level 2` đúng → `level 3`
- `level 3` đúng → `level 4`
- `level 4` đúng → `level 5`
- `level 5` đúng → giữ `level 5`, chỉ tăng interval
- Sai ở bất kỳ level nào → giảm ít nhất 1 level
- Sai ở `level 5` → không còn là `mastered`

**Reset policy:**
- **Soft reset** (mặc định):
  - `level = 0`
  - `status = new`
  - `repetitions = 0`
  - `interval_days = 0`
  - `next_review_at = now`
  - Giữ các thống kê lịch sử nếu có
- **Hard reset** (tuỳ chọn):
  - Xóa progress record và tạo lại như mới
  - Chỉ dùng khi user xác nhận rõ

**Retention target đề xuất:** khoảng **90%**

**Session flow:**
1. User chọn **1 list**
2. User chọn cách học trong list:
   - Học từ mới / level thấp
   - Ôn due
   - Ôn theo level
   - Kiểm tra lại từ đã thuộc
3. Hệ thống dựng queue chỉ từ list đó
4. Hiện card → user trả lời → update `level/status/next_review_at` của từ đó
5. Cards bị "Quên" quay lại cuối hàng đợi trong session
6. Kết thúc → hiện kết quả: X đúng / Y sai / Z tổng, thời gian

**Hiển thị trên list:** Badge "5 từ cần ôn hôm nay" trên mỗi list card

---

### R4: Multiple Choice — Trắc nghiệm

**User story:** Tôi muốn ôn bằng trắc nghiệm 4 lựa chọn từ list của mình.

**2 mode trắc nghiệm:**

**Mode A: JP → VN (Nhìn từ → chọn nghĩa)**
```
Câu hỏi:  行きます (いきます)

  A. Đến          B. Đi ✅
  C. Về (nhà)     D. Ăn

Kết quả: ✅ Chính xác!
```

**Mode B: VN → JP (Nhìn nghĩa → chọn từ)**
```
Câu hỏi:  Đi

  A. 来ます        B. 帰ります
  C. 行きます ✅    D. 食べます

Kết quả: ✅ Chính xác!
```

**Logic:**
- 4 options: 1 đáp án đúng + 3 random từ **cùng list**
- Nếu list < 4 từ → không cho chơi mode này (hiện warning)
- Chọn đúng → xanh + auto next (1.5s delay)
- Chọn sai → đỏ + highlight đáp án đúng + hiện ví dụ
- Kết thúc → Score: 8/10 đúng, thời gian, accuracy %
- Có thể chạy từ `Review Due`, `Check Learned`, hoặc `Relearn / Reset`
- Update SRS:
  - đúng tốt = `quality 5`
  - đúng nhưng khó = `quality 3`
  - sai = `quality 1`
- Nếu check một card `mastered` mà trả lời sai → card tụt level và xuống `relearning`

---

### R5: Typing Test — Gõ từ (Hard mode)

**User story:** Tôi muốn ôn bằng cách gõ từ vựng để nhớ sâu hơn.

**2 mode gõ:**

**Mode A: Gõ Reading (Hiragana)**
```
Từ:    行きます
Nghĩa: Đi
Từ loại: Động từ nhóm 1

Gõ đọc: [___________]

→ User gõ: いきます
→ ✅ Chính xác!
```

**Mode B: Gõ Nghĩa (Tiếng Việt)**
```
Từ:    行きます (いきます)
Từ loại: Động từ nhóm 1

Gõ nghĩa: [___________]

→ User gõ: đi
→ ✅ Chính xác!
```

**Logic:**
- So sánh case-insensitive, trim whitespace
- Cho phép gõ gần đúng: "Đi" = "đi" = " đi " → đúng
- Sai → hiện đáp án đúng + ví dụ + nút "Tiếp tục"
- Đúng → xanh + auto next (1s)
- Kết thúc → Score + hiện các từ sai để xem lại
- Có thể chạy từ `Review Due`, `Check Learned`, hoặc `Relearn / Reset`
- Update SRS:
  - đúng tốt = `quality 5`
  - đúng nhưng chậm/khó = `quality 3`
  - sai = `quality 1`
- Nếu check một card `mastered` mà gõ sai → card tụt level và xuống `relearning`

---

## Database Tables (Module này)

```sql
-- Vocabulary Lists
CREATE TABLE vocabulary_lists (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID NOT NULL REFERENCES users(id),
    name VARCHAR(100) NOT NULL,         -- "N4_Bài 1"
    description TEXT,
    word_count INT DEFAULT 0,
    created_at TIMESTAMP DEFAULT NOW(),
    updated_at TIMESTAMP DEFAULT NOW()
);

-- Vocabulary Items
CREATE TABLE vocabulary_items (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    list_id UUID NOT NULL REFERENCES vocabulary_lists(id) ON DELETE CASCADE,
    word VARCHAR(100) NOT NULL,          -- 行きます
    reading VARCHAR(100) NOT NULL,       -- いきます
    word_type VARCHAR(50) NOT NULL,      -- Động từ nhóm 1
    meaning VARCHAR(200) NOT NULL,       -- Đi
    example_sentence TEXT,               -- 学校に行きます。
    example_meaning TEXT,                -- Tôi đi đến trường.
    order_index INT DEFAULT 0
);

-- User Word Progress (SRS)
CREATE TABLE user_word_progress (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID NOT NULL REFERENCES users(id),
    vocabulary_item_id UUID NOT NULL REFERENCES vocabulary_items(id) ON DELETE CASCADE,
    level INT DEFAULT 0,                 -- 0..5
    repetitions INT DEFAULT 0,
    ease_factor FLOAT DEFAULT 2.5,
    interval_days INT DEFAULT 0,
    next_review_at TIMESTAMP DEFAULT NOW(),
    last_reviewed_at TIMESTAMP,
    status VARCHAR(20) DEFAULT 'new',    -- new|learning|review|mastered|relearning
    lapse_count INT DEFAULT 0,           -- số lần đang review/mastered nhưng quên
    learning_step_index INT DEFAULT 0,   -- bước hiện tại trong learning/relearning
    UNIQUE(user_id, vocabulary_item_id)
);

-- Review Sessions
CREATE TABLE review_sessions (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID NOT NULL REFERENCES users(id),
    list_id UUID NOT NULL REFERENCES vocabulary_lists(id),
    mode VARCHAR(20) NOT NULL,           -- flashcard|multichoice|typing
    total_cards INT,
    correct_count INT DEFAULT 0,
    wrong_count INT DEFAULT 0,
    duration_seconds INT,
    started_at TIMESTAMP DEFAULT NOW(),
    completed_at TIMESTAMP
);
```

---

## API Endpoints

| # | Method | Endpoint | Mô tả | Request Body | Response |
|---|--------|----------|--------|-------------|----------|
| 1 | POST | `/api/vocabulary/lists/import` | Import JSON → tạo list + items | `{ name, words[] }` | `{ listId, wordCount }` |
| 2 | GET | `/api/vocabulary/lists` | Lấy tất cả lists của user | — | `[ { id, name, wordCount, masteredCount, createdAt } ]` |
| 3 | GET | `/api/vocabulary/lists/{id}` | Chi tiết list + items + progress | — | `{ list, items[], progress[] }` |
| 4 | PUT | `/api/vocabulary/lists/{id}` | Sửa tên/description | `{ name, description }` | `{ success }` |
| 5 | DELETE | `/api/vocabulary/lists/{id}` | Xóa list (cascade items) | — | `{ success }` |
| 6 | DELETE | `/api/vocabulary/items/{id}` | Xóa 1 item | — | `{ success }` |
| 7 | GET | `/api/review/{listId}/due` | Lấy cards cần ôn | — | `{ dueCount, cards[] }` |
| 8 | GET | `/api/review/{listId}/learned` | Lấy cards đã học của list để check lại | `?scope=mastered|reviewed|all` | `{ cards[] }` |
| 9 | GET | `/api/review/{listId}/levels` | Lấy cards theo level trong cùng list | `?minLevel=0&maxLevel=2` | `{ cards[] }` |
| 10 | GET | `/api/review/{listId}/all` | Lấy tất cả cards của list | — | `{ cards[] }` |
| 11 | POST | `/api/review/answer` | Submit đáp án 1 card | `{ itemId, quality, mode, sessionType }` | `{ nextReview, newStatus, newLevel }` |
| 12 | POST | `/api/review/session` | Lưu kết quả session | `{ listId, mode, sessionType, total, correct, wrong, duration }` | `{ sessionId }` |
| 13 | POST | `/api/review/{listId}/reset` | Reset progress của list hoặc subset cards | `{ resetType, itemIds? }` | `{ success, affectedCount }` |

---

## Backend Logic Design

### 1. Domain boundary

**Một list = một topic cố định**
- `VocabularyList` chỉ đại diện cho chủ đề như `N4_Bài 1`
- Review flow không được tạo ra list mới
- Mọi tiến độ học được gắn với `VocabularyItem` thông qua `UserWordProgress`

**Một item = một card học**
- Mỗi `VocabularyItem` là một đơn vị review độc lập
- Từng item có `level`, `status`, `next_review_at`, `interval_days` riêng
- Trong cùng một list, có thể có từ ở `level 0`, từ ở `level 3`, và từ ở `level 5`

### 2. Aggregate và responsibilities

**VocabularyList**
- Chứa metadata của list: tên, mô tả, số từ
- Không chứa logic progression

**VocabularyItem**
- Chứa nội dung học: `word`, `reading`, `type`, `meaning`, `example`
- Không chứa progress của user

**UserWordProgress**
- Là aggregate chính cho repetition
- Thuộc về `(user_id, vocabulary_item_id)`
- Chứa toàn bộ trạng thái học của một từ:
  - `level`
  - `status`
  - `repetitions`
  - `ease_factor`
  - `interval_days`
  - `next_review_at`
  - `last_reviewed_at`
  - `lapse_count`
  - `learning_step_index`

**ReviewSession**
- Chỉ lưu kết quả của một lượt học
- Không phải nguồn sự thật cho progress
- Chỉ dùng cho lịch sử, analytics, kết quả cuối session

### 3. Source of truth

**Nguồn sự thật duy nhất cho tiến độ học là `user_word_progress`**
- UI list detail phải đọc level/status từ bảng này
- Review session chỉ đọc và update bảng này
- Không được suy luận progress từ tên list, sub-list, hay session cũ

### 4. Invariants bắt buộc

- Mỗi `(user_id, vocabulary_item_id)` chỉ có đúng 1 progress record
- Import list mới phải tạo progress record cho tất cả items với:
  - `level = 0`
  - `status = new`
  - `next_review_at = now`
- Review query chỉ được lấy item trong `list_id` đã chọn
- Multiple choice distractors chỉ được lấy từ cùng list
- Reset progress chỉ update progress record hiện có, không tạo list mới

### 5. State + level model

**Level là signal chính cho UX**
- `0` = chưa học
- `1-2` = learning
- `3-4` = review
- `5` = mastered

**Status là signal phụ cho scheduler**
- `new`
- `learning`
- `review`
- `mastered`
- `relearning`

**Mapping chuẩn**
- `level 0` -> `new`
- `level 1-2` -> `learning`
- `level 3-4` -> `review`
- `level 5` -> `mastered`
- Khi card từ `level >= 3` trả lời sai -> `relearning`

### 6. Query model

Backend cần hỗ trợ 4 kiểu query cho review:

**A. Due query**
- Input: `listId`
- Rule: lấy cards có `next_review_at <= now`
- Sort: `next_review_at ASC`, sau đó `order_index ASC`

**B. Level range query**
- Input: `listId`, `minLevel`, `maxLevel`
- Rule: chỉ lấy cards trong khoảng level đó
- Dùng cho “học từ mới”, “ôn level thấp”, “ôn level cao”

**C. Learned scope query**
- Input: `listId`, `scope`
- `scope=mastered` -> chỉ level 5
- `scope=reviewed` -> level 3-5
- `scope=all` -> level 1-5

**D. All cards query**
- Input: `listId`
- Rule: lấy tất cả cards của list, chủ yếu cho debug/admin/full reset

### 7. Answer processing pipeline

Mỗi lần submit answer, backend phải làm theo pipeline cố định:

1. Load `UserWordProgress` theo `userId + itemId`
2. Verify item thuộc về list của user
3. Xác định `quality`
   - `1 = Quên`
   - `3 = Khó`
   - `5 = Nhớ`
4. Tính `newLevel`, `newStatus`, `newInterval`, `newNextReviewAt`
5. Update progress record
6. Save transaction
7. Return DTO cho UI:
   - `oldLevel`
   - `newLevel`
   - `oldStatus`
   - `newStatus`
   - `nextReviewAt`
   - `requeueInSession`
   - `requeueAfterSeconds`

### 8. Level progression rules

**Quality = 5**
- Nếu `level < 5` -> tăng `level` lên 1
- Nếu `level = 5` -> giữ nguyên level, tăng interval
- Nếu đang `learning` và đủ step -> chuyển sang `review`

**Quality = 3**
- Nếu `level <= 2` -> có thể giữ nguyên hoặc tăng chậm theo rule
- Nếu `level >= 3` -> giữ level hoặc tăng interval ít hơn `quality = 5`
- Không được xử lý như fail

**Quality = 1**
- Nếu `level = 0` -> giữ `level 0`
- Nếu `level = 1-2` -> giảm 1 level hoặc reset về đầu learning step
- Nếu `level = 3-5` -> giảm ít nhất 1-2 level và chuyển `status = relearning`
- Card phải quay lại cuối queue trong session

### 9. Scheduling rules

**Learning**
- Dùng steps ngắn:
  - step 1: `+1m`
  - step 2: `+10m`
- Chỉ sau khi qua steps mới được graduate sang review

**Review**
- Dùng interval theo ngày
- `quality = 5` tăng mạnh hơn
- `quality = 3` tăng nhẹ hơn

**Mastered**
- Là review state ổn định
- Nếu user làm sai thì card phải tụt level và vào `relearning`

**Relearning**
- Dùng step ngắn:
  - `+10m`
- Sau khi pass lại, quay về `review`, không nhảy thẳng lên `mastered`

### 10. Reset rules

**Soft reset**
- Update progress:
  - `level = 0`
  - `status = new`
  - `repetitions = 0`
  - `interval_days = 0`
  - `learning_step_index = 0`
  - `lapse_count = 0`
  - `next_review_at = now`
- Giữ item và list nguyên vẹn

**Hard reset**
- Xóa progress record cũ
- Tạo lại progress record mới với trạng thái mặc định
- Chỉ dùng khi user xác nhận rõ

### 11. Service design

**IVocabularyService**
- `ImportAsync`
- `GetListsAsync`
- `GetByIdAsync`
- `UpdateAsync`
- `DeleteListAsync`
- `DeleteItemAsync`
- `AddItemAsync`

**IReviewService**
- `GetDueCardsAsync`
- `GetCardsByLevelAsync`
- `GetLearnedCardsAsync`
- `GetAllCardsAsync`
- `SubmitAnswerAsync`
- `SaveSessionAsync`
- `ResetListProgressAsync`

**IScheduler / SrsAlgorithm**
- Pure function, không truy cập DB
- Input:
  - current progress
  - quality
  - current time
- Output:
  - new level
  - new status
  - new interval
  - next review
  - requeue metadata

### 12. DTO contract cho UI

`ReviewCardDto` nên có:
- `itemId`
- `word`
- `reading`
- `wordType`
- `meaning`
- `exampleSentence`
- `exampleMeaning`
- `level`
- `status`
- `nextReviewAt`

`ReviewAnswerResultDto` nên có:
- `itemId`
- `oldLevel`
- `newLevel`
- `oldStatus`
- `newStatus`
- `nextReviewAt`
- `intervalDays`
- `requeueInSession`
- `requeueAfterSeconds`

### 13. Điều current backend đang lệch so với yêu cầu

- Nếu backend chỉ update `status` mà không có `level`, UI sẽ không thể hiển thị đúng yêu cầu
- Nếu query learned/review không filter theo `listId`, sẽ sai nguyên tắc module
- Nếu reset tạo progress/list mới thay vì update progress hiện có, sẽ sai yêu cầu
- Nếu review logic tập trung vào “session type” mà không coi `VocabularyItem` là đơn vị chính, flow sẽ bị lệch

---

## Tasks

### Backend (.NET)

- [ ] B1: Tạo Entities: `VocabularyList`, `VocabularyItem`, `UserWordProgress`, `ReviewSession` trong `JPLearn.Core/Entities/` → Verify: `dotnet build` OK
- [ ] B2: Tạo DTOs: `ImportVocabularyDto`, `VocabularyListDto`, `VocabularyItemDto`, `ReviewAnswerDto` → Verify: build OK
- [ ] B3: Cấu hình DbContext — DbSet + Fluent API (indexes, relationships, cascade delete) → Verify: `dotnet ef migrations add VocabularyModule` thành công
- [ ] B4: `IVocabularyService` + `VocabularyService` — Import JSON (parse, validate, batch insert), GetLists, GetById, Update, Delete → Verify: unit test import 10 từ thành công
- [ ] B5: `SrsAlgorithm.cs` — static method `Calculate(quality, repetitions, easeFactor, interval)` → returns `(newReps, newEF, newInterval, nextReview)` → Verify: unit test 3 cases (quên/khó/nhớ)
- [ ] B6: Thiết kế state machine + level progression cho từng từ trong cùng list → Verify: unit test state transitions đúng
- [ ] B7: `IReviewService` + `ReviewService` — GetDueCards, GetLearnedCards, GetCardsByLevel, GetAllCards, SubmitAnswer (gọi scheduler), SaveSession, ResetListProgress → Verify: submit answer → level/status update đúng
- [ ] B8: `VocabularyListController` — 5 endpoints (import, getAll, getById, update, delete) → Verify: Swagger test CRUD thành công
- [ ] B9: `ReviewController` — endpoints cho `due`, `learned`, `all`, `answer`, `session`, `reset` → Verify: Swagger test review flow
- [ ] B10: User isolation — tất cả queries filter `WHERE user_id = currentUserId` → Verify: 2 users, user A không thấy list user B

### Frontend (React)

- [ ] F1: Types — `VocabularyList`, `VocabularyItem`, `WordProgress`, `ReviewSession` trong `types/vocabulary.ts` → Verify: no TypeScript errors
- [ ] F2: API client — `vocabulary.api.ts` (import, getLists, getById, delete) + `review.api.ts` (getDue, getLearned, getAll, answer, session, reset) → Verify: functions gọi đúng endpoints
- [ ] F3: `ImportVocabularyPage` — Form (tên + JSON textarea) + Preview table + Validate + Submit → Verify: paste JSON → preview → import thành công
- [ ] F4: `VocabularyListsPage` — Grid cards (tên, X từ, Y% thuộc, "Z cần ôn") + nút Import mới → Verify: hiện danh sách lists
- [ ] F5: `VocabularyDetailPage` — Bảng items (word, reading, type, meaning, example, level, status) + 3 nút review + delete → Verify: click list → thấy items + level/status
- [ ] F6: `Flashcard.tsx` — Flip card (mặt trước: word+reading+type, mặt sau: meaning+example) + animation → Verify: click flip → xoay mượt
- [ ] F7: `ReviewEntrySelector` — Chọn học theo `due` / `level range` / `mastered check` trong từng list → Verify: chọn entry point đúng
- [ ] F8: `FlashcardReview` — Session flow theo list: load cards theo entry point → hiện Flashcard → 3 nút (Quên/Khó/Nhớ) → call API → next → kết quả → Verify: hoàn thành 5 cards → hiện score
- [ ] F9: `MultipleChoice.tsx` — Hiện câu hỏi + 4 options + feedback (đúng/sai) + hiện ví dụ khi sai → Verify: chọn đáp án → feedback đúng
- [ ] F10: `MultipleChoiceReview` — Session flow theo list + entry point → random questions → 4 options từ cùng list → score → Verify: hoàn thành quiz → score
- [ ] F11: `TypingQuiz.tsx` — Input field + so sánh đáp án (case-insensitive, trim) + feedback → Verify: gõ đúng → xanh, sai → đỏ + show answer
- [ ] F12: `TypingReview` — Session flow theo list + entry point → gõ → check → score + hiện từ sai → Verify: hoàn thành → kết quả
- [ ] F13: `ResetProgressModal` — Chọn `reset all` / `reset mastered` / `reset selected` + xác nhận soft reset → Verify: reset level/status trong cùng list
- [ ] F14: `ReviewSelectPage` — Chọn mode (Flashcard/Trắc nghiệm/Gõ từ) + chọn sub-mode (JP→VN / VN→JP) + scope học → Verify: chọn → navigate đúng
- [ ] F15: `ReviewResultPage` — Hiện kết quả: score, accuracy%, thời gian, danh sách từ sai → Verify: hiện đúng data

**Dependencies:**
``` 
B1 → B2 → B3 → B4 → B5 → B6 → B7 → B8 → B9 → B10
F1 → F2 → F3, F4, F5 (parallel)
F6 → F8
F7 → F8, F10, F12, F14
F9 → F10
F11 → F12
F8 + F10 + F12 → F15
F13 độc lập sau khi có backend reset
```

---

## Phase X: Verification

- [ ] Import JSON 10 từ "N4_Bài 1" → list hiện đúng 10 items
- [ ] User A import → User B không thấy
- [ ] Review Due: vào `N4_Bài 1` chỉ thấy due cards của `N4_Bài 1`
- [ ] List mới import: mọi từ bắt đầu ở `level 0`
- [ ] Trả lời đúng một từ ở flashcard/typing → từ đó tăng level trong cùng list
- [ ] Không tạo thêm vocabulary list mới trong quá trình ôn tập
- [ ] Flashcard: đánh giá Quên → card quay lại cuối hàng đợi trong session
- [ ] Check Learned: chọn `Mastered only` → chỉ lấy cards mastered trong list
- [ ] Check Learned: trả lời sai một card `mastered` → card giảm level và chuyển xuống `relearning`
- [ ] Relearn / Reset: soft reset toàn bộ list → cards quay về `new`, không trộn list khác
- [ ] Multiple Choice: 4 options hiện ra, đáp án đúng trong đó
- [ ] Multiple Choice: cả 3 distractors đều thuộc cùng list đang học
- [ ] Multiple Choice: list < 4 từ → warning không cho chơi
- [ ] Typing: gõ "đi" hoặc "Đi" hoặc " đi " đều được chấp nhận
- [ ] Typing: gõ sai → hiện đáp án + ví dụ
- [ ] Typing: gõ sai một card đã thuộc → card tụt xuống `relearning`
- [ ] Kết quả review: score, accuracy, thời gian chính xác
- [ ] `dotnet build` + `npm run build` — no errors
