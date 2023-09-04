import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoreRoutingHubComponent } from './core-routing-hub.component';

describe('CoreRoutingHubComponent', () => {
  let component: CoreRoutingHubComponent;
  let fixture: ComponentFixture<CoreRoutingHubComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreRoutingHubComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoreRoutingHubComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
