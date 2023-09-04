import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoreReasonManagementComponent } from './core-reason-management.component';

describe('CoreReasonManagementComponent', () => {
  let component: CoreReasonManagementComponent;
  let fixture: ComponentFixture<CoreReasonManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreReasonManagementComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoreReasonManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
