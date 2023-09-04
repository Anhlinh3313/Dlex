import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CoreRolePagesComponent } from './core-role-pages.component';

describe('CoreRolePagesComponent', () => {
  let component: CoreRolePagesComponent;
  let fixture: ComponentFixture<CoreRolePagesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreRolePagesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoreRolePagesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
