import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoreTypeShippingComponent } from './core-type-shipping.component';

describe('CoreTypeShippingComponent', () => {
  let component: CoreTypeShippingComponent;
  let fixture: ComponentFixture<CoreTypeShippingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreTypeShippingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoreTypeShippingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
