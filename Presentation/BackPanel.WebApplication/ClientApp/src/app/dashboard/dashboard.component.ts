import { Component, Inject, OnInit } from '@angular/core';
import { GeneralService } from '../core/services/general.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  toggle = true;
  theme:'light' | 'dark' = 'light';
  constructor(_generalService:GeneralService,@Inject('DIRECTION') public dir:string) { 
    _generalService.$theme.subscribe(value => this.theme = value);
  }

  ngOnInit(): void {
  }

}
