import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateTypeCargoComponent } from './create-update-type-cargo.component';

describe('CreateUpdateTypeCargoComponent', () => {
  let component: CreateUpdateTypeCargoComponent;
  let fixture: ComponentFixture<CreateUpdateTypeCargoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateUpdateTypeCargoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdateTypeCargoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
