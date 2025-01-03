import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AppComponent } from './app.component';
import { ExpenseEntryComponent } from './components/expense-entry/expense-entry.component';
import { ExpenseListComponent } from './components/expense-list/expense-list.component';
import { ExpenseSummaryComponent } from './components/expense-summary/expense-summary.component';
import { SearchComponent } from './components/search/search.component';
import { CategoryListComponent } from './components/category-list/category-list.component';
import { CategoryManageComponent } from './components/category-manage/category-manage.component';
import { HomeComponent } from './components/home/home.component';
import { AppRoutingModule } from './app-routing.module';
import { ExpenseService } from './services/expense.service';

@NgModule({
  declarations: [
    AppComponent,
    ExpenseEntryComponent,
    ExpenseListComponent,
    ExpenseSummaryComponent,
    SearchComponent,
    CategoryListComponent,
    CategoryManageComponent,
    HomeComponent
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    FormsModule,
    CommonModule,
    
  ],
  providers: [ExpenseService],
  bootstrap: [AppComponent]
})
export class AppModule { }