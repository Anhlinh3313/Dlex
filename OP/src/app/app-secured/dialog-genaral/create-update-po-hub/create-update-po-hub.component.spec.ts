import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdatePoHubComponent } from './create-update-po-hub.component';

describe('CreateUpdatePoHubComponent', () => {
  let component: CreateUpdatePoHubComponent;
  let fixture: ComponentFixture<CreateUpdatePoHubComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateUpdatePoHubComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdatePoHubComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
