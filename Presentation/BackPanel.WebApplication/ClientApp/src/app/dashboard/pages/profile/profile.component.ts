import { Component, Inject, OnInit } from '@angular/core';
import { Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { firstValueFrom } from 'rxjs';
import { Admin } from 'src/app/core/models/admin.model';
import { RequestStatus } from 'src/app/core/models/request-status.enum';
import { AccountService } from 'src/app/core/services/account.service';
import { GeneralService } from 'src/app/core/services/general.service';
import { AlertMessage, AlertMessageComponent, MessageTypes } from 'src/app/shared/components/alert-message/alert-message.component';
import { ControlTypes } from 'src/app/shared/components/form-builder/control-type.enum';
import { FormBuilderGroup } from 'src/app/shared/components/form-builder/form-builder-group.model';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  standalone: false,
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  user: Admin | null = null;
  formGroups: FormBuilderGroup[] = [];
  passwordFormGroups: FormBuilderGroup[] = [];
  dimRequest = RequestStatus.Initial;
  theme:'light' | 'dark' = 'light';
  constructor(
    private _service: AccountService, 
    @Inject("BASE_API_URL") public baseUrl: string, 
    private _dialog: MatDialog,
    _generalService:GeneralService) {
    this.user = _service.$currentUser.value;
    _generalService.$theme.subscribe(value => this.theme = value);

  }
  onFormSubmit(body: any) {
    var user = body;
    user['image'] = !body['image'] ? 'none' : body['image'][0]['path'];
    this.updateProfile(user);
  }
  onPasswordSubmit(body: any) {
    
    this.passwordChange(body['oldPassword'],body['newPassword']);
  }
  initPasswordChangeForm(){
    this.passwordFormGroups = [
      {
        title: "Dashboard.Profile_PasswordChange",
        controls: [
          {
            title: "Dashboard.Profile_OldPassword",
            name: "oldPassword",
            icon: "key",
            controlType: ControlTypes.PasswordInput,
            width: "100%",
            validators: [
              Validators.required,
              Validators.minLength(8),
            ]
          },
          {
            title: "Dashboard.Profile_NewPassword",
            name: "newPassword",
            icon: "key",
            controlType: ControlTypes.PasswordInput,
            width: "100%",
            validators: [
              Validators.required,
              Validators.minLength(8),
            ]
          },
          {
            title: "Dashboard.Profile_NewPasswordConfirmation",
            name: "newPassword",
            icon: "key",
            controlType: ControlTypes.PasswordInput,
            width: "100%",
            validators: [
              Validators.required,
              Validators.minLength(8),
            ]
          },
        ]
      }
    ];
  }
  initFormGroups() {
    this.formGroups = [
      {
        title: "Dashboard.Profile_BasicInfo",
        controls: [
          {
            title: "id",
            name: "id",
            controlType: ControlTypes.Hidden,
            width: "0px",
            value: this.user ? this.user.id : undefined
          },
          {
            title: "Dashboard.Profile_Name",
            name: "userName",
            icon: "user tie icon",
            controlType: ControlTypes.TextInput,
            width: "50%",
            value: this.user ? this.user.userName : undefined,
            validators: [
              Validators.required,
              Validators.minLength(8),
              Validators.maxLength(25),
            ]
          },
          {
            title: "Dashboard.Profile_Phone",
            name: "phoneNumber",
            icon: "phone icon",
            controlType: ControlTypes.NumberInput,
            width: "50%",
            value: this.user ? this.user.phoneNumber : undefined,
            validators: [
              Validators.required,
              Validators.minLength(10),
              Validators.maxLength(12),
            ]
          },
          {
            title: "Dashboard.Profile_Email",
            name: "email",
            icon: "mail icon",
            controlType: ControlTypes.TextInput,
            width: "100%",
            value: this.user ? this.user.email : undefined,
            validators: [
              Validators.required
            ]
          },
          {
            title: "Dashboard.Profile_Image",
            name: "image",
            controlType: ControlTypes.LocalFilePicker,
            width: "100%",
            value: this.user ? this.user.image : undefined,
          },
        ]
      }
    ];
  }
  async updateProfile(user: any) {
    this.dimRequest = RequestStatus.Loading;

    try {
      var result = await firstValueFrom(this._service.updateProfile(user as Admin));
      this.dimRequest = RequestStatus.Success;
      if (this._service.$currentUser.value) {
        this._service.$currentUser.value.userName = result.data.userName;
        this._service.$currentUser.value.phoneNumber = result.data.phoneNumber;
        this._service.$currentUser.value.email = result.data.email;
        this._service.$currentUser.value.image = result.data.image;
      }
      this._dialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
        data: {
          type: MessageTypes.SUCCESS,
          message: "Dashboard.Profile_Success",
          title: "Dashboard.Profile_SuccessTitle"
        }
      })

    } catch (error) {
      this.dimRequest = RequestStatus.Failed;
    }
  }
  async passwordChange(oldPassword:string,newPassword:string) {
    this.dimRequest = RequestStatus.Loading;
    try {
      await firstValueFrom(this._service.changePassword(oldPassword,newPassword));
      this.dimRequest = RequestStatus.Success;
      this._dialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
        data: {
          type: MessageTypes.SUCCESS,
          message: "Dashboard.Profile_PasswordSuccess",
          title: "Dashboard.Profile_SuccessTitle"
        }
      })

    } catch (error) {
      this.dimRequest = RequestStatus.Failed;
    }
  }
  ngOnInit(): void {
    this.initFormGroups();
    this.initPasswordChangeForm();
  }

}


