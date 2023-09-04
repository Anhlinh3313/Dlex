import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateRoutingHubComponent } from './create-update-routing-hub.component';

describe('CreateUpdateRoutingHubComponent', () => {
  let component: CreateUpdateRoutingHubComponent;
  let fixture: ComponentFixture<CreateUpdateRoutingHubComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateUpdateRoutingHubComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdateRoutingHubComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
