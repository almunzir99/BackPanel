/**
 * A single node in the permission tree — works at any depth.
 * - Branch nodes: have `children`, no `value`.
 * - Leaf nodes:   have `value` (the claim string to persist), no `children`.
 */
export interface PermNode {
  key: string;
  label: string;
  value?: string;        // leaf only — full dot string, e.g. "Administration.CompanyInfo.Policies.Edit"
  checked?: boolean;     // leaf only
  children?: PermNode[]; // branch only
  expanded?: boolean;
  allChecked?: boolean;
  someChecked?: boolean;
}
