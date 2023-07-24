import { Component, Inject, OnInit } from '@angular/core';
import { GeneralService } from '../core/services/general.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  toggle = true;
  theme: 'light' | 'dark' = 'light';
  constructor(_generalService: GeneralService,private _translateService:TranslateService ,@Inject('DIRECTION') public dir: string) {
    _generalService.$theme.subscribe(value => this.theme = value);
  }
  onLanguageChange(lang: string) {
    this.dir = lang == "ar" ? "rtl" : "ltr";
    this._translateService.use(lang)
  }
  ngOnInit(): void {
  }

}
