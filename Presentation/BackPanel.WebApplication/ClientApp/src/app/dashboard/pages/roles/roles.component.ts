import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { firstValueFrom } from 'rxjs';
import { Direction } from '@angular/cdk/bidi';
import { TranslateService } from '@ngx-translate/core';
import { RequestStatus } from 'src/app/core/models/request-status.enum';
import { PermNode } from 'src/app/core/models/permission-group.model';
import { PermissionClaim } from 'src/app/core/models/permission-claim.model';
import { Role } from 'src/app/core/models/role.model';
import { GeneralService } from 'src/app/core/services/general.service';
import { RolesService } from 'src/app/core/services/roles.service';
import { AlertMessage, AlertMessageComponent, MessageTypes } from 'src/app/shared/components/alert-message/alert-message.component';
import { Column } from 'src/app/shared/components/datatable/column.model';
import { PageSpec, SortSpec } from 'src/app/shared/components/datatable/datatable.component';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  standalone: false,
  styleUrls: ['./roles.component.scss']
})
export class RolesComponent implements OnInit {
  columns: Column[] = [];
  data: Role[] = [];
  pageIndex = 1;
  pageSize = 10;
  totalRecords = 0;
  totalPages = 1;
  orderBy = "lastUpdate";
  ascending = false;
  searchValue = "";
  getRequest = RequestStatus.Initial;
  dimRequest = RequestStatus.Initial;
  theme: 'light' | 'dark' = 'light';
  @ViewChild("roleForm") roleForm?: TemplateRef<any>;
  role: any;

  // ── Permissions tree (unlimited depth) ──────────────────────────────────
  permTree: PermNode[] = [];
  permissionsLoading = false;

  // ── Layout direction (RTL / LTR) ──────────────────────────────────────────
  dir: Direction = 'rtl';

  constructor(
    private _service: RolesService,
    private _dialog: MatDialog,
    _generalService: GeneralService,
    private _translate: TranslateService
  ) {
    _generalService.$theme.subscribe(value => this.theme = value);
    this.dir = _translate.currentLang === 'ar' ? 'rtl' : 'ltr';
    _translate.onLangChange.subscribe(event => {
      this.dir = event.lang === 'ar' ? 'rtl' : 'ltr';
    });
  }

  ngOnInit(): void {
    this.initRole();
    this.initColumns();
    this.getData();
  }

  async getData() {
    try {
      this.getRequest = RequestStatus.Loading;
      const result = await firstValueFrom(this._service.get(this.pageIndex, this.pageSize, this.searchValue, this.orderBy, this.ascending));
      this.data = result.data;
      this.totalPages = result.totalPages;
      this.totalRecords = result.totalRecords;
      this.getRequest = RequestStatus.Success;
    } catch (error) {
      this.getRequest = RequestStatus.Failed;
    }
  }

  initRole() {
    this.role = { id: 0, title: '', permissions: [] };
  }

  initColumns() {
    this.columns = [
      { prop: "id",         title: "#",          show: true, sortable: true  },
      { prop: "title",      title: "Title",       show: true, sortable: true  },
      { prop: "createdAt",  title: "CreatedAt",   show: true, sortable: true  },
      { prop: "lastUpdate", title: "LastUpdate",  show: true, sortable: true  },
      { prop: "Actions",    title: "Actions",     show: true, sortable: false }
    ];
  }

  /********************************* Permissions Tree (unlimited depth) ******************************* */

  async loadPermTree(existingClaims: PermissionClaim[] = []) {
    this.permissionsLoading = true;
    try {
      const result = await firstValueFrom(this._service.getAvailablePermissions());
      const existing = new Set(existingClaims.map(c => c.claimValue));
      this.permTree = this.buildTree(result.data ?? [], existing);
    } catch (_) {
      this.permTree = [];
    } finally {
      this.permissionsLoading = false;
    }
  }

  private buildTree(values: string[], existing: Set<string>): PermNode[] {
    const roots: PermNode[] = [];
    for (const value of values) {
      const parts = value.split('.');
      let level = roots;
      for (let i = 0; i < parts.length; i++) {
        const key = parts[i];
        const isLeaf = i === parts.length - 1;
        if (isLeaf) {
          level.push({ key, label: key, value, checked: existing.has(value) });
        } else {
          let branch = level.find(n => n.key === key && !!n.children);
          if (!branch) {
            branch = { key, label: key, children: [], expanded: true };
            level.push(branch);
          }
          level = branch.children!;
        }
      }
    }
    this.permTree = roots;
    this.recalcTree();
    return roots;
  }

  // ── Called from HTML when a leaf checkbox changes ─────────────────────────
  onLeafChange() { this.recalcTree(); }

  // ── Called from HTML when a branch checkbox toggles all its subtree ───────
  setBranchChecked(node: PermNode, checked: boolean) {
    this.setAllLeaves(node, checked);
    this.recalcTree();
  }

