import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CorePriceManagementComponent } from './core-price-management.component';

describe('CorePriceManagementComponent', () => {
  let component: CorePriceManagementComponent;
  let fixture: ComponentFixture<CorePriceManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CorePriceManagementComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CorePriceManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
