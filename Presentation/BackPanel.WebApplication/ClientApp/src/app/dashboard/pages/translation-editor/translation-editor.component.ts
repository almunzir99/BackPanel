import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { firstValueFrom, forkJoin, map } from 'rxjs';
import { RequestStatus } from 'src/app/core/models/request-status.enum';
import { GeneralService } from 'src/app/core/services/general.service';
import { TranslationEditorService } from 'src/app/core/services/translation-editor.service';
import { AlertMessage, AlertMessageComponent, MessageTypes } from 'src/app/shared/components/alert-message/alert-message.component';
import { ControlTypes } from 'src/app/shared/components/form-builder/control-type.enum';
import { FormBuilderGroup } from 'src/app/shared/components/form-builder/form-builder-group.model';
import { FormBuilderComponent, FormBuilderPropsSpec } from 'src/app/shared/components/form-builder/form-builder.component';
import { langs } from 'src/app/shared/extras/languages';

@Component({
  selector: 'app-translation-editor',
  templateUrl: './translation-editor.component.html',
  styleUrls: ['./translation-editor.component.scss']
})
export class TranslationEditorComponent implements OnInit {
  translationTree: any = {};
  languages: string[] = [];
  getRequest = RequestStatus.Initial;
  dimRequest = RequestStatus.Initial;
  editMode: 'new' | 'modify' = 'new';
  selectedNode: any = {};
  theme: 'light' | 'dark' = 'light';
  closed = false; // toggle tree section on smaller devices
  controlPressed = false
  constructor(private _service: TranslationEditorService,
    private _dialog: MatDialog,
    _generalService: GeneralService) {
    _generalService.$theme.subscribe(value => this.theme = value);
    document.addEventListener("keydown", (event) => {
      if (event.ctrlKey) {
        this.controlPressed = true;
        setTimeout(() => {
          this.controlPressed = false;

        }, 500);
      }
      if (this.controlPressed && event.code == 'KeyE') {
        this.initializeSelectedNode(this.selectedNode.parent);
      }
      if(this.controlPressed && event.code == 'Equal') {
        this.onApply();
      }
    })

  }
  initializeSelectedNode(parent: string | null = null) {
    this.editMode = 'new';
    this.selectedNode['parent'] = parent;
    this.selectedNode['title'] = null;
    this.languages.forEach(lang => {
      this.selectedNode[lang] = null;
    });
    this.closed = true;

  }

