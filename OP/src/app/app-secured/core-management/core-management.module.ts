import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CoreManagementRoutingModule } from './core-management-routing.module';
import { CoreServiceComponent } from './core-service/core-service.component';
import { CorePaymentsComponent } from './core-payments/core-payments.component';
import { CoreTypeCargoComponent } from './core-type-cargo/core-type-cargo.component';
import { CoreTypeShippingComponent } from './core-type-shipping/core-type-shipping.component';
import { CorePrintFormManagementComponent } from './core-print-form-management/core-print-form-management.component';
import { CoreHolidayManagementComponent } from './core-holiday-management/core-holiday-management.component';
import { CoreBankAccountsComponent } from './core-bank-accounts/core-bank-accounts.component';
import { CoreReasonManagementComponent } from './core-reason-management/core-reason-management.component';
import { CoreTypeOfComplainComponent } from './core-type-of-complain/core-type-of-complain.component';
import { CorePriceFormulaComponent } from './core-price-formula/core-price-formula.component';
import { FormsModule } from '@angular/forms';
import { CalendarModule } from 'primeng/calendar';
import { DialogModule } from 'primeng/dialog';
import { TableModule } from 'primeng/table';
import { CheckboxModule } from 'primeng/checkbox';
import { DropdownModule } from 'primeng/dropdown';
import { RadioButtonModule } from 'primeng/radiobutton';
import { MultiSelectModule } from 'primeng/multiselect';
import { CreateUpdateServiceComponent } from './core-dialog-management/create-update-service/create-update-service.component';
import { CreateUpdatePaymentComponent } from './core-dialog-management/create-update-payment/create-update-payment.component';
import { CreateUpdateTypeCargoComponent } from './core-dialog-management/create-update-type-cargo/create-update-type-cargo.component';
import { CreateUpdateTypeShippingComponent } from './core-dialog-management/create-update-type-shipping/create-update-type-shipping.component';
import { CKEditorModule } from 'ng2-ckeditor';
import { CreatePrintFormComponent } from './core-dialog-management/create-print-form/create-print-form.component';
import { CreateUpdateBankAccountComponent } from './core-dialog-management/create-update-bank-account/create-update-bank-account.component';
import { CreateUpdateReasonComponent } from './core-dialog-management/create-update-reason/create-update-reason.component';
import { CreateUpdateComlaintypeComponent } from './core-dialog-management/create-update-comlaintype/create-update-comlaintype.component';
import { CreateUpdateFormulaComponent } from './core-dialog-management/create-update-formula/create-update-formula.component';
import { CreateUpdatePromotionFormulaComponent } from './core-dialog-management/create-update-promotion-formula/create-update-promotion-formula.component';
import { CoreDiscountRecipeComponent } from './core-discount-recipe/core-discount-recipe.component';
import { DateFormatNoTimePipe } from 'src/app/shared/pipes/dateFormatNoTime.pipes';
import { TabViewModule } from 'primeng/tabview';
import { CorePricingTypeComponent } from './core-pricing-type/core-pricing-type.component';
import { CreateUpdatePricingTypeComponent } from './core-dialog-management/create-update-pricing-type/create-update-pricing-type.component';

@NgModule({
  declarations: [
    CoreServiceComponent,
    CorePaymentsComponent,
    CoreTypeCargoComponent,
    CoreTypeShippingComponent,
    CorePrintFormManagementComponent,
    CoreHolidayManagementComponent,
    CoreBankAccountsComponent,
    CoreReasonManagementComponent,
    CoreTypeOfComplainComponent,
    CorePriceFormulaComponent,
    CoreDiscountRecipeComponent,
    CreateUpdateServiceComponent,
    CreateUpdatePaymentComponent,
    CreateUpdateTypeCargoComponent,
    CreateUpdateTypeShippingComponent,
    CreatePrintFormComponent,
    DateFormatNoTimePipe,
    CreateUpdateBankAccountComponent,
    CreateUpdateReasonComponent,
    CreateUpdateComlaintypeComponent,
    CreateUpdateFormulaComponent,
    CreateUpdatePromotionFormulaComponent,
    CorePricingTypeComponent,
    CreateUpdatePricingTypeComponent,
  ],
  imports: [
    CommonModule,
    CoreManagementRoutingModule,
    FormsModule,
    CalendarModule,
    DialogModule,
    TableModule,
    CheckboxModule,
    DropdownModule,
    RadioButtonModule,
    MultiSelectModule,
    CKEditorModule,
    TabViewModule
  ]
})
export class CoreManagementModule { }
