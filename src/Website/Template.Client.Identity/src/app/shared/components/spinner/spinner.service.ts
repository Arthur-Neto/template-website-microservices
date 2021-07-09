import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';
import { Injectable } from '@angular/core';
import { defer, NEVER } from 'rxjs';
import { finalize, share } from 'rxjs/operators';

import { SpinnerComponent } from './spinner.component';

@Injectable({
    providedIn: 'root',
})
export class SpinnerService {
    private overlayRef: OverlayRef | undefined = undefined;

    constructor(private overlay: Overlay) {}

    public readonly spinner$ = defer(() => {
        this.show();

        return NEVER.pipe(
            finalize(() => {
                this.hide();
            })
        );
    }).pipe(share());

    public hide(): void {
        this.overlayRef?.detach();
        this.overlayRef = undefined;
    }

    private show(): void {
        Promise.resolve(null).then(() => {
            if (!this.overlayRef) {
                this.overlayRef = this.overlay.create({
                    positionStrategy: this.overlay
                        .position()
                        .global()
                        .centerHorizontally()
                        .centerVertically(),
                    hasBackdrop: true,
                });
            }
            this.overlayRef.attach(new ComponentPortal(SpinnerComponent));
        });
    }
}
