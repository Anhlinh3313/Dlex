import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoreCopyPriceComponent } from './core-copy-price.component';

describe('CoreCopyPriceComponent', () => {
  let component: CoreCopyPriceComponent;
  let fixture: ComponentFixture<CoreCopyPriceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreCopyPriceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoreCopyPriceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
