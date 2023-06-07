import { Component, Inject, Input, OnInit } from '@angular/core';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { GeneralService } from 'src/app/core/services/general.service';

@Component({
  selector: 'app-fields-matcher',
  templateUrl: './fields-matcher.component.html',
  styleUrls: ['./fields-matcher.component.scss']
})
export class FieldsMatcherComponent implements OnInit {
  fromItems: any[] = [];
  toItems: any[] = [];
  matchItems: any[] = [];
  theme: 'light' | 'dark' = 'light';
  constructor(@Inject(MAT_DIALOG_DATA) public data: fieldMatcherSpec,
    _generalService: GeneralService
  ) {
    _generalService.$theme.subscribe(value => this.theme = value);
    this.fromItems = data.fromItems;
    this.toItems = data.toItems;

  }
  drop(event: CdkDragDrop<any>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex,
      );
    }
  }
  ngOnInit(): void {
  }
  onSubmit() {
    var map = new Map<string, string>();
    var shortestArrayLength = this.toItems.length > this.matchItems.length ? this.matchItems.length : this.toItems.length;
    for (let index = 0; index < shortestArrayLength; index++) {
      map.set(this.matchItems[index], this.toItems[index]);
    }
    if (this.data.onSubmit)
      this.data.onSubmit(map);
  }
  onCancel() {
    if (this.data.onCancel)
      this.data.onCancel();
  }

}
export interface fieldMatcherSpec {
  fromItems: string[],
  toItems: string[],
  onSubmit?: (result: Map<string, string>) => void
  onCancel?: () => void

}