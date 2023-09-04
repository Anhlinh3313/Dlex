import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoreDeductPriceComponent } from './core-deduct-price.component';

describe('CoreDeductPriceComponent', () => {
  let component: CoreDeductPriceComponent;
  let fixture: ComponentFixture<CoreDeductPriceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreDeductPriceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoreDeductPriceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
