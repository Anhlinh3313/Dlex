import { NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { TabViewModule } from 'primeng/tabview';
import { CalendarModule } from 'primeng/calendar';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MenuComponent } from './menu/menu.component';
import { FooterComponent } from './footer/footer.component';
import { MenuItemComponent } from './menu-item/menu-item.component';
import { RouterModule } from '@angular/router';
import { TopbarComponent } from './topbar/topbar.component';
import {DropdownModule} from 'primeng/dropdown';
import { DialogService, DynamicDialogModule } from 'primeng/dynamicdialog';
import { BreadcrumbComponent } from './breadcrumb/breadcrumb.component';
import { Page404Component } from './403/403.component';


@NgModule({
  declarations: [
    MenuComponent,
    FooterComponent,
    MenuItemComponent,
    TopbarComponent,
    BreadcrumbComponent,
    Page404Component
  ],
  providers: [
    DialogService
  ],
  imports: [
    RouterModule,
    ButtonModule,
    TabViewModule,
    CalendarModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    DropdownModule,
    DynamicDialogModule,
  ],
  exports: [
    MenuComponent,
    FooterComponent,
    MenuItemComponent,
    TopbarComponent,
    BreadcrumbComponent,
    Page404Component
  ]
})
export class ComponentsModule { }
