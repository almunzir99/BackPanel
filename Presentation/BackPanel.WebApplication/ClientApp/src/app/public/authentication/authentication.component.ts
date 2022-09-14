import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { AuthenticationModel } from 'src/app/core/models/authentication.model';
import { RequestStatus } from 'src/app/core/models/request-status.enum';
import { AuthService } from 'src/app/core/services/auth.service';
import { AlertMessage, AlertMessageComponent, MessageTypes } from 'src/app/shared/components/alert-message/alert-message.component';
import { ApiResponse } from 'src/app/core/models/wrappers/api-response.model';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.scss']
})
export class AuthenticationComponent implements OnInit {
  requestStatus = RequestStatus.Initial;
  constructor(private _authService: AuthService, private _dialog: MatDialog,private router:Router) { }
  formSubmitted(body:AuthenticationModel){
    this.authenticate(body);

  }
  async authenticate(model:AuthenticationModel) {
    try {
      this.requestStatus = RequestStatus.Loading;
      var result = await firstValueFrom(this._authService.autthenticate(model));
      this._authService.setCurrentUser(result.data);
      this._authService.saveToken(result.data.token);
      var notifications = await firstValueFrom(this._authService.getNotifications());
      this._authService.$notifications.next(notifications.data);
      this.requestStatus = RequestStatus.Success;
      this.router.navigate(['/','dashboard']);
    } 
    
    catch (error) {
      this.requestStatus = RequestStatus.Failed;
      if(error instanceof HttpErrorResponse && error.status == 400)
      {
        var apiError =  error.error as ApiResponse<any>;
        this._dialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
          data: {
            type: MessageTypes.ERROR,
            message: `${apiError.message}`,
            title: "Failed"
          }
        })
      }
      else{
        this._dialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
          data: {
            type: MessageTypes.ERROR,
            message: "Sorry, Operation Failed Please try again",
            title: "Failed"
          }
        })
      }
    }
  }
  ngOnInit(): void {
    if(this._authService.$currentUser.value)
    this.router.navigate(['dashboard']); 
  }

}
