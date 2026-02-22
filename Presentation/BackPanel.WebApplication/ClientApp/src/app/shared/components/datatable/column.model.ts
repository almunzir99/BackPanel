import { SearchControlType } from "src/app/core/enums/search-control-type.enum";

export interface Column {
    title: string;
    prop: string;
    sortable: boolean;
    show: boolean;
    importable?: boolean;
    searchable?: boolean;
    searchControlType?: SearchControlType;
    searchFieldData?: any[] // data to populate the search field data
    labelProp?: string;
    valueProp?: string;
}