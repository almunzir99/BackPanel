# Identity Migration Handoff (Current State + Next Steps)

## Scope Context
This handoff documents the **current implementation state** after the role/permission redesign push and outlines the **next concrete steps** to complete the migration to Microsoft Identity across backend and frontend.

---

## What is already implemented

### 1) Identity foundation added (backend)
- Identity models created:
  - [Infrastructure/BackPanel.Persistence/Identity/AppUser.cs](Infrastructure/BackPanel.Persistence/Identity/AppUser.cs)
  - [Infrastructure/BackPanel.Persistence/Identity/AppRole.cs](Infrastructure/BackPanel.Persistence/Identity/AppRole.cs)
- DbContext switched to Identity base context:
  - [Infrastructure/BackPanel.Persistence/Database/AppDbContext.cs](Infrastructure/BackPanel.Persistence/Database/AppDbContext.cs)
- Identity DI registration added:
  - [Presentation/BackPanel.WebApplication/Program.cs](Presentation/BackPanel.WebApplication/Program.cs)
- Identity EF package added:
  - [Infrastructure/BackPanel.Persistence/BackPanel.Persistence.csproj](Infrastructure/BackPanel.Persistence/BackPanel.Persistence.csproj)

### 2) Identity migration generated
- Migration files generated:
  - [Infrastructure/BackPanel.Persistence/Migrations/20260219213031_IdentityFoundation.cs](Infrastructure/BackPanel.Persistence/Migrations/20260219213031_IdentityFoundation.cs)
  - [Infrastructure/BackPanel.Persistence/Migrations/20260219213031_IdentityFoundation.Designer.cs](Infrastructure/BackPanel.Persistence/Migrations/20260219213031_IdentityFoundation.Designer.cs)
  - [Infrastructure/BackPanel.Persistence/Migrations/AppDbContextModelSnapshot.cs](Infrastructure/BackPanel.Persistence/Migrations/AppDbContextModelSnapshot.cs)

### 3) Roles module moved away from legacy Role CQRS
- Roles API now uses RoleManager directly:
  - [Presentation/BackPanel.WebApplication/Areas/API/Controllers/Common/RolesController.cs](Presentation/BackPanel.WebApplication/Areas/API/Controllers/Common/RolesController.cs)
- Legacy roles feature handlers deleted under:
  - [Core/BackPanel.Application/Features/Roles](Core/BackPanel.Application/Features/Roles)

### 4) Legacy permission coupling removed from active app flow
- DTO removed:
  - [Core/BackPanel.Application/DTOs/PermissionDto.cs](Core/BackPanel.Application/DTOs/PermissionDto.cs)
- Mapping cleanup:
  - [Core/BackPanel.Application/Mapping/MappingProfile.cs](Core/BackPanel.Application/Mapping/MappingProfile.cs)
- UnitOfWork cleanup:
  - [Core/BackPanel.Application/Interfaces/IUnitOfWork.cs](Core/BackPanel.Application/Interfaces/IUnitOfWork.cs)
  - [Infrastructure/BackPanel.Persistence/Repository/UnitOfWork.cs](Infrastructure/BackPanel.Persistence/Repository/UnitOfWork.cs)

### 5) Frontend switched to role-based checks (no nested permission object)
- Guard/menu/home updated:
  - [Presentation/BackPanel.WebApplication/ClientApp/src/app/core/guards/permission.guard.ts](Presentation/BackPanel.WebApplication/ClientApp/src/app/core/guards/permission.guard.ts)
  - [Presentation/BackPanel.WebApplication/ClientApp/src/app/dashboard/components/menu/menu.list.ts](Presentation/BackPanel.WebApplication/ClientApp/src/app/dashboard/components/menu/menu.list.ts)
  - [Presentation/BackPanel.WebApplication/ClientApp/src/app/dashboard/components/menu/menu.component.ts](Presentation/BackPanel.WebApplication/ClientApp/src/app/dashboard/components/menu/menu.component.ts)
  - [Presentation/BackPanel.WebApplication/ClientApp/src/app/dashboard/pages/home/home.component.ts](Presentation/BackPanel.WebApplication/ClientApp/src/app/dashboard/pages/home/home.component.ts)
  - [Presentation/BackPanel.WebApplication/ClientApp/src/app/dashboard/pages/home/home.component.html](Presentation/BackPanel.WebApplication/ClientApp/src/app/dashboard/pages/home/home.component.html)
