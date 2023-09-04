import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateTypeShippingComponent } from './create-update-type-shipping.component';

describe('CreateUpdateTypeShippingComponent', () => {
  let component: CreateUpdateTypeShippingComponent;
  let fixture: ComponentFixture<CreateUpdateTypeShippingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateUpdateTypeShippingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdateTypeShippingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
