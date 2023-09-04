import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateStationHubComponent } from './create-update-station-hub.component';

describe('CreateUpdateStationHubComponent', () => {
  let component: CreateUpdateStationHubComponent;
  let fixture: ComponentFixture<CreateUpdateStationHubComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateUpdateStationHubComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdateStationHubComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
