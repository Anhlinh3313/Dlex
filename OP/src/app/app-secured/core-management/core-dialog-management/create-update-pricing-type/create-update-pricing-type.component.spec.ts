import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdatePricingTypeComponent } from './create-update-pricing-type.component';

describe('CreateUpdatePricingTypeComponent', () => {
  let component: CreateUpdatePricingTypeComponent;
  let fixture: ComponentFixture<CreateUpdatePricingTypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateUpdatePricingTypeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdatePricingTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
