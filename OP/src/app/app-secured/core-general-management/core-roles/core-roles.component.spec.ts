import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CoreRolesComponent } from './core-roles.component';

describe('CoreRolesComponent', () => {
  let component: CoreRolesComponent;
  let fixture: ComponentFixture<CoreRolesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreRolesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoreRolesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