  // ── Recursively set every leaf under node ─────────────────────────────────
  private setAllLeaves(node: PermNode, checked: boolean) {
    if (!node.children) { node.checked = checked; return; }
    node.children.forEach(c => this.setAllLeaves(c, checked));
  }

  // ── Recalc allChecked / someChecked for every branch, bottom-up ───────────
  private recalcTree() {
    this.permTree.forEach(n => this.recalcNode(n));
  }

  private recalcNode(node: PermNode): { total: number; checked: number } {
    if (!node.children) return { total: 1, checked: node.checked ? 1 : 0 };
    let total = 0, checked = 0;
    for (const child of node.children) {
      const r = this.recalcNode(child);
      total += r.total; checked += r.checked;
    }
    node.allChecked  = total > 0 && checked === total;
    node.someChecked = checked > 0 && checked < total;
    return { total, checked };
  }

  // ── Walk every leaf and collect checked ones ──────────────────────────────
  private collectPermissions(): PermissionClaim[] {
    return this.gatherClaims(this.permTree);
  }

  private gatherClaims(nodes: PermNode[]): PermissionClaim[] {
    const claims: PermissionClaim[] = [];
    for (const node of nodes) {
      if (!node.children && node.checked && node.value)
        claims.push({ claimType: 'Permission', claimValue: node.value });
      else if (node.children)
        claims.push(...this.gatherClaims(node.children));
    }
    return claims;
  }

  /********************************* Event Binding ******************************************** */

  onPageChange(event: PageSpec) {
    this.pageIndex = event.pageIndex!;
    this.pageSize = event.pageSize!;
    this.getData();
  }
  onSortChange(event: SortSpec) {
    this.orderBy = event.prop!;
    this.ascending = event.ascending;
    this.getData();
  }
  onSearch(value: string) {
    this.searchValue = value;
    this.getData();
  }
  onCreate() { this.openForm(); }
  onUpdate(item: Role) { this.openForm(item); }

  onDeleteClick(id: number) {
    this._dialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
      data: { type: MessageTypes.CONFIRM, message: "Are Sure you want to Delete this Item ?", title: "Confirm" }
    }).afterClosed().subscribe({ next: res => { if (res == true) this.delete(id); } });
  }

  onFormSubmit() {
    this.role.permissions = this.collectPermissions();
    this._dialog.closeAll();
    if (this.role['id'] == 0) this.create(this.role);
    else this.update(this.role);
  }

  closeDialog = () => this._dialog.closeAll();

  onExportClick(type: string) {
    this.dimRequest = RequestStatus.Loading;
    this._service.export(type, () => { this.dimRequest = RequestStatus.Success; }, (err) => { this.dimRequest = RequestStatus.Failed; });
  }

  /********************************* Form Configuration ******************************************** */

  async openForm(item?: Role) {
    if (item) {
      try {
        const result = await firstValueFrom(this._service.single(item.id!));
        this.role = result.data;
      } catch (_) {
        this.role = { ...item };
      }
    } else {
      this.initRole();
    }
    await this.loadPermTree(this.role.permissions ?? []);
    this._dialog.open(this.roleForm!, { width: '560px', maxHeight: '90vh', direction: this.dir });
  }

  /********************************* Api Integration ******************************************** */

  create = async (item: Role) => {
    try {
      this.dimRequest = RequestStatus.Loading;
      await firstValueFrom(this._service.post(item));
      this.dimRequest = RequestStatus.Success;
      this._dialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
        data: { type: MessageTypes.SUCCESS, message: "Item Created Successfully", title: "Success" }
      }).afterClosed().subscribe(_ => this._dialog.closeAll());
      this.getData();
    } catch (error) { this.dimRequest = RequestStatus.Failed; }
  }

  update = async (item: Role) => {
    try {
      this.dimRequest = RequestStatus.Loading;
      await firstValueFrom(this._service.put(item));
      this.dimRequest = RequestStatus.Success;
      this._dialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
        data: { type: MessageTypes.SUCCESS, message: "Item Update Successfully", title: "Success" }
      }).afterClosed().subscribe(_ => this._dialog.closeAll());
      this.getData();
    } catch (error) { this.dimRequest = RequestStatus.Failed; }
  }

  delete = async (id: number) => {
    try {
      this.dimRequest = RequestStatus.Loading;
      await firstValueFrom(this._service.delete(id));
      this.dimRequest = RequestStatus.Success;
      this._dialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
        data: { type: MessageTypes.SUCCESS, message: "Item Deleted Successfully", title: "Success" }
      }).afterClosed().subscribe(_ => this._dialog.closeAll());
      this.getData();
    } catch (error) { this.dimRequest = RequestStatus.Failed; }
  }
}

