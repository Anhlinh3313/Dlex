import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoreCustomerInfoLogComponent } from './core-customer-info-log.component';

describe('CoreCustomerInfoLogComponent', () => {
  let component: CoreCustomerInfoLogComponent;
  let fixture: ComponentFixture<CoreCustomerInfoLogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreCustomerInfoLogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoreCustomerInfoLogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
