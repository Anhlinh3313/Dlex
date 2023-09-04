import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoreTypeOfComplainComponent } from './core-type-of-complain.component';

describe('CoreTypeOfComplainComponent', () => {
  let component: CoreTypeOfComplainComponent;
  let fixture: ComponentFixture<CoreTypeOfComplainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreTypeOfComplainComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoreTypeOfComplainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
