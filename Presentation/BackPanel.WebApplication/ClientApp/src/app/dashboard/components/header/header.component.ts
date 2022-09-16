import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'dashboard-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  @Output("toggleClick") toggleClickEventEmitter = new EventEmitter<boolean>();
  @Input("toggle") toggle = false;
  opened = false;
  isFullScreen = false;
  constructor(private _authService: AuthService, private router: Router) { }

  ngOnInit(): void {
  }
  onToggle() {
    this.toggle = !this.toggle;
    this.toggleClickEventEmitter.emit(this.toggle);
  }
  logout() {
    this._authService.logout();
    this.router.navigate(['authentication']);
  }
  openFullScreen() {
    var body = document.getElementById("main-body");
    if (body) {
      if (!this.isFullScreen) {
        body.requestFullscreen();
        this.isFullScreen = true;
      }
      else {
        document.exitFullscreen();
        this.isFullScreen = false;
      }
    }
  }
}