- Role UI simplified:
  - [Presentation/BackPanel.WebApplication/ClientApp/src/app/dashboard/pages/roles/roles.component.ts](Presentation/BackPanel.WebApplication/ClientApp/src/app/dashboard/pages/roles/roles.component.ts)
  - [Presentation/BackPanel.WebApplication/ClientApp/src/app/dashboard/pages/roles/roles.component.html](Presentation/BackPanel.WebApplication/ClientApp/src/app/dashboard/pages/roles/roles.component.html)
- Permission model removed:
  - [Presentation/BackPanel.WebApplication/ClientApp/src/app/core/models/permission.model.ts](Presentation/BackPanel.WebApplication/ClientApp/src/app/core/models/permission.model.ts)
- Auth bootstrap/login adjusted to prefer `user.role` from payload:
  - [Presentation/BackPanel.WebApplication/ClientApp/src/app/app.component.ts](Presentation/BackPanel.WebApplication/ClientApp/src/app/app.component.ts)
  - [Presentation/BackPanel.WebApplication/ClientApp/src/app/public/authentication/authentication.component.ts](Presentation/BackPanel.WebApplication/ClientApp/src/app/public/authentication/authentication.component.ts)

### 6) Legacy file paths requested for removal were deleted and replaced
- Deleted paths:
  - [Core/BackPanel.Domain/Entities/UserEntityBase.cs](Core/BackPanel.Domain/Entities/UserEntityBase.cs)
  - [Core/BackPanel.Domain/Entities/Role.cs](Core/BackPanel.Domain/Entities/Role.cs)
  - [Core/BackPanel.Domain/Entities/Permission.cs](Core/BackPanel.Domain/Entities/Permission.cs)
- Replacements created under Identity folder (same type names kept for compatibility):
  - [Core/BackPanel.Domain/Entities/Identity/UserAccountBase.cs](Core/BackPanel.Domain/Entities/Identity/UserAccountBase.cs)
  - [Core/BackPanel.Domain/Entities/Identity/RoleDefinition.cs](Core/BackPanel.Domain/Entities/Identity/RoleDefinition.cs)
  - [Core/BackPanel.Domain/Entities/Identity/LegacyPermissionDefinition.cs](Core/BackPanel.Domain/Entities/Identity/LegacyPermissionDefinition.cs)

---

## Current known limitations / incomplete parts

1. **Admin account CQRS is still legacy-generic** (password hash/salt + generic account handlers) and not fully Identity-native yet.
   - Main folder: [Core/BackPanel.Application/Generic/Accounts](Core/BackPanel.Application/Generic/Accounts)

2. **Cross-layer architecture constraint**:
   - `Core.BackPanel.Application` currently cannot reference `Infrastructure` types (`AppRole`, `AppUser`).
   - Any direct `RoleManager<AppRole>` usage must be in Web layer, or introduce abstraction in Core.

3. **Roles controller export endpoints currently return not implemented** in Identity mode:
   - [Presentation/BackPanel.WebApplication/Areas/API/Controllers/Common/RolesController.cs](Presentation/BackPanel.WebApplication/Areas/API/Controllers/Common/RolesController.cs)

4. **Permission attribute still exists** (currently role-title based compatibility logic):
   - [Core/BackPanel.Application/Attributes/Permissions/PermissionsAttribute.cs](Core/BackPanel.Application/Attributes/Permissions/PermissionsAttribute.cs)

5. Full web project build may fail intermittently due to output file lock by running process; Core/Persistence builds pass.

---

## Build status snapshot

- Backend core projects compile:
  - `dotnet build Core/BackPanel.Application/BackPanel.Application.csproj` ✅
  - `dotnet build Infrastructure/BackPanel.Persistence/BackPanel.Persistence.csproj` ✅
- Frontend compile:
  - `npm run build` ✅ (warnings only)
- Full web project build can fail if `BackPanel.WebApplication` dlls are locked by running process.

---

## Next steps (ordered, high priority)

## Phase A — Finish backend Identity cutover for Admin accounts

