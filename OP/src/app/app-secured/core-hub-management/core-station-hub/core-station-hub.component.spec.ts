import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoreStationHubComponent } from './core-station-hub.component';

describe('CoreStationHubComponent', () => {
  let component: CoreStationHubComponent;
  let fixture: ComponentFixture<CoreStationHubComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreStationHubComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoreStationHubComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
