# Frontend-Only Migration Plan (Static Content Features)

## Mục tiêu

Tách 4 feature hiện dùng backend API thành **static JSON + frontend-only logic**, giữ nguyên UX và access control (isLocked/accessTier). Backend chỉ còn phục vụ: auth, purchase, active-vocabulary, memory/SRS.

---

## Phân tích hiện trạng

### Features cần migrate → Static

| Feature | API calls hiện tại | Backend thực sự cần? |
|---|---|---|
| **Vocabulary** | getCourses, getLessons, getLessonById, getPracticeCards, recordView/Practice, addToMemory | ❌ Data tĩnh. `addToMemory` → giữ, nhưng record*Practice/View bỏ |
| **Kanji** | getLevelStats, getLessonsByLevel, getLessonById, getKanjiItems, getVocabulary | ❌ Data tĩnh |
| **Grammar** | getLevels, getLessons, getLessonById, getPattern, searchPatterns, exercises, checkExercise | ❌ Data tĩnh. `addToStudy/removeFromStudy`, review SRS → giữ ở backend |
| **Speaking** | getCourses, getLessonsByCourse, getLesson | ❌ Data tĩnh hoàn toàn |

### Features giữ nguyên backend

- `active-vocabulary` → per-user list
- `memory` → SRS per-user (nextReviewAt, intervals)
- `grammar` SRS phần review (`getDueCards`, `submitReviewAnswer`) → giữ backend
- `grammar` addToStudy/removeFromStudy → giữ backend
- `auth` + `payment/order` → bắt buộc

---

## Kiến trúc đề xuất

```
client/
  public/
    data/                        ← Static JSON files (served as-is)
      vocabulary/
        courses.json
        {courseCode}/
          lessons.json
          lessons/{lessonId}.json
      kanji/
        levels.json
        {level}/
          lessons.json
          lessons/{lessonId}.json  ← chứa cả kanjiItems + vocabulary
      grammar/
        levels.json
        {level}/
          lessons.json
          lessons/{lessonId}.json  ← chứa cả patterns + exercises
      speaking/
        courses.json
        {courseCode}/
          lessons.json
          lessons/{lessonId}.json
  src/
    shared/
      services/
        staticDataService.ts     ← fetch() wrapper với cache (Map<string, Promise>)
```

### staticDataService pattern

```ts
const cache = new Map<string, Promise<unknown>>();

export function fetchStatic<T>(path: string): Promise<T> {
  if (!cache.has(path)) {
    cache.set(path, fetch(`/data/${path}`).then(r => r.json()));
  }
  return cache.get(path) as Promise<T>;
}
```

Dùng `fetch()` thay `axios`. Cache in-memory tránh re-fetch khi navigate.

---

## Open Questions

> [!IMPORTANT]
> **Dữ liệu JSON lấy từ đâu?**
> 
> Có 2 lựa chọn:
> - **A) Export từ DB hiện tại** → chạy script .NET/SQL export ra JSON → copy vào `public/data/`
> - **B) Seed từ material folder** → dùng `material/` hiện có trong project
>
> Recommend: **A** — đảm bảo data đồng bộ với DB đang dùng.

> [!IMPORTANT]
> **Grammar: `checkExercise` hiện gọi backend để check đáp án**
> 
> Nếu static, logic check phải chuyển lên frontend. Tức là `expectedAnswer` và `acceptableAnswers` phải có trong JSON.
> Câu hỏi: **Hiện tại `expectedAnswer` có được trả về trong `getPatternExercises`?** Hay chỉ qua `revealExerciseAnswer`?
> 
> Nếu câu trả lời nằm ở `revealExerciseAnswer` thì cần đưa vào JSON bài học.

