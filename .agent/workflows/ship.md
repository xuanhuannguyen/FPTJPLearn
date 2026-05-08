---
description: Structured Git management for solo developers. Ensures clean history and safe backups.
---

# /ship - Solo Developer Git Workflow

$ARGUMENTS

---

## Purpose

This command automates the "Save Point" process for your project. It ensures that your code is committed with meaningful messages and pushed to the remote repository safely.

---

## Behavior

When `/ship` is triggered:

1. **Scan Changes**
   - Check which files were modified.
   - Categorize changes (Frontend, Backend, Design, Infrastructure).

2. **Atomic Grouping**
   - If changes span multiple domains, suggest splitting them into separate commits.
   - Ask: "Should I commit all of this together or split by module?"

3. **Message Generation**
   - Generate a [Conventional Commits](https://www.conventionalcommits.org/) style message.
   - Format: `type(scope): description`
   - Examples: `feat(grammar): add reset functionality`, `fix(ui): left-align card badges`.

4. **Execution**
   - Stage files (`git add`).
   - Commit with the generated message.
   - Push to current branch.

---

## Usage Examples

```
/ship redid the grammar practice UI
/ship added memory srs service
/ship fixed navigation bug on detail page
```

---

## Output Format

```markdown
### 🚀 Shipping Changes

**Summary of work:**
- [List of major changes]

**Commit Details:**
- Message: `feat(module): description`
- Files affected: `X` files

**Status:**
- [ ] Staging
- [ ] Committing
- [ ] Pushing to GitHub

*Done! Your progress is safe.*
```
