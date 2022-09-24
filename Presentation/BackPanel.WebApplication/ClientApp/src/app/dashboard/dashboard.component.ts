import { Component, Inject, OnInit } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  toggle = true;
  constructor(@Inject('DIRECTION') public dir:string) { }

  ngOnInit(): void {
  }

}
