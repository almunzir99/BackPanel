import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'empty-placeholder',
  templateUrl: './empty.component.html',
  styleUrls: ['./empty.component.scss']
})
export class EmptyComponent implements OnInit {
  @Input('message') message = 'No Item Found';
  @Input('showButton') showButton = true;
  @Output('buttonClick') buttonClickEventEmitter = new EventEmitter();
  constructor() { }

  ngOnInit(): void {
  }

}
