import { Component, OnInit } from '@angular/core';
import { Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Column } from 'src/app/shared/components/datatable/column.model';
import { ControlTypes } from 'src/app/shared/components/form-builder/control-type.enum';
import { FormBuilderGroup } from 'src/app/shared/components/form-builder/form-builder-group.model';
import { FormBuilderComponent, FormBuilderPropsSpec } from 'src/app/shared/components/form-builder/form-builder.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  
  constructor(private dialog:MatDialog) {

   }

  ngOnInit(): void {
  }

}
