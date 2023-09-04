import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CoreWardComponent } from './core-ward.component';


describe('CoreWardComponent', () => {
  let component: CoreWardComponent;
  let fixture: ComponentFixture<CoreWardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreWardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoreWardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
