import { Routes } from '@angular/router';
import { BusinessCardList } from './components/business-card-list/business-card-list';
import { BusinessCardDetails } from './components/business-card-details/business-card-details';
import { BusinessCardForm } from './components/business-card-form/business-card-form';

export const routes: Routes = [
  { path: '', component: BusinessCardList },
  { path: 'create', component: BusinessCardForm },
  { path: 'edit/:id', component: BusinessCardForm },
  { path: 'details/:id', component: BusinessCardDetails }
];
