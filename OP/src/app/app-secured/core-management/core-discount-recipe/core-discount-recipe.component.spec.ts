import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoreDiscountRecipeComponent } from './core-discount-recipe.component';

describe('CoreDiscountRecipeComponent', () => {
  let component: CoreDiscountRecipeComponent;
  let fixture: ComponentFixture<CoreDiscountRecipeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreDiscountRecipeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoreDiscountRecipeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
