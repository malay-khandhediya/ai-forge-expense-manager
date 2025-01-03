import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ExpenseEntryComponent } from './components/expense-entry/expense-entry.component';
import { ExpenseListComponent } from './components/expense-list/expense-list.component';
import { ExpenseSummaryComponent } from './components/expense-summary/expense-summary.component';
import { SearchComponent } from './components/search/search.component';
import { CategoryListComponent } from './components/category-list/category-list.component';
import { CategoryManageComponent } from './components/category-manage/category-manage.component';

const routes: Routes = [
  { path: 'expense-entry', component: ExpenseEntryComponent },
  { path: 'expense-list', component: ExpenseListComponent },
  { path: 'expense-summary', component: ExpenseSummaryComponent },
  { path: 'search', component: SearchComponent },
  { path: 'category-list', component: CategoryListComponent },
  { path: 'category-manage', component: CategoryManageComponent },
  { path: '', redirectTo: '/expense-list', pathMatch: 'full' },
  { path: '**', redirectTo: '/expense-list' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }