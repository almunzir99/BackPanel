import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { firstValueFrom } from 'rxjs';
import { Direction } from '@angular/cdk/bidi';
import { TranslateService } from '@ngx-translate/core';
import { RequestStatus } from 'src/app/core/models/request-status.enum';
import { PermissionSection, PermissionModule } from 'src/app/core/models/permission-group.model';
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

  // ── Permissions tree state (3-level: Section > Module > Action) ───────────
  permissionSections: PermissionSection[] = [];
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

  /********************************* 3-Level Permissions Tree ******************************************** */

  async loadPermissionSections(existingClaims: PermissionClaim[] = []) {
    this.permissionsLoading = true;
    try {
      const result = await firstValueFrom(this._service.getAvailablePermissions());
      const existingValues = new Set(existingClaims.map(c => c.claimValue));

      this.permissionSections = (result.data ?? []).map(section => ({
        ...section,
        expanded: true,
        modules: section.modules.map(mod => ({
          ...mod,
          expanded: true,
          actions: mod.actions.map(action => ({
            ...action,
            checked: existingValues.has(action.value)
          }))
        }))
      }));

      this.permissionSections.forEach(s => this.recalcSection(s));
    } catch (_) {
      this.permissionSections = [];
    } finally {
      this.permissionsLoading = false;
    }
  }

  // ── Section-level toggle ──────────────────────────────────────────────────

  toggleSectionExpanded(section: PermissionSection) {
    section.expanded = !section.expanded;
  }

  onSectionCheckChange(section: PermissionSection, checked: boolean) {
    section.modules.forEach(mod => {
      mod.actions.forEach(a => a.checked = checked);
      this.recalcModule(mod);
    });
    this.recalcSection(section);
  }

  // ── Module-level toggle ───────────────────────────────────────────────────

  toggleModuleExpanded(mod: PermissionModule) {
    mod.expanded = !mod.expanded;
  }

  onModuleCheckChange(section: PermissionSection, mod: PermissionModule, checked: boolean) {
    mod.actions.forEach(a => a.checked = checked);
    this.recalcModule(mod);
    this.recalcSection(section);
  }

  // ── Action-level toggle ───────────────────────────────────────────────────

  onActionCheckChange(section: PermissionSection, mod: PermissionModule) {
    this.recalcModule(mod);
    this.recalcSection(section);
  }

  // ── State recalculation ───────────────────────────────────────────────────

  private recalcModule(mod: PermissionModule) {
    const checked = mod.actions.filter(a => a.checked).length;
    mod.allChecked  = checked === mod.actions.length;
    mod.someChecked = checked > 0 && checked < mod.actions.length;
  }

  private recalcSection(section: PermissionSection) {
    const totalActions  = section.modules.reduce((n, m) => n + m.actions.length, 0);
    const checkedActions = section.modules.reduce((n, m) => n + m.actions.filter(a => a.checked).length, 0);
    section.allChecked  = checkedActions === totalActions && totalActions > 0;
    section.someChecked = checkedActions > 0 && checkedActions < totalActions;
  }

  // ── Collect checked permissions ───────────────────────────────────────────

  private collectPermissions(): PermissionClaim[] {
    const claims: PermissionClaim[] = [];
    for (const section of this.permissionSections) {
      for (const mod of section.modules) {
        for (const action of mod.actions) {
          if (action.checked) {
            claims.push({ claimType: 'Permission', claimValue: action.value });
          }
        }
      }
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
    await this.loadPermissionSections(this.role.permissions ?? []);
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

