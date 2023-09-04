import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ComponentsModule } from './components/components.module';
import { GoogleplaceDirective } from './directives/google-place.directive';
import { environment } from 'src/environments/environment';
import { AgmCoreModule } from '@agm/core';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    GoogleplaceDirective,
  ],
  imports: [
    AgmCoreModule.forRoot({
      apiKey: environment.gMapKey,
      libraries: ["places"],
      language: 'vi',
      region: 'VN'
    }),
    ComponentsModule,
    CommonModule,
    FormsModule
  ],
  exports: [
    ComponentsModule,
    GoogleplaceDirective,
  ],
})
export class SharedModule { }
