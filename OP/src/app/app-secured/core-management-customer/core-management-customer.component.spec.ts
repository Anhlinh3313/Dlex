import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoreManagementCustomerComponent } from './core-management-customer.component';

describe('CoreManagementCustomerComponent', () => {
  let component: CoreManagementCustomerComponent;
  let fixture: ComponentFixture<CoreManagementCustomerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreManagementCustomerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoreManagementCustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
