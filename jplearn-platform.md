# JPLearn Platform — Implementation Plan

## Overview

**Nền tảng học tiếng Nhật** lấy cảm hứng từ NhaiKanji.com, cho phép user tạo tài khoản riêng, import từ vựng qua JSON, ôn tập bằng SRS (SM-2), và mở rộng sang Kanji/Ngữ pháp.

**Project Type:** WEB (Fullstack — React SPA + .NET API)
**Mục tiêu kép:** Học React + .NET & tạo product bán được

---

## Success Criteria

- [ ] User đăng ký/đăng nhập được (JWT auth)
- [ ] User import JSON → tạo vocabulary list thành công
- [ ] Flashcard review với SM-2 algorithm hoạt động
- [ ] Typing quiz & Multiple choice quiz hoạt động
- [ ] Mỗi user chỉ thấy data của mình (per-user isolation)
- [ ] Dark theme UI giống NhaiKanji
- [ ] Deploy được lên Vercel (FE) + Railway (BE)

---

## Tech Stack

| Layer | Technology | Lý do |
|-------|-----------|-------|
| **Frontend** | Vite + React 19 + TypeScript | Học React core, SPA thuần |
| **Styling** | CSS Modules + CSS Variables | Dark theme, no framework dependency |
| **Routing** | React Router v7 | SPA navigation |
| **State** | Zustand | Lightweight, dễ học |
| **Backend** | .NET 8 Web API | Học .NET, production-grade |
| **ORM** | EF Core 8 | Code-first, migrations |
| **Database** | PostgreSQL | Reliable, free hosting (Neon/Railway) |
| **Auth** | ASP.NET Identity + JWT | Industry standard |
| **Deploy FE** | Vercel | Free, fast CDN |
| **Deploy BE** | Railway | Free tier 500h/tháng |

---

## Database Schema

```
Users
├── id (uuid, PK)
├── email (unique)
├── password_hash
├── display_name
├── role (free | premium)
├── premium_expires_at
├── created_at

VocabularyLists
├── id (uuid, PK)
├── user_id (FK → Users)
├── name
├── description
├── level (N5|N4|N3|N2|N1|custom)
├── word_count
├── created_at / updated_at

VocabularyItems
├── id (uuid, PK)
├── list_id (FK → VocabularyLists)
├── word (食べる)
├── reading (たべる)
├── meaning (ăn)
├── word_type (動詞|名詞|形容詞...)
├── example_sentence
├── example_meaning
├── order_index

UserWordProgress (SRS State)
├── id (uuid, PK)
├── user_id (FK → Users)
├── vocabulary_item_id (FK → VocabularyItems)
├── repetitions (int)
├── ease_factor (float, default 2.5)
├── interval_days (int)
├── next_review_at (timestamp)
├── last_reviewed_at
├── status (new|learning|review|mastered)

ReviewSessions
├── id (uuid, PK)
├── user_id (FK → Users)
├── list_id (FK → VocabularyLists)
├── mode (flashcard|typing|multichoice)
├── total_cards / correct_count / wrong_count
├── duration_seconds
├── started_at / completed_at

--- Phase 2: Kanji ---
KanjiLessons
├── id, level (N5→N1), lesson_number, title

KanjiItems
├── id, lesson_id (FK), character, onyomi, kunyomi
├── meaning, stroke_count, examples_json

UserKanjiProgress
├── id, user_id, kanji_item_id
├── (same SRS fields as UserWordProgress)

--- Phase 3: Grammar ---
GrammarPatterns
├── id, user_id, level, pattern, meaning
├── structure, example_sentence, example_meaning, notes

--- Phase 4: Premium ---
LicenseKeys
├── id, key_code (unique), duration_days
├── is_used, used_by (FK), used_at, created_at
```

---

## File Structure

