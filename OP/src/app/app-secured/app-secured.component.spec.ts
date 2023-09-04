import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppSecuredComponent } from './app-secured.component';

describe('AppSecuredComponent', () => {
  let component: AppSecuredComponent;
  let fixture: ComponentFixture<AppSecuredComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AppSecuredComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AppSecuredComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
