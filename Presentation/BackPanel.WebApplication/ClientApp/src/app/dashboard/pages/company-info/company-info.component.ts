import { Component, Inject, OnInit } from '@angular/core';
import { Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { firstValueFrom } from 'rxjs';
import { CompanyInfo } from 'src/app/core/models/company-info.model';
import { RequestStatus } from 'src/app/core/models/request-status.enum';
import { CompanyInfoService } from 'src/app/core/services/company-info.service';
import { GeneralService } from 'src/app/core/services/general.service';
import { AlertMessage, AlertMessageComponent, MessageTypes } from 'src/app/shared/components/alert-message/alert-message.component';
import { ControlTypes } from 'src/app/shared/components/form-builder/control-type.enum';
import { FormBuilderGroup } from 'src/app/shared/components/form-builder/form-builder-group.model';

@Component({
  selector: 'app-company-info',
  templateUrl: './company-info.component.html',
  styleUrls: ['./company-info.component.scss']
})
export class CompanyInfoComponent implements OnInit {

  company: CompanyInfo | null = null;
  theme: 'light' | 'dark' = 'light';
  dimRequest = RequestStatus.Initial;
  formGroups: FormBuilderGroup[] = [];
  constructor(
    private _service: CompanyInfoService,
    @Inject("BASE_API_URL") public baseUrl: string,
    private _dialog: MatDialog,
    _generalService: GeneralService
  ) {
    this.company = _service.$companyIfo;
    _generalService.$theme.subscribe(value => this.theme = value);

  }
  async getData() {
    try {
      this.dimRequest = RequestStatus.Loading;
      // load some data
      this.initFormGroups();
      this.dimRequest = RequestStatus.Success;
    } catch (error) {
      console.log(error);
      this.dimRequest = RequestStatus.Failed;

    }
  }
  initFormGroups() {
    this.formGroups = [
      {

        title: "Basic company Information",
        controls: [
          {
            title: "Id",
            name: "id",
            controlType: ControlTypes.Hidden,
            width: "0px",
            value: this.company ? this.company.id : undefined
          },
          {
            title: "Company Name",
            name: "companyName",
            icon: "domain",
            controlType: ControlTypes.TextInput,
            width: "100%",
            value: this.company ? this.company.companyName : undefined,
            validators: [
              Validators.required,
              Validators.minLength(8),
              Validators.maxLength(25),
            ]
          },
          {
            title: "Phone Number",
            name: "phoneNumber",
            icon: "phone",
            controlType: ControlTypes.NumberInput,
            width: "50%",
            value: this.company ? this.company.phoneNumber : undefined,
            validators: [
              Validators.required,
              Validators.minLength(10),
              Validators.maxLength(12),
            ]

          },
          {
            title: "Fax",
            name: "fax",
            icon: "fax",
            controlType: ControlTypes.NumberInput,
            width: "50%",
            value: this.company ? this.company.fax : undefined,

          },
          {
            title: "Email",
            name: "email",
            icon: "mail",
            controlType: ControlTypes.TextInput,
            width: "100%",
            value: this.company ? this.company.email : undefined,
            validators: [
              Validators.required
            ]
          },
          {
            name: 'aboutUs',
            title: 'About the company',
            controlType: ControlTypes.RichTextEditor,
            value: this.company ? this.company.aboutUs : undefined,
            width: '100%'
          },
          {
            title: "Address",
            name: "address",
            icon: "map",
            controlType: ControlTypes.TextInput,
            width: "100%",
            value: this.company ? this.company.address : undefined,
            validators: [
              Validators.required
            ]
          },
          {
            title: "الصورة",
            name: "logo",
            controlType: ControlTypes.LocalFilePicker,
            width: "100%",
          }
        ]
      }
    ];
  }
  onFormSubmit(body: any) {
    var info = body;
    info.logo = !body['logo'] || !body['logo'][0] ? info.logo : { path: body['logo'][0]['path'] };
    this.update(info);
  }
  async update(info: CompanyInfo) {
    this.dimRequest = RequestStatus.Loading;

    try {
      var result = await firstValueFrom(this._service.put(info));
      this.dimRequest = RequestStatus.Success;
      if (this._service.$companyIfo) {
        this._service.$companyIfo.companyName = result.data.companyName;
        this._service.$companyIfo.phoneNumber = result.data.phoneNumber;
        this._service.$companyIfo.email = result.data.email;
        this._service.$companyIfo.address = result.data.address;
        this._service.$companyIfo.logo = result.data.logo;
        this._service.$companyIfo.deliveryCompanyId = result.data.deliveryCompanyId;
        this._service.$companyIfo.aboutUs = result.data.aboutUs;
      }
      this._dialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
        data: {
          type: MessageTypes.SUCCESS,
          message: "تم تحديث بيانات المؤسسة  بنجاح",
          title: "نجاح"
        }
      })

    } catch (error) {
      this.dimRequest = RequestStatus.Failed;
    }
  }
  ngOnInit(): void {
    this.getData();
  }

}