```
JPLearn/
├── client/                          # React Frontend
│   ├── src/
│   │   ├── api/                     # API client (axios)
│   │   │   ├── auth.api.ts
│   │   │   ├── vocabulary.api.ts
│   │   │   └── axios.config.ts
│   │   ├── components/
│   │   │   ├── layout/ (Navbar, Sidebar, Layout)
│   │   │   ├── ui/ (Button, Card, Modal, JsonImportForm)
│   │   │   └── review/ (Flashcard, TypingQuiz, MultipleChoice)
│   │   ├── pages/
│   │   │   ├── auth/ (LoginPage, RegisterPage)
│   │   │   ├── dashboard/ (DashboardPage)
│   │   │   ├── vocabulary/ (ListsPage, DetailPage, ImportPage)
│   │   │   └── review/ (SelectPage, SessionPage)
│   │   ├── store/ (auth.store.ts, vocabulary.store.ts)
│   │   ├── hooks/
│   │   ├── styles/ (globals.css + component CSS modules)
│   │   ├── types/
│   │   ├── App.tsx, Router.tsx, main.tsx
│   ├── package.json, vite.config.ts, tsconfig.json
│
├── server/                          # .NET Backend
│   ├── JPLearn.Api/
│   │   ├── Controllers/ (Auth, VocabularyList, VocabularyItem, Review)
│   │   ├── Program.cs, appsettings.json
│   ├── JPLearn.Core/
│   │   ├── Entities/ (User, VocabularyList, VocabularyItem, etc.)
│   │   ├── DTOs/
│   │   ├── Interfaces/
│   │   └── Services/ (VocabularyService, ReviewService, SrsAlgorithm)
│   ├── JPLearn.Infrastructure/
│   │   ├── Data/ (AppDbContext, Migrations/)
│   │   ├── Repositories/
│   │   └── Extensions/
│   └── JPLearn.sln
│
├── docs/
├── .gitignore
└── README.md
```

---

## Task Breakdown

### Module 0: Foundation (P0)

- [ ] 0.1 Tạo .NET solution: Api + Core + Infrastructure → Verify: `dotnet build` OK
- [ ] 0.2 Tạo Vite React project `client/` + TypeScript → Verify: `npm run dev` OK
- [ ] 0.3 Setup PostgreSQL + EF Core + AppDbContext → Verify: migration thành công
- [ ] 0.4 Tạo Entity classes (User, VocabularyList, VocabularyItem, UserWordProgress, ReviewSession) → Verify: tables tạo đúng
- [ ] 0.5 Setup ASP.NET Identity + JWT auth (register/login/refresh) → Verify: POST /api/auth/login → JWT
- [ ] 0.6 Setup CORS cho FE (5173) ↔ BE (5000) → Verify: không bị CORS block
- [ ] 0.7 CSS Variables + Dark theme system `globals.css` → Verify: dark theme render đúng
- [ ] 0.8 Layout components (Navbar, Sidebar) → Verify: navigation hoạt động
- [ ] 0.9 React Router + protected routes + axios JWT interceptor → Verify: redirect khi chưa auth
- [ ] 0.10 Login/Register pages + kết nối API → Verify: đăng ký → đăng nhập → dashboard

### Module 1: Vocabulary CRUD (P0)

- [ ] 1.1 API: CRUD VocabularyList (filter by user_id) → Verify: user A không thấy data user B
- [ ] 1.2 API: CRUD VocabularyItem → Verify: add 5 items → getById trả đủ 5
- [ ] 1.3 API: Import JSON endpoint (parse + validate + batch insert) → Verify: POST JSON → list tạo đúng
- [ ] 1.4 FE: VocabularyListsPage (grid lists + progress) → Verify: hiện lists
- [ ] 1.5 FE: ImportVocabularyPage (JSON textarea + preview + submit) → Verify: paste → preview → submit
- [ ] 1.6 FE: VocabularyDetailPage (items table) → Verify: click list → thấy từ vựng
- [ ] 1.7 FE: Edit/Delete list + items → Verify: sửa/xóa hoạt động

### Module 2: SRS Review Engine (P0)

- [ ] 2.1 Implement SM-2 algorithm `SrsAlgorithm.cs` → Verify: unit test pass
- [ ] 2.2 API: GET /review/{listId}/due (cards cần ôn) → Verify: trả đúng cards due
- [ ] 2.3 API: POST /review/answer (update SRS state) → Verify: progress update đúng
- [ ] 2.4 API: POST /review/session (lưu kết quả) → Verify: session saved
- [ ] 2.5 FE: Flashcard component (flip animation) → Verify: flip mượt
- [ ] 2.6 FE: 3 nút đánh giá (Quên/Khó/Dễ) → Verify: click → next card + SRS update
- [ ] 2.7 FE: TypingQuiz component → Verify: gõ đúng → xanh, sai → đỏ
- [ ] 2.8 FE: MultipleChoice component (4 options) → Verify: random options, click → feedback
- [ ] 2.9 FE: ReviewSelectPage (chọn list + mode) → Verify: chọn → bắt đầu
- [ ] 2.10 FE: ReviewSessionPage (quiz flow + kết quả) → Verify: hoàn thành → score

