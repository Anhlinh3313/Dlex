import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CreateUpdateProvinceComponent } from './create-update-province.component';

describe('CoreUserComponent', () => {
  let component: CreateUpdateProvinceComponent;
  let fixture: ComponentFixture<CreateUpdateProvinceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateUpdateProvinceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdateProvinceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
