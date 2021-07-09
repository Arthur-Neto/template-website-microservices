export interface IHeaderModel {
    columnName: string;
    headerName: string;
}

export interface IActionModel {
    icon?: string;
    name: string;
    function: () => any;
}

export class IGridModel {
    id!: string;
}
