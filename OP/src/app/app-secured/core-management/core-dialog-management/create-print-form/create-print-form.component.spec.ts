import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatePrintFormComponent } from './create-print-form.component';

describe('CreatePrintFormComponent', () => {
  let component: CreatePrintFormComponent;
  let fixture: ComponentFixture<CreatePrintFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreatePrintFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreatePrintFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
