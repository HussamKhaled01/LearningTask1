import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BusinessCardDetails } from './business-card-details';

describe('BusinessCardDetails', () => {
  let component: BusinessCardDetails;
  let fixture: ComponentFixture<BusinessCardDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BusinessCardDetails],
    }).compileComponents();

    fixture = TestBed.createComponent(BusinessCardDetails);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
