import { AfterViewInit, Component } from '@angular/core';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements AfterViewInit {
  title = 'op-angular';
  timerLoading: any;
  constructor(
    private messageService: MessageService
  ) {
    this.timerLoading = setInterval(() => {
      const el = document.getElementsByClassName('active-menuitem');
      if (el.item(0)) {
        el.item(0).scrollIntoView();
        clearInterval(this.timerLoading);
      }
    }, 1000);
  }

  onConfirm(): void {
    this.messageService.clear('custom-message');
  }

  ngAfterViewInit(): void {
  }
}
