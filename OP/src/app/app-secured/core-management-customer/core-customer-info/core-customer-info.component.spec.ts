import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoreCustomerInfoComponent } from './core-customer-info.component';

describe('CoreCustomerInfoComponent', () => {
  let component: CoreCustomerInfoComponent;
  let fixture: ComponentFixture<CoreCustomerInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreCustomerInfoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoreCustomerInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
