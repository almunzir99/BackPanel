/** Leaf — one permission action (e.g. "View"). `value` is the full dot-notation string stored in the DB. */
export interface PermissionAction {
  value: string;   // "Administration.Admins.View"
  label: string;   // "View"
  checked?: boolean;
}

/** Mid-level — a module within a section (e.g. "Admins"). */
export interface PermissionModule {
  key: string;     // "Admins"
  label: string;
  actions: PermissionAction[];
  expanded?: boolean;
  allChecked?: boolean;
  someChecked?: boolean;
}

/** Top-level — a section (e.g. "Administration"). */
export interface PermissionSection {
  key: string;     // "Administration"
  label: string;
  modules: PermissionModule[];
  expanded?: boolean;
  allChecked?: boolean;
  someChecked?: boolean;
}
