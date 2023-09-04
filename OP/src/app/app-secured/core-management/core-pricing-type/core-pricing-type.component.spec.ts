import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CorePricingTypeComponent } from './core-pricing-type.component';

describe('CorePricingTypeComponent', () => {
  let component: CorePricingTypeComponent;
  let fixture: ComponentFixture<CorePricingTypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CorePricingTypeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CorePricingTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
