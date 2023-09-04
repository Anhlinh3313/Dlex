import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateReasonComponent } from './create-update-reason.component';

describe('CreateUpdateReasonComponent', () => {
  let component: CreateUpdateReasonComponent;
  let fixture: ComponentFixture<CreateUpdateReasonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateUpdateReasonComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdateReasonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
