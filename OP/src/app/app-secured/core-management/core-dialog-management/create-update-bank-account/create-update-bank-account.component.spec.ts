import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateBankAccountComponent } from './create-update-bank-account.component';

describe('CreateUpdateBankAccountComponent', () => {
  let component: CreateUpdateBankAccountComponent;
  let fixture: ComponentFixture<CreateUpdateBankAccountComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateUpdateBankAccountComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdateBankAccountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
