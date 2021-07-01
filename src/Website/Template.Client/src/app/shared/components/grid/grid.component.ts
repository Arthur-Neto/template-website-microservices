import { SelectionModel } from '@angular/cdk/collections';
import {
    AfterViewInit,
    Component,
    EventEmitter,
    Input,
    OnInit,
    Output,
    ViewChild,
} from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTable, MatTableDataSource } from '@angular/material/table';

import { IActionModel } from './shared/grid.model';

@Component({
    selector: 'app-grid',
    templateUrl: './grid.component.html',
    styleUrls: ['./grid.component.scss'],
})
export class GridComponent<T> implements OnInit, AfterViewInit {
    @Input() public headerNames!: string[];
    @Input() public displayedColumns!: string[];
    @Input() public dataSource!: MatTableDataSource<T>;
    @Input() public actions!: IActionModel[];

    @Output() public selectionChange: EventEmitter<T> = new EventEmitter();

    @ViewChild(MatPaginator, { static: false }) paginator!: MatPaginator;
    @ViewChild(MatSort, { static: false }) sort!: MatSort;
    @ViewChild(MatTable, { static: false }) table!: MatTable<T>;

    public selection = new SelectionModel<T>(false, []);
    public noRowsMsg = 'No data';
    public hasIdColumn!: boolean;
    public headers!: string[];

    public ngOnInit(): void {
        this.hasIdColumn = this.displayedColumns[0] === 'id';
        this.headers = this.hasIdColumn
            ? this.displayedColumns.slice(1)
            : this.displayedColumns;
    }

    public ngAfterViewInit(): void {
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
    }

    public onApplyFilter(event: Event): void {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
        this.noRowsMsg = `No data matching the filter "${filterValue}"`;

        if (this.dataSource.paginator) {
            this.dataSource.paginator.firstPage();
        }
    }

    public onGridAction(event: () => any): void {
        event();
        this.onSelectionChanged(null as any);
    }

    public onSelectionChanged(row: T): void {
        this.selection.toggle(row);
        this.selectionChange.emit(row);
    }
}
