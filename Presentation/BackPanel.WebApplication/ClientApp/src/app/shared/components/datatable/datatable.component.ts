import { Component, EventEmitter, Input, OnInit, Output, TemplateRef } from '@angular/core';
import { GeneralService } from 'src/app/core/services/general.service';
import { Column } from './column.model';
import *  as XLSX from 'xlsx';
import { MatDialog } from '@angular/material/dialog';
import { fieldMatcherSpec, FieldsMatcherComponent } from '../fields-matcher/fields-matcher.component';
import { comparisonOperators } from '../../constants/comparison-operator.list';
import { ComparisonOperator } from 'src/app/core/enums/comparison-operator.enum';
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
  @Input('show-import-button') showImport = true;
  @Input('break-word') breakWords = true;
  @Input('fixed') fixed = false;
  @Input("controls-template") controlTemplate?: TemplateRef<any>;
  @Output('pageChange') pageChangeEmitter = new EventEmitter<PageSpec>();
  @Output('sortChange') sortChangeEmitter = new EventEmitter<SortSpec>();
  @Output('searchChange') searchChangeEmitter = new EventEmitter<string>();
  @Output('createClick') createClickEmitter = new EventEmitter();
  @Output('exportClick') exportClickEmitter = new EventEmitter<string>();
  @Output('dataImported') DataImportedEventEmitter = new EventEmitter<any[]>();
  @Output('fieldSearchChanged') FieldSearchResultEventEmitter = new EventEmitter<FieldsSearchListResult>();


  theme: 'light' | 'dark' = 'light';
  sortProp = "";
  ascending = false;
  importFile: File | null = null;
  importedData = [];
  comparisonOperators = comparisonOperators;
  _searchableColumns: Column[] = [];
  _fieldSearchResult: FieldSearchResult[] = [];
  constructor(_generalService: GeneralService, private _dialog: MatDialog) {
    _generalService.$theme.subscribe(value => this.theme = value);
  }
  ngOnInit(): void {
    this.sortProp = this.columns[0].prop;
    this._searchableColumns = this.columns.filter(c => c.searchable);
    this._fieldSearchResult = this.columns.map((c) => {
      var item: FieldSearchResult = {
        propName: c.prop,
        propValue: null,
        operatorIcon: "las la-search",
        operator: ComparisonOperator.Equal
      };
      return item;
    })
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
    if (event.pageIndex)
      event.pageIndex = event.pageIndex + 1;
    this.pageChangeEmitter.emit(event);
  }
  onExportClick(type: string) {
    this.exportClickEmitter.emit(type);

  }
  onImportFileChange(event: any) {
    this.importFile = event.target.files[0];
    this.readDataFromImportedFile(() => {
      this._dialog.open<FieldsMatcherComponent, fieldMatcherSpec, any>(FieldsMatcherComponent, {
        data: {
          fromItems: Object.keys(this.importedData[0]),
          toItems: this.columns.filter(c => c.importable).map(c => c.prop),
          onCancel: () => {
            this._dialog.closeAll();
          },
          onSubmit: (value) => {
            this._dialog.closeAll();
            var result = this.extractDataUsingMap(value, this.importedData);
            this.DataImportedEventEmitter.emit(result);
          }
        }
      });
    });

  }

  onOperatorSelected(colIndex: number, operatorObject: any) {
    this._fieldSearchResult[colIndex].operator = operatorObject.value;
    this._fieldSearchResult[colIndex].operatorIcon = operatorObject.icon;
    var list = this._fieldSearchResult.filter(c => c.propValue != null && c.propValue.trim().length > 0);
    var index = list.indexOf(this._fieldSearchResult[colIndex]);
    var result: FieldsSearchListResult = { list: list, colIndex: index };
    this.FieldSearchResultEventEmitter.emit(result);

  }
  searchFieldChange(colIndex: number, target: any) {
    this._fieldSearchResult[colIndex].propValue = target.value;
    var list = this._fieldSearchResult.filter(c => c.propValue != null && c.propValue.trim().length > 0);
    var index = list.indexOf(this._fieldSearchResult[colIndex]);
    var result: FieldsSearchListResult = { list: list, colIndex: index };
    this.FieldSearchResultEventEmitter.emit(result);
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
  // configure data importer
  readDataFromImportedFile(onLoaded?: () => void) {
    if (this.importFile) {
      let fileReader = new FileReader();
      fileReader.readAsArrayBuffer(this.importFile);
      fileReader.onload = (e) => {
        var arrayBuffer = fileReader.result as ArrayBuffer;
        if (arrayBuffer) {
          var data = new Uint8Array(arrayBuffer);
          var strArr: string[] = [];
          data.forEach(element => {
            strArr.push(String.fromCharCode(element));
          });
          var jointStr = strArr.join("");
          var workbook = XLSX.read(jointStr, { type: "binary" });
          var first_sheet_name = workbook.SheetNames[0];
          var worksheet = workbook.Sheets[first_sheet_name];
          this.importedData = XLSX.utils.sheet_to_json(worksheet, { raw: true });
          if (onLoaded)
            onLoaded();
        }
      }
    }
  }
  extractDataUsingMap(map: Map<string, string>, arr: any[]): any[] {
    var newArray: any[] = [];
    arr.forEach(element => {
      var newElement: any = {};
      map.forEach((value, key, index) => {
        newElement[value] = element[key];
      });
      newArray.push(newElement);
    });
    return newArray;
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
export interface FieldsSearchListResult {
  list: FieldSearchResult[];
  colIndex: number;
}
export interface FieldSearchResult {
  propName: string;
  propValue: string | null;
  operator: ComparisonOperator;
  operatorIcon?: string;
}
