import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CorePoHubComponent } from './core-po-hub.component';

describe('CorePoHubComponent', () => {
  let component: CorePoHubComponent;
  let fixture: ComponentFixture<CorePoHubComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CorePoHubComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CorePoHubComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
