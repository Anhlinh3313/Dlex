import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateFormulaComponent } from './create-update-formula.component';

describe('CreateUpdateFormulaComponent', () => {
  let component: CreateUpdateFormulaComponent;
  let fixture: ComponentFixture<CreateUpdateFormulaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateUpdateFormulaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdateFormulaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
