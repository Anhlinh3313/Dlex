import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CorePriceListSettingComponent } from './core-price-list-setting.component';

describe('CorePriceListSettingComponent', () => {
  let component: CorePriceListSettingComponent;
  let fixture: ComponentFixture<CorePriceListSettingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CorePriceListSettingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CorePriceListSettingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
