import { Message } from 'primeng/api';
import { Constant } from '../../infrastructure/constant';
import { ResponseModel } from '../../models/entity/response.model';
import { MsgService } from '../../services/local/msg.service';

export class HandleError {
  constructor(protected messageService: MsgService) { }

  isValidResponse(x: ResponseModel): boolean {
    if (!x.isSuccess) {
      if (x.message) {
        this.messageService.warn(x.message);
      } else if (x.data) {
        const mess: Message[] = [];

        for (const key in x.data) {
          if (Object.prototype.hasOwnProperty.call(x.data, key)) {
            const element = x.data[key];
            mess.push({ severity: Constant.messageStatus.warn, detail: element });
          }
        }
        this.messageService.addAll(mess);
      }
      else {
        this.messageService.msgBoxError('Đã có lỗi xảy ra! Vui lòng thử lại!');
      }
    }

    return x.isSuccess;
  }
}
