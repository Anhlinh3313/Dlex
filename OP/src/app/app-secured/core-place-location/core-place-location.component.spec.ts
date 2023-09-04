import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoreGeneralManagementComponent } from './core-general-management.component';

describe('CoreGeneralManagementComponent', () => {
  let component: CoreGeneralManagementComponent;
  let fixture: ComponentFixture<CoreGeneralManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreGeneralManagementComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoreGeneralManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
