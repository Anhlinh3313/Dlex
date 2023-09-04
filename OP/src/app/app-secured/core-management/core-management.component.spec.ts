import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoreManagementComponent } from './core-management.component';

describe('CoreManagementComponent', () => {
  let component: CoreManagementComponent;
  let fixture: ComponentFixture<CoreManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreManagementComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoreManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
