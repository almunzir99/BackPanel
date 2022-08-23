import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'alert-message',
  templateUrl: './alert-message.component.html',
  styleUrls: ['./alert-message.component.scss']
})
export class AlertMessageComponent implements OnInit {
  types = MessageTypes;
  message!:AlertMessage;
  
  constructor(public dialogRef: MatDialogRef<AlertMessageComponent>,@Inject(MAT_DIALOG_DATA) public data: AlertMessage) {
      this.message = data;
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
}
export enum MessageTypes {
  SUCCESS,
  WARNING,
  ERROR,
  CONFIRM
} 
