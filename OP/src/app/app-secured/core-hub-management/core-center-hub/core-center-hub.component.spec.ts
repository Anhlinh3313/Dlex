import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoreCenterHubComponent } from './core-center-hub.component';

describe('CoreCenterHubComponent', () => {
  let component: CoreCenterHubComponent;
  let fixture: ComponentFixture<CoreCenterHubComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreCenterHubComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoreCenterHubComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
