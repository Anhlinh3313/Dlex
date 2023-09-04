import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CorePriceFormulaComponent } from './core-price-formula.component';

describe('CorePriceFormulaComponent', () => {
  let component: CorePriceFormulaComponent;
  let fixture: ComponentFixture<CorePriceFormulaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CorePriceFormulaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CorePriceFormulaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
