import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoreHubManagementComponent } from './core-hub-management.component';

describe('CoreHubManagementComponent', () => {
  let component: CoreHubManagementComponent;
  let fixture: ComponentFixture<CoreHubManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreHubManagementComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoreHubManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
