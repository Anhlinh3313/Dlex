import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CorePrintFormManagementComponent } from './core-print-form-management.component';

describe('CorePrintFormManagementComponent', () => {
  let component: CorePrintFormManagementComponent;
  let fixture: ComponentFixture<CorePrintFormManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CorePrintFormManagementComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CorePrintFormManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
