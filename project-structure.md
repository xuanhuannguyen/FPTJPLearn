# JPLearn — Scalable Project Structure (Feature-based)

> Khi thêm nhiều module → nhóm theo **feature/module**, không nhóm theo type.

## Nguyên tắc

```
❌ Group by Type (khó scale):       ✅ Group by Feature (dễ scale):
Controllers/                        Features/
  AuthController.cs                   Auth/
  VocabularyController.cs               AuthController.cs
  KanjiController.cs                    AuthService.cs
  GrammarController.cs                  AuthDtos.cs
Services/                             Vocabulary/
  VocabularyService.cs                  VocabularyController.cs
  KanjiService.cs                       VocabularyService.cs
  GrammarService.cs                     VocabularyEntities.cs
→ 10 modules = mỗi folder 10 files    → Thêm module = thêm 1 folder
```

---

## Server (.NET) — Scalable

```
server/
├── JPLearn.sln
│
├── JPLearn.Api/
│   ├── Program.cs
│   ├── appsettings.json
│   └── Middleware/
│       └── UserIdMiddleware.cs
│
├── JPLearn.Core/                     # Domain + Business Logic
│   │
│   ├── Common/                       # ── Shared code ──
│   │   ├── Entities/
│   │   │   └── BaseEntity.cs         # Id, CreatedAt, UpdatedAt
│   │   ├── Interfaces/
│   │   │   └── IRepository.cs
│   │   └── Services/
│   │       └── SrsAlgorithm.cs       # SM-2 (dùng chung cho mọi module)
│   │
│   ├── Auth/                         # ── Module Auth ──
│   │   ├── AuthController.cs
│   │   ├── AuthService.cs
│   │   ├── IAuthService.cs
│   │   └── DTOs/
│   │       ├── LoginDto.cs
│   │       ├── RegisterDto.cs
│   │       └── AuthResponseDto.cs
│   │
│   ├── Vocabulary/                   # ── Module Từ vựng ──
│   │   ├── VocabularyController.cs
│   │   ├── VocabularyService.cs
│   │   ├── IVocabularyService.cs
│   │   ├── Entities/
│   │   │   ├── VocabularyList.cs
│   │   │   ├── VocabularyItem.cs
│   │   │   └── UserWordProgress.cs
│   │   └── DTOs/
│   │       ├── ImportVocabularyDto.cs
│   │       ├── VocabularyListDto.cs
│   │       └── VocabularyItemDto.cs
│   │
│   ├── Review/                       # ── Module Ôn tập ──
│   │   ├── ReviewController.cs
│   │   ├── ReviewService.cs
│   │   ├── IReviewService.cs
│   │   ├── Entities/
│   │   │   └── ReviewSession.cs
│   │   └── DTOs/
│   │       ├── ReviewAnswerDto.cs
│   │       └── SaveSessionDto.cs
│   │
│   ├── Kanji/                        # ── Module Kanji (Phase 2) ──
│   │   ├── KanjiController.cs
│   │   ├── KanjiService.cs
│   │   ├── Entities/
│   │   │   ├── KanjiLesson.cs
│   │   │   ├── KanjiItem.cs
│   │   │   └── UserKanjiProgress.cs
│   │   └── DTOs/
│   │
│   ├── Grammar/                      # ── Module Ngữ pháp (Phase 3) ──
│   │   ├── GrammarController.cs
│   │   ├── GrammarService.cs
│   │   ├── Entities/
│   │   │   └── GrammarPattern.cs
│   │   └── DTOs/
│   │
│   ├── Premium/                      # ── Module Premium (Phase 4) ──
│   │   ├── PremiumController.cs
│   │   ├── LicenseService.cs
│   │   ├── Entities/
│   │   │   └── LicenseKey.cs
│   │   └── DTOs/
│   │
│   └── [FutureModule]/               # ── Thêm module mới = thêm folder ──
│       ├── Controller.cs
│       ├── Service.cs
│       ├── Entities/
│       └── DTOs/
│
└── JPLearn.Infrastructure/           # Data Access Layer
    ├── Data/
    │   ├── AppDbContext.cs            # Tất cả DbSets
    │   ├── Migrations/
    │   └── Configurations/            # Fluent API per entity
    │       ├── VocabularyListConfig.cs
    │       ├── KanjiItemConfig.cs
    │       └── ...
    ├── Repositories/
    │   └── Repository.cs
    └── Extensions/
        └── ServiceCollectionExtensions.cs  # Auto-register tất cả modules
```

---

## Client (React) — Scalable

