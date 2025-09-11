import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import { TemplatePortal } from '@angular/cdk/portal';
import { Component, OnInit, TemplateRef, ViewChild, ViewContainerRef } from '@angular/core';

@Component({
  selector: 'dimmer-loading',
  templateUrl: './dimmer-loading.component.html',
  styleUrls: ['./dimmer-loading.component.scss']
})
export class DimmerLoadingComponent implements OnInit {

  private overlayRef: OverlayRef | null = null;
  @ViewChild('dimmerTemplate') dimmerTemplate!: TemplateRef<any>;

  constructor(private overlay: Overlay, private viewContainerRef: ViewContainerRef) {
    setTimeout(() => {
      this.show();
    }, 500);
  }
  show() {
    if (this.overlayRef) {
      return;
    }

    this.overlayRef = this.overlay.create({
      hasBackdrop: true,
      backdropClass: 'loading-backdrop',
      panelClass: 'loading-panel',
      positionStrategy: this.overlay.position()
        .global()
        .centerHorizontally()
        .centerVertically()
    });

    const spinnerPortal = new TemplatePortal(this.dimmerTemplate, this.viewContainerRef);
    this.overlayRef.attach(spinnerPortal);
  }

  ngOnInit(): void {
  }

  ngOnDestroy() {
    setTimeout(() => {
      if (this.overlayRef) {
        console.log('destroy');
        this.overlayRef.detachBackdrop();
        this.overlayRef.dispose();
        this.overlayRef = null;
      }
    }, 550);
  }
}
