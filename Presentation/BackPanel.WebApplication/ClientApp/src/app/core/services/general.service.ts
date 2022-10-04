import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GeneralService {
  $theme = new BehaviorSubject<'light' | 'dark'>('light');
  constructor() {
    var currentTheme = localStorage.getItem('themeMode');
    if (currentTheme)
      this.$theme.next(currentTheme as 'light' | 'dark');
    this.$theme.subscribe(value => {
      localStorage.setItem('themeMode', value);
    })
  }
}
