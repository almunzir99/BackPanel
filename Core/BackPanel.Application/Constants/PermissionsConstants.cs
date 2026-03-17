namespace BackPanel.Application.Constants;

/// <summary>
/// Single source of truth for all permission values in the system.
/// Format: "Section.Module.Action"
///
/// Adding a new permission requires only adding a new constant here.
/// The roles controller builds the nested tree from these values automatically at runtime via reflection.
/// </summary>
public static class PermissionsConstants
{
    // ── Administration > Admins ─────────────────────────────────────────────
    public static readonly string ViewAdmins           = "Administration.Admins.View";
    public static readonly string AddAdmins            = "Administration.Admins.Add";
    public static readonly string EditAdmins           = "Administration.Admins.Edit";
    public static readonly string DeleteAdmins         = "Administration.Admins.Delete";
    public static readonly string ChangePasswordAdmins = "Administration.Admins.ChangePassword";

    // ── Administration > Roles ──────────────────────────────────────────────
    public static readonly string ViewRoles   = "Administration.Roles.View";
    public static readonly string AddRoles    = "Administration.Roles.Add";
    public static readonly string EditRoles   = "Administration.Roles.Edit";
    public static readonly string DeleteRoles = "Administration.Roles.Delete";

    // ── Administration > Messages ───────────────────────────────────────────
    public static readonly string ViewMessages   = "Administration.Messages.View";
    public static readonly string DeleteMessages = "Administration.Messages.Delete";

    // ── Administration > Business ────────────────────────────────────────
    public static readonly string ViewBusiness = "Administration.Business.View";
    public static readonly string EditBusiness = "Administration.Business.Edit";
    public static readonly string AjustBusinessPolicies = "Administration.Business.Policies.Edit";

}