  getKeys(object: any) {
    return Object.keys(object);
  }
  selectNode(parent: string, title: string, node: any) {
    this.selectedNode['parent'] = parent;
    this.editMode = 'modify';
    this.selectedNode['title'] = title;
    this.languages.forEach(lang => {
      this.selectedNode[lang] = node[lang];
    });
    this.closed = true;
  }
  onApply() {
    var values: any = {};
    this.languages.forEach(lang => {
      values[lang] = this.selectedNode[lang];
    });
    if (this.selectedNode['title'] == null)
      this.createNode(this.selectedNode['parent'], this.selectedNode['title'], values)
    else
      this.updateNode(this.selectedNode['parent'], this.selectedNode['title'], values)
    this.initializeSelectedNode(this.selectedNode.parent)

  }
  onDelete() {
    this._dialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
      data: {
        type: MessageTypes.CONFIRM,
        message: "Are Sure you want to Delete this Item ?",
        title: "Confirm"
      }
    }).afterClosed().subscribe({
      next: (res) => {
        if (res == true)
          this.deleteNode(this.selectedNode['parent'], this.selectedNode['title']);
      }
    })
  }
  onParentCreate() {
    this.OpenParentDialog();
  }
  onParentUpdate() {
    this.OpenParentDialog(this.selectedNode['parent']);
  }
  onParentDelete() {
    this._dialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
      data: {
        type: MessageTypes.CONFIRM,
        message: "Are Sure you want to Delete this Item ?",
        title: "Confirm"
      }
    }).afterClosed().subscribe({
      next: (res) => {
        if (res == true)
          this.deleteParent(this.selectedNode['parent']);
      }
    })
  }

  onLangaugeCreate() {
    this.OpenLanguageDialog();
  }
  OpenParentDialog(title: string | undefined = undefined) {
    var formControlsGroup: FormBuilderGroup[] = [
      {
        title: "Create New parent",
        controls: [
          {
            title: "Parent Name",
            name: "title",
            value: title,
            controlType: ControlTypes.TextInput,
            icon: 'folder_open'
          }
        ]
      }
    ];
    this._dialog.open<FormBuilderComponent, FormBuilderPropsSpec, any>(FormBuilderComponent, {
      data: {
        controlsGroups: formControlsGroup,
        onSubmit: (res) => {
          this._dialog.closeAll();
          if (!title)
            this.createParent(res['title']);
          else
            this.updateParent(title, res['title']);
        },
        onCancel: () => {
          this._dialog.closeAll();
        },
      },
    })
  }
  OpenLanguageDialog() {
    var formControlsGroup: FormBuilderGroup[] = [
      {
        title: "Create New Language",
        controls: [
          {
            title: "Language",
            name: "code",
            data: langs,
            labelProp: 'name',
            valueProp: 'code',
            controlType: ControlTypes.Selection,
            icon: 'translate'
          }
        ]
      }
    ];
    this._dialog.open<FormBuilderComponent, FormBuilderPropsSpec, any>(FormBuilderComponent, {
      data: {
        controlsGroups: formControlsGroup,
        onSubmit: (res) => {
          this.createLanguage(res['code']);
          this._dialog.closeAll();
        },
        onCancel: () => {
          this._dialog.closeAll();
        },
      },
    })
  }
  /********************************* Api Integration ******************************************** */

  async getData() {
    try {
      this.getRequest = RequestStatus.Loading;
      var obs = forkJoin([this._service.getLanguages(), this._service.getLanguageTree()]).pipe(map(([languages, tree]) => {
        return { languages, tree };
      }));
      var result = await firstValueFrom(obs);
      this.languages = result.languages.data;
      this.translationTree = result.tree.data;
      this.getRequest = RequestStatus.Success;

    } catch (error) {
      this.getRequest = RequestStatus.Failed;
    }
  }
  async createNode(parent: string, title: string, values: any) {
    this.dimRequest = RequestStatus.Loading;
    try {
      await firstValueFrom(this._service.createNode(parent, title, values));
      this.dimRequest = RequestStatus.Success;
      this.translationTree[parent][title] = values;
    } catch (error) {
      this.dimRequest = RequestStatus.Failed;
    }
  }
  async updateNode(parent: string, title: string, values: any) {
    this.dimRequest = RequestStatus.Loading;
    try {
      await firstValueFrom(this._service.updateNode(parent, title, values));
      this.dimRequest = RequestStatus.Success;
      this.translationTree[parent][title] = values;
    } catch (error) {
      this.dimRequest = RequestStatus.Failed;
    }
  }
  async deleteNode(parent: string, title: string) {
    this.dimRequest = RequestStatus.Loading;
    try {
      await firstValueFrom(this._service.deleteNode(parent, title));
      this.dimRequest = RequestStatus.Success;
      delete this.translationTree[parent][title];
      this.initializeSelectedNode(parent);
    } catch (error) {
      this.dimRequest = RequestStatus.Failed;
    }
  }
  async createParent(title: string) {
    this.dimRequest = RequestStatus.Loading;
    try {
      await firstValueFrom(this._service.createParent(title));
      this.dimRequest = RequestStatus.Success;
      this.translationTree[title] = {};
    } catch (error) {
      this.dimRequest = RequestStatus.Failed;
    }
  }
  async updateParent(oldTitle: string, newTitle: string) {
    this.dimRequest = RequestStatus.Loading;
    try {
      await firstValueFrom(this._service.updateParent(oldTitle, newTitle));
      this.dimRequest = RequestStatus.Success;
      var value = this.translationTree[oldTitle];
      this.translationTree[newTitle] = value;
      delete this.translationTree[oldTitle];
    } catch (error) {
      this.dimRequest = RequestStatus.Failed;
    }
  }
  async deleteParent(title: string) {
    this.dimRequest = RequestStatus.Loading;
    try {
      await firstValueFrom(this._service.deleteParent(title));
      this.dimRequest = RequestStatus.Success;
      delete this.translationTree[title];
      this.initializeSelectedNode();
    } catch (error) {
      this.dimRequest = RequestStatus.Failed;
    }
  }
  async createLanguage(code: string) {
    this.dimRequest = RequestStatus.Loading;
    try {
      await firstValueFrom(this._service.CreateLanguage(code));
      this.dimRequest = RequestStatus.Success;
      this.getData();
      this.initializeSelectedNode();
    } catch (error) {
      this.dimRequest = RequestStatus.Failed;
    }
  }
  ngOnInit(): void {
    this.getData();
    this.initializeSelectedNode();
  }

}
