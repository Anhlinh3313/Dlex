import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateOrUpdateCustomerInfoLogComponent } from './create-or-update-customer-info-log.component';

describe('CreateOrUpdateCustomerInfoLogComponent', () => {
  let component: CreateOrUpdateCustomerInfoLogComponent;
  let fixture: ComponentFixture<CreateOrUpdateCustomerInfoLogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateOrUpdateCustomerInfoLogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateOrUpdateCustomerInfoLogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
