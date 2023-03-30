import { Component, Inject, OnInit } from '@angular/core';
import { Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { firstValueFrom } from 'rxjs';
import { Admin } from 'src/app/core/models/admin.model';
import { RequestStatus } from 'src/app/core/models/request-status.enum';
import { AuthService } from 'src/app/core/services/auth.service';
import { GeneralService } from 'src/app/core/services/general.service';
import { AlertMessage, AlertMessageComponent, MessageTypes } from 'src/app/shared/components/alert-message/alert-message.component';
import { ControlTypes } from 'src/app/shared/components/form-builder/control-type.enum';
import { FormBuilderGroup } from 'src/app/shared/components/form-builder/form-builder-group.model';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  user: Admin | null = null;
  formGroups: FormBuilderGroup[] = [];
  passwordFormGroups: FormBuilderGroup[] = [];
  dimRequest = RequestStatus.Initial;
  theme:'light' | 'dark' = 'light';
  constructor(
    private _service: AuthService, 
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

        title: "Password Change",
        controls: [

          {
            title: "Old Password",
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
            title: "New Password",
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
            title: "New Password Confirmation",
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

        title: "Basic User Information",
        controls: [
          {
            title: "id",
            name: "id",
            controlType: ControlTypes.Hidden,
            width: "0px",
            value: this.user ? this.user.id : undefined
          },
          {
            title: "Name",
            name: "username",
            icon: "user tie icon",
            controlType: ControlTypes.TextInput,
            width: "50%",
            value: this.user ? this.user.username : undefined,
            validators: [
              Validators.required,
              Validators.minLength(8),
              Validators.maxLength(25),
            ]
          },
          {
            title: "phone",
            name: "phone",
            icon: "phone icon",
            controlType: ControlTypes.NumberInput,
            width: "50%",
            value: this.user ? this.user.phone : undefined,
            validators: [
              Validators.required,
              Validators.minLength(10),
              Validators.maxLength(12),

            ]

          },
          {
            title: "email",
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
            title: "image",
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
        this._service.$currentUser.value.username = result.data.username;
        this._service.$currentUser.value.phone = result.data.phone;
        this._service.$currentUser.value.email = result.data.email;
        this._service.$currentUser.value.image = result.data.image;
      }
      this._dialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
        data: {
          type: MessageTypes.SUCCESS,
          message: "Profile Updated Successfully",
          title: "Success"
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
          message: "Password Changed Successfully",
          title: "Success"
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


