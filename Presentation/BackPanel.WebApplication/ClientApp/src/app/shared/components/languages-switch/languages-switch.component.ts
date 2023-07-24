import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'languages-switch',
  templateUrl: './languages-switch.component.html',
  styleUrls: ['./languages-switch.component.scss']
})
export class LanguagesSwitchComponent implements OnInit {
  @Output("onChange") onChange = new EventEmitter<string>();
  constructor(private translateService:TranslateService) { }

  ngOnInit(): void {
  }
  onSwitch() {
    var lang = this.translateService.currentLang == "ar" ? "en" : "ar";
    this.onChange.emit(lang);
  }

}
