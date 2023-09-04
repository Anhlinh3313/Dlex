import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoreUserRelationComponent } from './core-user-relation.component';

describe('CoreUserRelationComponent', () => {
  let component: CoreUserRelationComponent;
  let fixture: ComponentFixture<CoreUserRelationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoreUserRelationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoreUserRelationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
