# JPLearn — Progress Log

## ✅ Đã hoàn thành

### Bước 1: .NET Solution Setup
- Solution `JPLearn.sln` với 3 projects: Api → Core + Infrastructure, Infrastructure → Core
- NuGet: Npgsql, EF Core (Tools, Design), Swashbuckle, EF Core (Core project)

### Bước 2: Entities (JPLearn.Core)
- `Common/Entities/BaseEntity.cs` — Id, CreatedAt, UpdatedAt
- `Vocabulary/Entities/VocabularyList.cs` — UserId, Name, Description, WordCount
- `Vocabulary/Entities/VocabularyItem.cs` — Word, Reading, WordType, Meaning, Example
- `Vocabulary/Entities/UserWordProgress.cs` — SRS: Repetitions, EaseFactor, IntervalDays, NextReviewAt
- `Review/Entities/ReviewSession.cs` — Mode, TotalCards, CorrectCount, WrongCount

### Bước 3: DTOs (JPLearn.Core)
- `Vocabulary/DTOs/ImportVocabularyDto.cs` — Import request with validation
- `Vocabulary/DTOs/VocabularyListDto.cs` — Response: List, ListDetail, Item
- `Review/DTOs/ReviewDtos.cs` — ReviewCard, ReviewAnswer, SaveSession, DueCardsResponse

### Bước 4: Interfaces (JPLearn.Core)
- `Vocabulary/IVocabularyService.cs` — Import, GetLists, GetById, Update, Delete
- `Review/IReviewService.cs` — GetDueCards, GetAllCards, SubmitAnswer, SaveSession

### Bước 5: Database (JPLearn.Infrastructure)
- `Data/AppDbContext.cs` — 4 DbSets, ApplyConfigurationsFromAssembly
- `Data/Configurations/VocabularyConfigurations.cs` — snake_case tables, indexes, cascade delete
- Neon PostgreSQL cloud connected
- Migration `InitVocabularyModule` applied ✅

### Bước 6: Services (JPLearn.Infrastructure/Services)
- `VocabularyService.cs` — Import JSON, CRUD, per-user isolation (WHERE user_id)
- `ReviewService.cs` — SRS due cards, submit answer with SM-2, session logging

### Bước 7: SRS Algorithm (JPLearn.Core)
- `Common/Services/SrsAlgorithm.cs` — SM-2 implementation (Quên=1, Khó=3, Nhớ=5)

### Bước 8: Controllers (JPLearn.Api)
- `Controllers/VocabularyController.cs` — 6 endpoints (import, getAll, getById, update, deleteList, deleteItem)
- `Controllers/ReviewController.cs` — 4 endpoints (getDue, getAll, answer, session)
- DevUserId hardcoded (chưa có Auth)

### Bước 9: Program.cs
- PostgreSQL + CORS (localhost:5173) + Swagger + DI registration
- Server chạy tại `http://localhost:5000`
- Swagger UI xác nhận 10 endpoints OK ✅

---

## ⏳ Chưa làm

### Backend
- [x] Initial Solution Setup (API, Core, Infrastructure)
- [x] Database Configuration (Neon PostgreSQL, EF Core)
- [x] Dependency Injection & Circular Reference Fixes
- [x] SRS Algorithm Logic (SM-2 base)
- [x] Vocabulary Service (Import, CRUD)
- [x] Review Service (SRS scheduling, Session recording)
- [x] RESTful Endpoints (VocabularyController, ReviewController)
- [x] Swagger UI & CORS Configuration

## Frontend Phase
- [x] Vite React + TypeScript Setup
- [x] Tailwind CSS + Lucide Icons Integration
- [x] Pro Max Light Theme Implementation (Claymorphism, Nunito Sans)
- [x] Layout Components (Sidebar, Navbar)
- [x] Vocabulary List Page (Grid UI inspired by NhaiKanji)
- [x] Import JSON Modal (with validation error parsing)
- [x] API Integration (Axios connecting to .NET for fetching & importing lists)

## Next Steps
- [ ] Vocabulary List Detail Page (View words inside a list)
- [ ] Review Session / Flashcard UI (Flipping cards, SM-2 scoring buttons)
- [ ] User Authentication (JWT, ASP.NET Identity)
- [ ] Grammar module
- [ ] Premium module

---

## 📝 Ghi chú kỹ thuật

- **Dependency flow:** Api → Core + Infrastructure, Infrastructure → Core (KHÔNG có Core → Infrastructure)
- **Services nằm ở Infrastructure** (vì cần access AppDbContext)
- **Interfaces nằm ở Core** (clean architecture)
- **DevUserId:** `00000000-0000-0000-0000-000000000001` — thay bằng JWT userId khi có Auth
- **Neon connection:** Host=ep-rapid-river-anfv6vah.c-6.us-east-1.aws.neon.tech (nhớ đổi password sau)
