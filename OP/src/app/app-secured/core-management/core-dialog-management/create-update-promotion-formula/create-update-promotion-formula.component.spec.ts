import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdatePromotionFormulaComponent } from './create-update-promotion-formula.component';

describe('CreateUpdatePromotionFormulaComponent', () => {
  let component: CreateUpdatePromotionFormulaComponent;
  let fixture: ComponentFixture<CreateUpdatePromotionFormulaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateUpdatePromotionFormulaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdatePromotionFormulaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
