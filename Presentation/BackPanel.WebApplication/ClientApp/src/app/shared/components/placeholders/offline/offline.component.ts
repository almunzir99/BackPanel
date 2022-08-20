import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'offline-placeholder',
  templateUrl: './offline.component.html',
  styleUrls: ['./offline.component.scss']
})
export class OfflineComponent implements OnInit {
  @Input('message') message = 'Operation Failed, please try again';
  @Input('showButton') showButton = true;
  @Output('buttonClick') buttonClickEventEmitter = new EventEmitter();
  constructor() { }

  ngOnInit(): void {
  }

}
