import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoreProvinceComponent } from './core-province.component';

describe('CoreProvinceComponent', () => {
  let component: CoreProvinceComponent;
  let fixture: ComponentFixture<CoreProvinceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreProvinceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoreProvinceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
