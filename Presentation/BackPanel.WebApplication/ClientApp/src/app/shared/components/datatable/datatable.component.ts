import { Component, EventEmitter, Input, OnInit, Output, TemplateRef } from '@angular/core';
import { GeneralService } from 'src/app/core/services/general.service';
import { Column } from './column.model';

@Component({
  selector: 'data-table',
  templateUrl: './datatable.component.html',
  styleUrls: ['./datatable.component.scss']
})
export class DatatableComponent implements OnInit {
  @Input('title') title: string = 'Data Table';
  @Input("columns") columns: Column[] = [];
  @Input("rows") rows: any[] = [];
  @Input("cell-template") cellTemplate?: TemplateRef<any>;
  @Input('current-page') currentPage: number = 1;
  @Input('page-size') PageSize: number = 10;
  @Input('total') total: number = 0;
  @Input('loading') loading = false;
  @Input('show-header') showHeader = true;
  @Input('show-footer') showFooter = true;
  @Input('show-header-title') showHeaderTitle = true;
  @Input('show-controls') showControls = true;
  @Input('show-create-button') showCreate = true;
  @Input('show-search') showSearch = true;
  @Input('show-export-button') showExport = true;
  @Input('break-word') breakWords = true;
  @Input('fixed') fixed = false;
  @Input("controls-template") controlTemplate?: TemplateRef<any>;
  @Output('pageChange') pageChangeEmitter = new EventEmitter<PageSpec>();
  @Output('sortChange') sortChangeEmitter = new EventEmitter<SortSpec>();
  @Output('searchChange') searchChangeEmitter = new EventEmitter<string>();
  @Output('createClick') createClickEmitter = new EventEmitter();
  @Output('exportClick') exportClickEmitter = new EventEmitter<string>();
  theme:'light' | 'dark' = 'light';

  sortProp = "";
  ascending = false;
  constructor(_generalService:GeneralService) {
    _generalService.$theme.subscribe(value => this.theme = value);
   }
  ngOnInit(): void {
    this.sortProp = this.columns[0].prop;
  }
  ngAfterViewInit() {
    this.configureColumnsResizer();
  }
  /******************* Configure Events Binding ****************** */
  onSortChange(prop: string, sortable: boolean) {
    if (!sortable)
      return;
    if (prop == this.sortProp) {
      this.ascending = !this.ascending;
    }
    this.sortProp = prop;
    this.sortChangeEmitter.emit({ prop: this.sortProp, ascending: this.ascending });
  }
  onSearchChange(value: any) {
    this.searchChangeEmitter.emit(value.target.value);
  }
  onCreateClick() {
    this.createClickEmitter.emit();
  }
  onPageChange(event: PageSpec) {
    if(event.pageIndex)
    event.pageIndex = event.pageIndex + 1;
    this.pageChangeEmitter.emit(event);
  }
  onExportClick(type: string) {
    this.exportClickEmitter.emit(type);

  }

  /******************* Configure Table Resizer ****************** */
  configureColumnsResizer() {
    var resizers = document.querySelectorAll(".resizer") as NodeListOf<HTMLElement>;
    var cols = document.querySelectorAll("th") as NodeListOf<HTMLElement>;
    cols.forEach((col, index) => {
      this.createResizableColumn(col, resizers[index]);
    });
  }
  createResizableColumn(col: HTMLElement, resizer: HTMLElement) {
    let x = 0;
    let w = 0;
    const mouseDownHandler = (ev: any) => {
      x = ev.clientX;
      const styles = window.getComputedStyle(col);
      w = parseInt(styles.width, 10);
      document.addEventListener('mousemove', mouseMoveHandler);
      document.addEventListener('mouseup', mouseUpHandler);
    }
    const mouseMoveHandler = function (e: any) {
      const dx = e.clientX - x;

      col.style.width = `${w + dx}px`;
    };

    const mouseUpHandler = function () {
      document.removeEventListener('mousemove', mouseMoveHandler);
      document.removeEventListener('mouseup', mouseUpHandler);
    };
    resizer.addEventListener('mousedown', mouseDownHandler);
  }

}

export interface SortSpec {
  prop: string;
  ascending: boolean;
}
export interface PageSpec {
  previousPageIndex?: number;
  pageIndex?: number;
  pageSize?: number;
  length?: number;
}
