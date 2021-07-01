import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
})
export class AppComponent implements OnInit {
    constructor(private router: Router) {}

    public ngOnInit(): void {
        this.router.navigate(['']);
    }
}
