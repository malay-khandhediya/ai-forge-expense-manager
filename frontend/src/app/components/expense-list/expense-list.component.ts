import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Expense } from '../../models/expense.model';
import { ExpenseService } from '../../services/expense.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { CategoryService } from '../../services/category.service';
import { Category } from '../../models/category.model';

@Component({
  selector: 'app-expense-list',
  templateUrl: './expense-list.component.html',
  styleUrls: ['./expense-list.component.css'],
  imports: [CommonModule,FormsModule,HttpClientModule],
    providers: [ExpenseService,CategoryService, HttpClient],
})
export class ExpenseListComponent implements OnInit {
  expenses: Expense[] = [];
  categories: Category[] = [];

  constructor(private expenseService: ExpenseService,    private categoryService: CategoryService,
  private router: Router) {}

  ngOnInit() {
    this.loadCategories();
    this.loadExpenses();
  }

  loadExpenses() {
    this.expenseService.getExpenses().subscribe((data: Expense[]) => {
      this.expenses = data;
      console.log(this.expenses)
    });
  }

  deleteExpense(id: number) {
    this.expenseService.deleteExpense(id).subscribe(() => {
      this.loadExpenses();
    });
  }

  navigateToAddExpense() {
    this.router.navigate(['/expense-entry']);
  }

  navigateToEditExpense(id: number) {
    this.router.navigate(['/expense-entry'], { queryParams: { id: id } });
  }

  
    loadCategories() {
      this.categoryService.getCategories().subscribe((data: Category[]) => {
        this.categories = data;
      });
    }
    getcategoryname(categoryId :number){
      return this.categories.find(x=>x.categoryId == categoryId)?.name;
    }
}