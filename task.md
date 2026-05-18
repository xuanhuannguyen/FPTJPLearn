# Frontend-Only Migration Tasks

## Phase 1 — Static Data Layer
- [x] Create `staticDataService.ts` with fetch + in-memory cache
- [x] Convert speaking source data → JSON for speaking
- [x] Create `public/data/` JSON files for all features

## Phase 2 — Vocabulary
- [x] Rewrite `vocabularyApi.ts` → use staticDataService
- [x] Keep `addToMemory` + `getMemoryStatus` → backend still

## Phase 3 — Kanji
- [x] Rewrite `kanjiApi.ts` → use staticDataService

## Phase 4 — Grammar (data only)
- [x] Rewrite grammar data calls → staticDataService
- [x] Move exercise checking → frontend logic
- [x] Keep SRS (addToStudy, getDueCards, submitReviewAnswer) → backend

## Phase 5 — Speaking
- [x] Rewrite `speakingApi.ts` → use staticDataService

## Phase 6 — Access Control
- [x] Create `useUserAccess` hook for frontend isLocked

## Phase 7 — Exam Practice
- [x] Create static exam JSON data
- [x] Rewrite `examApi.ts` → use staticDataService
- [x] Move study answer checking → frontend logic
- [x] Move exam attempts/review → localStorage frontend logic
