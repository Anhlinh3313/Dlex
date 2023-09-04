import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoreCountryComponent } from './core-country.component';

describe('CoreCountryComponent', () => {
  let component: CoreCountryComponent;
  let fixture: ComponentFixture<CoreCountryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreCountryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoreCountryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
