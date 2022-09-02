import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'dashboard-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  @Output("toggleClick") toggleClickEventEmitter = new EventEmitter<boolean>();
  @Input("toggle") toggle = false;
  opened = false;
  constructor() { }

  ngOnInit(): void {
  } 
  onToggle(){
    this.toggle = !this.toggle;
    this.toggleClickEventEmitter.emit(this.toggle);
  }
}
