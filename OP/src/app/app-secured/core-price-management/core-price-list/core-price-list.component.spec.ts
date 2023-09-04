import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CorePriceListComponent } from './core-price-list.component';

describe('CorePriceListComponent', () => {
  let component: CorePriceListComponent;
  let fixture: ComponentFixture<CorePriceListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CorePriceListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CorePriceListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
