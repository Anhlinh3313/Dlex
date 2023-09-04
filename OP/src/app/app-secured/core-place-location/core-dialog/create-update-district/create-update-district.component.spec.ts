import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CreateUpdateDistrictComponent } from './create-update-district.component';

describe('CoreUserComponent', () => {
  let component: CreateUpdateDistrictComponent;
  let fixture: ComponentFixture<CreateUpdateDistrictComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateUpdateDistrictComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdateDistrictComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
