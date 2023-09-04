import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CoreGeneralManagementRoutingModule } from './core-general-management-routing.module';
import { CoreUsersComponent } from './core-users/core-users.component';
import { CalendarModule } from 'primeng/calendar';
import { TableModule } from 'primeng/table';
import { DialogModule } from 'primeng/dialog';
import { CoreRolesComponent } from './core-roles/core-roles.component';
import { CheckboxModule } from 'primeng/checkbox';
import { FormsModule } from '@angular/forms';
import { CoreRolePagesComponent } from './core-role-pages/core-role-pages.component';
import { DropdownModule } from 'primeng/dropdown';
import { CreateUpdateUserComponent } from './core-dialog/create-update-user/create-update-user.component';
import { RadioButtonModule } from 'primeng/radiobutton';
import { CreateUpdateRoleComponent } from './core-dialog/create-update-role/create-update-role.component';
import { CoreUserRelationComponent } from './core-user-relation/core-user-relation.component';
import {MultiSelectModule} from 'primeng/multiselect';
import { AutoCompleteModule } from 'primeng/autocomplete';

@NgModule({
  declarations: [
    CoreUsersComponent,
    CoreRolesComponent,
    CoreRolePagesComponent,
    CreateUpdateUserComponent,
    CreateUpdateRoleComponent,
    CoreUserRelationComponent,
  ],
  imports: [
    FormsModule,
    CommonModule,
    CalendarModule,
    DialogModule,
    TableModule,
    CheckboxModule,
    DropdownModule,
    RadioButtonModule,
    CoreGeneralManagementRoutingModule,
    MultiSelectModule,
    AutoCompleteModule,
  ],
  exports: [
  ],
})
export class CoreGeneralManagementModule { }
