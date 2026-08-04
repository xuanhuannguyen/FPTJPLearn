# Security & Licensing Phase Plan

## Goal

Prepare JPLearn for paid operation with two production-critical controls:

- One active device per account. A new login invalidates the older device session.
- Central licensing mode. The site runs in either half licensing or full licensing. Paid users can access all content for their active package.

## Current State

- Payments already support SePay and PayOS.
- `AppUser.ActiveDeviceToken` already exists.
- `/api/auth/sync` already records the active device token.
- `FirebaseAuthMiddleware` already blocks stale device tokens with `ACCOUNT_LOGGED_IN_ELSEWHERE`.
- `PaymentAccessService` supports runtime access policy through `Access:PolicyMode`.
- Active vocabulary already uses free/premium quota behavior.
- Frontend access now reads backend truth through `/api/access/me`.

## Access Policy

### Half Licensing

- Vocabulary, grammar, and kanji are free for JPD113 and JPD123.
- Speaking:
  - JPD113 reading lessons 1-10 are free.
  - JPD113 Q&A lesson 1 is free.
  - JPD123 reading lessons 1-10 are free.
  - JPD123 Q&A lesson 4 is free.
- Exam practice:
  - Free users can access the first half of questions in each topic, ordered by `OrderIndex`.
  - Paid users can access all questions for their active package.
- Active vocabulary import limit behaves as premium: 10 imports per day.
- Orders/pricing remain enabled so users can buy a package to unlock everything.

### Full Licensing

- Free users:
  - JPD113 vocabulary, grammar, kanji: lesson 1 free.
  - JPD123 vocabulary, grammar, kanji: lesson 4 free.
  - Speaking locked.
  - Exam practice locked.
  - Active vocabulary import limit: 2 total lists.
- Paid users:
  - `jpd113` unlocks premium JPD113 content.
  - `jpd123` unlocks premium JPD123 content.
  - Combo unlocks both through two subscriptions.
  - Active vocabulary import limit: 10 imports per day.

## Implementation Phases

### Phase 1: Single-Device Session

- Keep `/api/auth/sync` as the source of truth for current device.
- Add client interceptor handling for `403 ACCOUNT_LOGGED_IN_ELSEWHERE`.
- On stale device:
  - Sign out from Firebase.
  - Clear local user state.
  - Redirect to login.
  - Show a short local notification message.
- `/api/auth/logout` clears the active device token when the current device logs out cleanly.

### Phase 2: Public Access Status API

- Add `GET /api/access/me`.
- Response shape:
  - `accessPolicyMode`
  - `licensingEnabled`
  - `freeExperienceEnabled`
  - `activeCourseCodes`
  - `subscriptions`
- This endpoint allows frontend locks, pricing, and navigation to use backend truth instead of environment-only flags.

### Phase 3: Frontend Access Hook

- Replace hard-coded `useUserAccess` behavior.
- Fetch `/api/access/me`.
- Lock only when:
  - licensing is enabled,
  - content is premium,
  - user does not have the mapped course subscription.
- Map package codes such as `vocab_jpd113`, `grammar_jpd123`, `speaking_jpd113` to `jpd113` or `jpd123`.

### Phase 4: Lesson Policy Audit

- Verify seeded/imported `accessTier` and `packageCode`:
  - JPD113: lesson 1 free, lessons 2-3 premium.
  - JPD123: lesson 4 free, lessons 5-7 premium.
- Kanji seed already follows this pattern.
- `tools/audit-access-policy.mjs` audits vocabulary, grammar, kanji, speaking, and exam policy.

### Phase 5: Admin Operation

- Keep existing manual subscription controls.
- Keep existing reset-device action.
- Add admin licensing setting screen:
  - `GET /api/admin/access-settings`
  - `PUT /api/admin/access-settings`
  - Admin route: `/jplearn-manage-xh21/access-settings`
- Runtime setting is stored in DB table `AppSettings`.
- If DB setting is absent, backend falls back to appsettings:
  - `Access:PolicyMode = half`
- Operating rule:
  - Half licensing = `accessPolicyMode=half`
  - Full licensing = `accessPolicyMode=full`

### Phase 6: Launch Checklist

- Login account A on device 1.
- Login same account A on device 2.
- Device 1 receives stale-session logout on next API call.
- With half licensing on, free user sees the half-access policy.
- With full licensing on, free user only sees configured free lessons.
- JPD113 subscriber opens JPD113 premium lessons but not JPD123.
- JPD123 subscriber opens JPD123 premium lessons but not JPD113.
- Paid user active vocabulary quota is 10 per day.
- Free user active vocabulary quota is 2 total lists.
- Successful SePay/PayOS order creates expected subscription.

## Completion Status

- Phase 1 single-device session: completed.
- Phase 2 public access API: completed.
- Phase 3 frontend access hook: completed.
- Phase 4 lesson policy audit: completed.
- Phase 5 admin operation: completed.
- Phase 6 build/test checklist: completed with automated checks below.

## Verification Commands

Run these before deploy:

```bash
dotnet build server/JPLearn.slnx
npm --prefix client run build
node tools/audit-access-policy.mjs
```

Run this after deploy database target is configured:

```bash
dotnet ef database update --project server/JPLearn.Infrastructure --startup-project server/JPLearn.Api
```

## Operating Notes

- Runtime licensing is controlled from `/jplearn-manage-xh21/access-settings`.
- Backend setting key is `Access:PolicyMode`.
- If DB setting is missing, backend falls back to `appsettings`; legacy `Payments:FreeExperienceEnabled=true` maps to half licensing and `false` maps to full licensing.
- Frontend no longer uses `VITE_FREE_EXPERIENCE_ENABLED`; backend/admin setting is the source of truth.
