import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CoreDistrictComponent } from './core-district.component';


describe('CoreDistrictComponent', () => {
  let component: CoreDistrictComponent;
  let fixture: ComponentFixture<CoreDistrictComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreDistrictComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoreDistrictComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
