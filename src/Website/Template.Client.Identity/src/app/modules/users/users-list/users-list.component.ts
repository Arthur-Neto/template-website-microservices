import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { finalize, take } from 'rxjs/operators';

import { IUsersGridModel } from '../shared/users.model';
import { UsersODataService } from '../shared/users.service';

@Component({
    templateUrl: './users-list.component.html',
    styleUrls: ['./users-list.component.scss'],
})
export class UserListComponent implements OnInit {
    public readonly headerNames = ['Username', 'Role'];
    public readonly displayedColumns = ['id', 'username', 'role'];

    public isLoading = true;
    public dataSource!: MatTableDataSource<IUsersGridModel>;

    constructor(private usersODataService: UsersODataService) {}

    public ngOnInit(): void {
        this.usersODataService
            .get()
            .pipe(
                take(1),
                finalize(() => (this.isLoading = false))
            )
            .subscribe((users: IUsersGridModel[]) => {
                this.dataSource = new MatTableDataSource(users);
            });
    }
}
