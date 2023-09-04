import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoreTypeCargoComponent } from './core-type-cargo.component';

describe('CoreTypeCargoComponent', () => {
  let component: CoreTypeCargoComponent;
  let fixture: ComponentFixture<CoreTypeCargoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreTypeCargoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoreTypeCargoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
