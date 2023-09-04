import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoreHolidayManagementComponent } from './core-holiday-management.component';

describe('CoreHolidayManagementComponent', () => {
  let component: CoreHolidayManagementComponent;
  let fixture: ComponentFixture<CoreHolidayManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreHolidayManagementComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoreHolidayManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
