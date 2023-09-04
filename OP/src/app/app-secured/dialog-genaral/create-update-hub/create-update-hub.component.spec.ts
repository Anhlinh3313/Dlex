import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateHubComponent } from './create-update-hub.component';

describe('CreateUpdateHubComponent', () => {
  let component: CreateUpdateHubComponent;
  let fixture: ComponentFixture<CreateUpdateHubComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateUpdateHubComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdateHubComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
