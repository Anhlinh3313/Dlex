import { MsgService } from './../services/local/msg.service';
import { Constant } from '../infrastructure/constant';
import { Message } from 'primeng/api';
import { Router } from '@angular/router';
import { ResponseModel } from '../models/entity/response.model';
import { PermissionService } from '../services/api/permission.service';

export class BaseComponent {

  isAdd = true;
  isEdit = true;
  isDelete = true;
  isHyper = true;

  constructor(protected messageService: MsgService,
              protected router: Router,
              protected permissionService: PermissionService,
    ) {
      this.permissionService.checkPermissionDetail(window.location.pathname, 1).then(x => {
        if (x.isSuccess) {
          if (x.data.length > 0) {
            const data = x.data[0];
            if (!data.access) {
              this.router.navigate(['403']);
            }
            else {
              this.isAdd = data.add;
              this.isEdit = data.edit;
              this.isDelete = data.delete;
            }
          }
        }
      });
  }

  clone(model: object): object {
    const data = new Object();
    for (const prop in model) {
      if (Object.prototype.hasOwnProperty.call(model, prop)) {
        data[prop] = model[prop];
      }
    }
    return data;
  }
  onKeypressValidate(inputStr: any): any {
    let isExist = false;
    const nameInput = inputStr.name;
    const pattern = RegExp(inputStr.pattern || '');
    if (nameInput === 'email') {
      if (!pattern.test(inputStr.value)) {
        isExist = true;
      }
    }
    if (isExist) {
      inputStr.focus();
    }
  }

  isValidResponse(x: ResponseModel): boolean {
    if (!x.isSuccess) {
      if (x.message) {
        this.messageService.add({ severity: Constant.messageStatus.warn, detail: x.message });
      } else if (x.data) {
        const mess: Message[] = [];

        if (Array.isArray(x.data)) {
          x.data = x.data[0];
        }

        for (const key in x.data) {
          if (Object.prototype.hasOwnProperty.call(x.data, key)) {
            const element = x.data;
            if (key !== 'key') {
              mess.push({ severity: Constant.messageStatus.warn, detail: element[key] });
            }
          }
        }
        this.messageService.addAll(mess);
      }
    }

    return x.isSuccess;
  }
}

export class ColumnExcel {
  Name: string;
  Index: number;
}