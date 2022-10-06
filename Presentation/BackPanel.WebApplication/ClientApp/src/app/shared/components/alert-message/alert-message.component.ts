import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { GeneralService } from 'src/app/core/services/general.service';

@Component({
  selector: 'alert-message',
  templateUrl: './alert-message.component.html',
  styleUrls: ['./alert-message.component.scss']
})
export class AlertMessageComponent implements OnInit {
  types = MessageTypes;
  message!:AlertMessage;
  errorOpened = false;
  theme:'light' | 'dark' = 'light';
  
  constructor(public dialogRef: MatDialogRef<AlertMessageComponent>,@Inject(MAT_DIALOG_DATA) public data: AlertMessage,_generalService:GeneralService) {
      this.message = data;
    _generalService.$theme.subscribe(value => this.theme = value);

   }

  ngOnInit(): void {
  }
  close(value:boolean)
  {
    this.dialogRef.close(value);
  }
}
 
export interface AlertMessage {
  type:MessageTypes;
  title:string;
  message:string;
  errors?:string[];
}
export enum MessageTypes {
  SUCCESS,
  WARNING,
  ERROR,
  CONFIRM
} 
