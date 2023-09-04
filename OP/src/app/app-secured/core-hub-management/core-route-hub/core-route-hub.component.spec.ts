import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoreRouteHubComponent } from './core-route-hub.component';

describe('CoreRouteHubComponent', () => {
  let component: CoreRouteHubComponent;
  let fixture: ComponentFixture<CoreRouteHubComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreRouteHubComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoreRouteHubComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
