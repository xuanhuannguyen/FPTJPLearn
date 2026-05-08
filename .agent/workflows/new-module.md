---
description: Create a new feature module following the JPLearn Fullstack Architecture.
---

# /new-module - Fullstack Feature Scaffolding

$ARGUMENTS

---

## Purpose

This command automates the creation of a new feature module. It ensures the folder structure, API setup, and routing follow the established "Grammar Module" pattern.

---

## Behavior

When `/new-module` is triggered:

1. **Information Gathering**
   - Ask for the module name (e.g., `Vocabulary`, `Kanji`).
   - Confirm if it needs both Frontend and Backend or just one side.

2. **Frontend Scaffolding**
   - Create directory `client/src/features/{module}/`
   - Create sub-folders: `api/`, `components/`, `pages/`, `types/`, `utils/`.
   - Create a base `{module}.routes.tsx` file.

3. **Backend Scaffolding**
   - Create `JPLearn.Application/Features/{Module}/` (Commands/Queries).
   - Create domain entity placeholder in `JPLearn.Domain`.
   - Update `JPLearn.Infrastructure` for DbContext and Migrations if needed.

4. **Integration**
   - Register the new routes in the main `AppRoutes.tsx`.
   - Suggest a "Plan" file for implementation details.

---

## Output Format

```markdown
### 🏗️ Scaffolding Module: [Name]

**Structure Created:**
- [ ] `client/src/features/[name]/`
- [ ] `JPLearn.Application/Features/[Name]/`

**Next Steps:**
1. Define the Domain Model in `Domain`.
2. Implement the first API endpoint in `Application`.
3. Create the main Page component in `Frontend`.

*Ready to start building! Use `/plan` to detail the implementation.*
```

---

## Examples

```
/new-module Vocabulary
/new-module Kanji with SRS support
```
