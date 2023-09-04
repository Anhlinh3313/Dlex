import { Injectable } from '@angular/core';
import { MessageService, Message } from 'primeng/api';

@Injectable({
  providedIn: 'root'
})
export class MsgService {

  constructor(
    private messageService: MessageService
  ) { }

  success(message: string): any {
    this.messageService.add({ severity: 'success', detail: message });
  }

  info(message: string): any {
    this.messageService.add({ severity: 'info', detail: message });
  }

  warn(message: string): any {
    this.messageService.add({ severity: 'warn', detail: message });
  }

  error(message: string): any {
    this.messageService.add({ severity: 'error', detail: message});
  }

  add(message: Message): any {
    this.messageService.add(message);
  }

  addAll(message: Message[]): any {
    this.messageService.addAll(message);
  }

  msgBoxSuccess(message: string): any {
    this.messageService.clear();
    this.messageService.add({key: 'custom-message', closable: true, sticky: true, severity: 'success', detail: message});
  }
  msgBoxWarn(message: string): any {
    this.messageService.clear();
    this.messageService.add({key: 'custom-message', closable: true, sticky: true, severity: 'warn', detail: message});
  }
  msgBoxError(message: string): any {
    this.messageService.clear();
    this.messageService.add({key: 'custom-message', closable: true, sticky: true, severity: 'error', detail: message});
  }

  clear(): any {
    this.messageService.clear();
  }
}
