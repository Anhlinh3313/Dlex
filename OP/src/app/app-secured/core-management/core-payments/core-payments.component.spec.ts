import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CorePaymentsComponent } from './core-payments.component';

describe('CorePaymentsComponent', () => {
  let component: CorePaymentsComponent;
  let fixture: ComponentFixture<CorePaymentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CorePaymentsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CorePaymentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
