import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AppComponent } from './app.component';
import { ExpenseEntryComponent } from './components/expense-entry/expense-entry.component';
import { ExpenseListComponent } from './components/expense-list/expense-list.component';
import { ExpenseSummaryComponent } from './components/expense-summary/expense-summary.component';
import { SearchComponent } from './components/search/search.component';
import { HomeComponent } from './components/home/home.component';
import { CategoryListComponent } from './components/category-list/category-list.component';
import { CategoryManageComponent } from './components/category-manage/category-manage.component';
import { AppRoutingModule } from './app-routing.module';

@NgModule({
  declarations: [
    AppComponent,
    ExpenseEntryComponent,
    ExpenseListComponent,
    ExpenseSummaryComponent,
    SearchComponent,
    HomeComponent,
    CategoryListComponent,
    CategoryManageComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    CommonModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }