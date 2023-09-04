import { Component, OnInit } from '@angular/core';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';

@Component({
  selector: 'app-welcome',
  templateUrl: './welcome.component.html',
  styleUrls: ['./welcome.component.scss']
})
export class WelcomeComponent implements OnInit {

  constructor(
    protected breadcrumbService: BreadcrumbService,
  ) {
 this.breadcrumbService.setItems([
    ]);
  }

  ngOnInit(): void {
  }

}
