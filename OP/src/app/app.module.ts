import { BrowserModule } from '@angular/platform-browser';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgProgressModule } from 'ngx-progressbar';
import { NgProgressHttpModule } from 'ngx-progressbar/http';
import { APP_BASE_HREF, CommonModule, DatePipe } from '@angular/common';
import { MessageService } from 'primeng/api';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms';
import {DropdownModule} from 'primeng/dropdown';
import { ToastModule } from 'primeng/toast';
import { DynamicDialogModule } from 'primeng/dynamicdialog';
import { DialogModule } from 'primeng/dialog';
import { InputNumberModule } from 'primeng/inputnumber';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { GalleriaModule } from 'primeng/galleria';
import { TooltipModule } from 'primeng/tooltip';
import { OverlayPanelModule } from 'primeng/overlaypanel';
import { ApiEndpointInterceptor } from './shared/http-interceptor/api-endpoint.interceptor';
import { AppSecuredComponent } from './app-secured/app-secured.component';
import { SharedModule } from './shared/shared.module';
import { CalendarModule } from 'primeng/calendar';
import { TableModule } from 'primeng/table';
import { RadioButtonModule } from 'primeng/radiobutton';
import { BlockUIModule } from 'primeng/blockui';
import { AgmCoreModule, GoogleMapsAPIWrapper } from '@agm/core';
import { AgmCoreConfig } from './shared/infrastructure/agmCore.config';
import { GeocodingApiService } from './shared/services/api/geocodingApiService.service';
import { HttpModule } from '@angular/http';

@NgModule({
  declarations: [
    AppComponent,
    AppSecuredComponent
  ],
  imports: [
    AgmCoreModule.forRoot(AgmCoreConfig),
    HttpClientModule,
    ToastModule,
    ButtonModule,
    NgProgressModule,
    NgProgressHttpModule,
    BrowserAnimationsModule,
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    DropdownModule,
    DynamicDialogModule,
    DialogModule,
    InputNumberModule,
    AutoCompleteModule,
    GalleriaModule,
    TooltipModule,
    OverlayPanelModule,
    SharedModule,
    CommonModule,
    CalendarModule,
    RadioButtonModule,
    TableModule,
    BlockUIModule,
    HttpModule,
  ],
  providers: [
    GoogleMapsAPIWrapper,
    MessageService,
    { provide: HTTP_INTERCEPTORS, useClass: ApiEndpointInterceptor, multi: true},
    DatePipe
  ],
  bootstrap: [AppComponent],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA,
  ]
})
export class AppModule { }