1. Replace generic account command/query flow for Admin with Identity-backed services.
   - Replace usage from:
     - [Core/BackPanel.Application/Generic/Accounts](Core/BackPanel.Application/Generic/Accounts)
   - Start at web API entry:
     - [Presentation/BackPanel.WebApplication/Areas/API/Controllers/Accounts/AccountBaseController.cs](Presentation/BackPanel.WebApplication/Areas/API/Controllers/Accounts/AccountBaseController.cs)
     - [Presentation/BackPanel.WebApplication/Areas/API/Controllers/Accounts/AdminAccountController.cs](Presentation/BackPanel.WebApplication/Areas/API/Controllers/Accounts/AdminAccountController.cs)

2. Introduce application abstractions in Core (interfaces) for identity operations, implemented in Infrastructure/Web.
   - Keep Core decoupled from `AppRole`/`AppUser` concrete classes.
   - Suggested target: `IIdentityUserService`, `IIdentityRoleService` in Core interfaces.

3. Migrate login/profile/password endpoints to Identity password/token APIs.
   - Remove dependency on `PasswordHash`/`PasswordSalt` fields in active logic.
   - Review handlers under:
     - [Core/BackPanel.Application/Features/Admins/Commands/Handlers](Core/BackPanel.Application/Features/Admins/Commands/Handlers)
     - [Core/BackPanel.Application/Features/Admins/Queries/Handlers](Core/BackPanel.Application/Features/Admins/Queries/Handlers)

4. Ensure `AdminDto.Role` is always populated via Identity role resolution in API response layer.
   - Required for frontend menu/guard behavior.

## Phase B — Remove remaining legacy permission/role mechanics

5. Remove `PermissionTitle` and custom permission attribute dependency from controllers.
   - Replace with explicit role-based policies/authorize attributes.
   - Touch:
     - [Presentation/BackPanel.WebApplication/Areas/API/Controllers/Common/ApiController.cs](Presentation/BackPanel.WebApplication/Areas/API/Controllers/Common/ApiController.cs)
     - [Core/BackPanel.Application/Attributes/Permissions/PermissionsAttribute.cs](Core/BackPanel.Application/Attributes/Permissions/PermissionsAttribute.cs)

6. Remove remaining obsolete role query/command registrations/usages (if any).
   - Re-scan for references to deleted files and generic role pipelines.

## Phase C — Data migration completion

7. Extend Identity migration with one-time data copy from legacy tables.
   - In [Infrastructure/BackPanel.Persistence/Migrations/20260219213031_IdentityFoundation.cs](Infrastructure/BackPanel.Persistence/Migrations/20260219213031_IdentityFoundation.cs):
     - Copy `Admins` → `AspNetUsers`
     - Copy `Roles` → `AspNetRoles`
     - Copy role assignment `Admin.RoleId` → `AspNetUserRoles`
     - Map manager users to manager/super role strategy

## Phase D — Frontend final alignment

8. After backend account cutover, update services and models only if response contracts change.
   - Verify these files:
     - [Presentation/BackPanel.WebApplication/ClientApp/src/app/core/services/account.service.ts](Presentation/BackPanel.WebApplication/ClientApp/src/app/core/services/account.service.ts)
     - [Presentation/BackPanel.WebApplication/ClientApp/src/app/core/models/admin.model.ts](Presentation/BackPanel.WebApplication/ClientApp/src/app/core/models/admin.model.ts)
     - [Presentation/BackPanel.WebApplication/ClientApp/src/app/dashboard/pages/admins/admins.component.ts](Presentation/BackPanel.WebApplication/ClientApp/src/app/dashboard/pages/admins/admins.component.ts)
     - [Presentation/BackPanel.WebApplication/ClientApp/src/app/public/authentication/authentication.component.ts](Presentation/BackPanel.WebApplication/ClientApp/src/app/public/authentication/authentication.component.ts)

---

## Suggested first command sequence in new window

1. Stop running web process that locks output DLLs.
2. Run:
   - `dotnet build Core/BackPanel.Application/BackPanel.Application.csproj`
   - `dotnet build Infrastructure/BackPanel.Persistence/BackPanel.Persistence.csproj`
   - `dotnet build Presentation/BackPanel.WebApplication/BackPanel.WebApplication.csproj`
3. Run frontend build:
   - `cd Presentation/BackPanel.WebApplication/ClientApp`
   - `npm run build`

---

## Key rule for continuation
Do not reference `BackPanel.Persistence.Identity.AppRole/AppUser` directly in Core Application project. Use abstraction or move Identity concrete orchestration to Web/Infrastructure layers.
