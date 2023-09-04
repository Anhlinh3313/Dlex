import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateUpdateUserComponent } from './create-update-user.component';
import { CreateUpdateUserRoutingModule } from './create-update-user-routing.module';


@NgModule({
  declarations: [
  ],
  imports: [
    CommonModule,
    CreateUpdateUserComponent,
    CreateUpdateUserRoutingModule,
  ]
})
export class CreateUpdateUserModule { }