```
client/src/
│
├── main.tsx
├── App.tsx
├── Router.tsx
│
├── shared/                           # ── Shared code ──
│   ├── api/
│   │   └── axios.config.ts           # Base config, JWT interceptor
│   ├── components/
│   │   ├── Layout.tsx
│   │   ├── Navbar.tsx
│   │   ├── Sidebar.tsx
│   │   ├── Button.tsx
│   │   ├── Card.tsx
│   │   ├── Modal.tsx
│   │   └── Toast.tsx
│   ├── hooks/
│   │   └── useAuth.ts
│   ├── store/
│   │   └── auth.store.ts
│   ├── types/
│   │   └── common.types.ts
│   └── styles/
│       ├── globals.css
│       └── animations.css
│
├── features/                         # ── Mỗi module = 1 folder ──
│   │
│   ├── auth/
│   │   ├── pages/
│   │   │   ├── LoginPage.tsx
│   │   │   └── RegisterPage.tsx
│   │   ├── api/
│   │   │   └── auth.api.ts
│   │   └── auth.routes.tsx           # Route definitions cho module
│   │
│   ├── dashboard/
│   │   ├── pages/
│   │   │   └── DashboardPage.tsx
│   │   └── dashboard.routes.tsx
│   │
│   ├── vocabulary/
│   │   ├── pages/
│   │   │   ├── VocabularyListsPage.tsx
│   │   │   ├── VocabularyDetailPage.tsx
│   │   │   └── ImportVocabularyPage.tsx
│   │   ├── components/
│   │   │   └── VocabularyCard.tsx
│   │   ├── api/
│   │   │   └── vocabulary.api.ts
│   │   ├── store/
│   │   │   └── vocabulary.store.ts
│   │   ├── types/
│   │   │   └── vocabulary.types.ts
│   │   └── vocabulary.routes.tsx
│   │
│   ├── review/
│   │   ├── pages/
│   │   │   ├── ReviewSelectPage.tsx
│   │   │   ├── FlashcardReviewPage.tsx
│   │   │   ├── MultiChoiceReviewPage.tsx
│   │   │   ├── TypingReviewPage.tsx
│   │   │   └── ReviewResultPage.tsx
│   │   ├── components/
│   │   │   ├── Flashcard.tsx
│   │   │   ├── MultipleChoice.tsx
│   │   │   ├── TypingQuiz.tsx
│   │   │   └── ReviewResult.tsx
│   │   ├── api/
│   │   │   └── review.api.ts
│   │   ├── hooks/
│   │   │   └── useReview.ts
│   │   └── review.routes.tsx
│   │
│   ├── kanji/                        # Phase 2
│   │   ├── pages/
│   │   ├── components/
│   │   ├── api/
│   │   └── kanji.routes.tsx
│   │
│   ├── grammar/                      # Phase 3
│   │   ├── pages/
│   │   ├── components/
│   │   ├── api/
│   │   └── grammar.routes.tsx
│   │
│   ├── premium/                      # Phase 4
│   │   ├── pages/
│   │   ├── api/
│   │   └── premium.routes.tsx
│   │
│   └── [future-module]/              # Thêm module = thêm folder
│       ├── pages/
│       ├── components/
│       ├── api/
│       ├── store/
│       ├── types/
│       └── [module].routes.tsx
```

---

## Cách thêm module mới (Checklist)

```
Ví dụ: Thêm module "Shadowing" (luyện nghe-nói)

Backend (.NET):
  1. Tạo folder: JPLearn.Core/Shadowing/
  2. Tạo: Entities/, DTOs/, Service, Controller
  3. Register DbSet trong AppDbContext
  4. Thêm config trong Configurations/
  5. dotnet ef migrations add AddShadowing
  6. Register service trong ServiceCollectionExtensions

Frontend (React):
  1. Tạo folder: features/shadowing/
  2. Tạo: pages/, components/, api/, types/
  3. Tạo shadowing.routes.tsx
  4. Import routes vào Router.tsx
  5. Thêm link vào Sidebar

→ Không chạm vào code module khác!
```

---

## Router.tsx (Auto-compose routes)

```tsx
import { authRoutes } from '@/features/auth/auth.routes';
import { dashboardRoutes } from '@/features/dashboard/dashboard.routes';
import { vocabularyRoutes } from '@/features/vocabulary/vocabulary.routes';
import { reviewRoutes } from '@/features/review/review.routes';
import { kanjiRoutes } from '@/features/kanji/kanji.routes';
// Thêm module → thêm 1 dòng import

const router = createBrowserRouter([
  { path: '/login', ...authRoutes },
  {
    path: '/',
    element: <Layout />,
    children: [
      ...dashboardRoutes,
      ...vocabularyRoutes,
      ...reviewRoutes,
      ...kanjiRoutes,
      // ...shadowingRoutes,  ← thêm 1 dòng
    ]
  }
]);
```

---

## So sánh: cũ vs mới

| Tiêu chí | Group by Type | Group by Feature ✅ |
|----------|--------------|-------------------|
| Thêm module mới | Sửa 5-6 folders | Tạo 1 folder mới |
| Xóa module | Xóa files rải rác | Xóa 1 folder |
| Tìm code | Nhảy qua lại nhiều folders | Tất cả trong 1 folder |
| Conflict khi 2 người code | Cao (cùng sửa Controllers/) | Thấp (khác folder) |
| Scale 10+ modules | Hỗn loạn | Rõ ràng |
