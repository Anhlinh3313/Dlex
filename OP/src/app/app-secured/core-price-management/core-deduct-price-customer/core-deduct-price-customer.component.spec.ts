import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoreDeductPriceCustomerComponent } from './core-deduct-price-customer.component';

describe('CoreDeductPriceCustomerComponent', () => {
  let component: CoreDeductPriceCustomerComponent;
  let fixture: ComponentFixture<CoreDeductPriceCustomerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreDeductPriceCustomerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoreDeductPriceCustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
