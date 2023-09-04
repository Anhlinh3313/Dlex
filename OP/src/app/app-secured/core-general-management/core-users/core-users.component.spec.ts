import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoreUsersComponent } from './core-users.component';

describe('CoreUserComponent', () => {
  let component: CoreUsersComponent;
  let fixture: ComponentFixture<CoreUsersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreUsersComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoreUsersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
