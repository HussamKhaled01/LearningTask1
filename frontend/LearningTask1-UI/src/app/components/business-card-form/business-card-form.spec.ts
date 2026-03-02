import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BusinessCardForm } from './business-card-form';

describe('BusinessCardForm', () => {
  let component: BusinessCardForm;
  let fixture: ComponentFixture<BusinessCardForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BusinessCardForm],
    }).compileComponents();

    fixture = TestBed.createComponent(BusinessCardForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
