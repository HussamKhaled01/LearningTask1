import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BusinessCardList } from './business-card-list';

describe('BusinessCardList', () => {
  let component: BusinessCardList;
  let fixture: ComponentFixture<BusinessCardList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BusinessCardList],
    }).compileComponents();

    fixture = TestBed.createComponent(BusinessCardList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
