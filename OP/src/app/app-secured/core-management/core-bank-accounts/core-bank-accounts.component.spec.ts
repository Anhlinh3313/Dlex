import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoreBankAccountsComponent } from './core-bank-accounts.component';

describe('CoreBankAccountsComponent', () => {
  let component: CoreBankAccountsComponent;
  let fixture: ComponentFixture<CoreBankAccountsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreBankAccountsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoreBankAccountsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
