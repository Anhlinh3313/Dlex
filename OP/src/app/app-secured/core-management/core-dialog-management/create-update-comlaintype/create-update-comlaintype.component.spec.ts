import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateComlaintypeComponent } from './create-update-comlaintype.component';

describe('CreateUpdateComlaintypeComponent', () => {
  let component: CreateUpdateComlaintypeComponent;
  let fixture: ComponentFixture<CreateUpdateComlaintypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateUpdateComlaintypeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdateComlaintypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