### Module 3: Dashboard (P1)

- [ ] 3.1 API: GET /dashboard/stats → Verify: trả aggregate data đúng
- [ ] 3.2 FE: DashboardPage (stats cards + "Cần ôn hôm nay") → Verify: hiện đúng stats

### Module 4: Kanji (P1 — Phase 2)

- [ ] 4.1 Entities: KanjiLesson, KanjiItem, UserKanjiProgress + migration → Verify: tables OK
- [ ] 4.2 API: CRUD Kanji + seed data N5 → Verify: GET lessons trả data
- [ ] 4.3 API: Kanji review (reuse SRS engine) → Verify: review hoạt động
- [ ] 4.4 FE: KanjiLessonsPage (grid by level) → Verify: hiện lessons
- [ ] 4.5 FE: KanjiDetailPage (on/kun, meaning) → Verify: chi tiết đúng
- [ ] 4.6 FE: Kanji review (reuse Flashcard/Typing/MC) → Verify: review hoạt động

### Module 5: Grammar (P2 — Phase 3)

- [ ] 5.1 Entity GrammarPattern + migration → Verify: table OK
- [ ] 5.2 API: CRUD grammar patterns (per-user) → Verify: CRUD hoạt động
- [ ] 5.3 FE: GrammarListPage + GrammarFormPage → Verify: thêm/xem ngữ pháp

### Module 6: Premium (P2 — Phase 4)

- [ ] 6.1 Entity LicenseKey + migration → Verify: table OK
- [ ] 6.2 API: Activate key + admin generate keys → Verify: nhập key → premium
- [ ] 6.3 Middleware: Feature gating → Verify: free user bị block
- [ ] 6.4 FE: Upgrade page + key input → Verify: nhập key thành công

---

## JSON Import Format

```json
{
  "name": "Từ vựng N3 - Công việc",
  "level": "N3",
  "words": [
    {
      "word": "出張",
      "reading": "しゅっちょう",
      "meaning": "công tác",
      "type": "名詞",
      "example": "来週、大阪に出張します。",
      "exampleMeaning": "Tuần sau tôi đi công tác Osaka."
    }
  ]
}
```

Required: `word`, `reading`, `meaning`. Optional: `type`, `example`, `exampleMeaning`. Max 500 words/list.

---

## SM-2 Algorithm

```
IF quality >= 3: interval tăng theo ease_factor
ELSE: reset về 1 ngày

UX: Quên (q=1) | Khó (q=3) | Dễ (q=5)
ease_factor = max(1.3, EF + 0.1 - (5-q) * (0.08 + (5-q) * 0.02))
```

---

## API Endpoints

| Method | Endpoint | Mô tả |
|--------|----------|--------|
| POST | /api/auth/register | Đăng ký |
| POST | /api/auth/login | Đăng nhập → JWT |
| POST | /api/auth/refresh | Refresh token |
| GET | /api/vocabulary/lists | Lists của user |
| POST | /api/vocabulary/lists | Tạo list |
| GET | /api/vocabulary/lists/{id} | Chi tiết + items |
| PUT | /api/vocabulary/lists/{id} | Update list |
| DELETE | /api/vocabulary/lists/{id} | Xóa list |
| POST | /api/vocabulary/lists/import | Import JSON |
| POST | /api/vocabulary/items | Thêm item |
| DELETE | /api/vocabulary/items/{id} | Xóa item |
| GET | /api/review/{listId}/due | Cards cần ôn |
| POST | /api/review/answer | Submit + update SRS |
| POST | /api/review/session | Lưu phiên ôn |
| GET | /api/dashboard/stats | Thống kê |

---

## Phase X: Verification

- [ ] `dotnet build` — no errors
- [ ] `npm run build` — no errors
- [ ] Security: No hardcoded secrets, JWT validated
- [ ] Per-user isolation: User A ≠ User B data
- [ ] SRS: SM-2 correct intervals (unit tests)
- [ ] UI: Dark theme consistent
- [ ] CORS: FE↔BE works
- [ ] Responsive: Mobile-friendly

---

## Implementation Order

```
Phase 1 (MVP): Module 0 → 1 → 2 → 3  (~3-4 tuần)
Phase 2:       Module 4 (Kanji)        (~1-2 tuần)
Phase 3:       Module 5 (Grammar)      (~1 tuần)
Phase 4:       Module 6 (Premium)      (~1 tuần)
```