> [!WARNING]
> **`isLocked` / `accessTier` trên static JSON**
> 
> Hiện tại backend trả `isLocked` dựa vào subscription của user. Nếu dùng static JSON, `isLocked` sẽ cố định — user premium vẫn thấy locked.
>
> Giải pháp: Khi user login, **JWT claim** chứa danh sách `packageCodes` đã mua. Frontend tự compute `isLocked = !userPackages.includes(lesson.packageCode)`.
> 
> Cần confirm: **JWT hiện tại có chứa purchased packages không?**

---

## Proposed Changes

### Phase 1 — Static Data Layer

#### [NEW] `client/src/shared/services/staticDataService.ts`
Fetch JSON từ `/data/`, cache in-memory, typed generics.

#### [NEW] `client/public/data/**/*.json`
Export từ DB hoặc seed. Cấu trúc khớp với type definitions hiện có.

---

### Phase 2 — Vocabulary (Frontend-Only)

#### [MODIFY] `client/src/features/vocabulary/api/vocabularyApi.ts`
- Xóa import `apiClient`
- Replace tất cả methods bằng `fetchStatic()`
- Bỏ: `recordView`, `recordFlashcardPractice`, `recordMultipleChoicePractice`, `recordTypingPractice` (no-op hoặc xóa)
- Giữ: `addToMemory`, `getMemoryStatus` → vẫn gọi backend (per-user data)

---

### Phase 3 — Kanji (Frontend-Only)

#### [MODIFY] `client/src/features/kanji/api/kanjiApi.ts`
- Xóa `apiClient`, `lessonDetailCache` Map (thay bằng `staticDataService` cache)
- Replace `getLevelStats`, `getLessonsByLevel`, `getLessonById`, `getKanjiItemsByLesson`, `getVocabularyByLesson` → `fetchStatic()`
- `getLevelStats` → tính `progressPercentage` từ 0 (hoặc đọc từ memory feature nếu cần)

---

### Phase 4 — Grammar (Frontend-Only Data, Backend SRS)

#### [MODIFY] `client/src/features/grammar/api/grammarApi.ts`
- `getLevels`, `getLessonsByLevel`, `getLessonById`, `getPatternById`, `searchPatterns` → `fetchStatic()`
- `getPatternExercises`, `checkExercise`, `revealExerciseAnswer` → frontend logic (cần answer trong JSON)
- **Giữ nguyên backend**: `addToStudy`, `removeFromStudy`, `getDueCards`, `submitReviewAnswer`, `getProgressSummary`

---

### Phase 5 — Speaking (Frontend-Only)

#### [MODIFY] `client/src/features/speaking/api/speakingApi.ts`
- Xóa `apiClient`
- Replace 3 methods bằng `fetchStatic()`
- Pages không cần thay đổi logic — chỉ data source đổi

---

### Phase 6 — Access Control (isLocked)

#### [MODIFY] `client/src/shared/hooks/useUserAccess.ts` *(new hook)*
- Đọc JWT claim `purchasedPackages: string[]`
- Export `isPackageLocked(packageCode?: string): boolean`
- Dùng trong tất cả pages thay cho `lesson.isLocked` từ API

---

## Verification Plan

### Automated
- Build pass: `npm run build` không lỗi
- TypeScript: `tsc --noEmit`

### Manual
1. Navigate qua Vocabulary → Kanji → Grammar → Speaking, data load đúng
2. Locked lesson hiển thị đúng với user free vs premium
3. Grammar exercises check đáp án đúng (frontend logic)
4. Grammar SRS review (getDueCards/submit) vẫn hoạt động qua backend
5. active-vocabulary và memory không bị ảnh hưởng
6. Network tab: không còn API call đến `/vocabulary/`, `/kanji/`, `/speaking/` (chỉ `/data/*.json`)

---

## Thứ tự thực thi

1. Confirm 2 open questions trên
2. Export JSON từ DB → `public/data/`
3. Build `staticDataService`
4. Migrate từng feature (Speaking → Kanji → Vocabulary → Grammar)
5. Implement `useUserAccess` hook
6. Test toàn bộ
