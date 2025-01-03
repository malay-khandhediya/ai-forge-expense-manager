import { Routes } from '@angular/router';
import { ExpenseEntryComponent } from './components/expense-entry/expense-entry.component';
import { ExpenseListComponent } from './components/expense-list/expense-list.component';
import { ExpenseSummaryComponent } from './components/expense-summary/expense-summary.component';
import { SearchComponent } from './components/search/search.component';
import { HomeComponent } from './components/home/home.component';
import { CategoryListComponent } from './components/category-list/category-list.component';
import { CategoryManageComponent } from './components/category-manage/category-manage.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'expense-entry', component: ExpenseEntryComponent },
  { path: 'expense-list', component: ExpenseListComponent },
  { path: 'category-list', component: CategoryListComponent },
  { path: 'category-manage', component: CategoryManageComponent },
  { path: 'expense-summary', component: ExpenseSummaryComponent },
  { path: 'search', component: SearchComponent }
];