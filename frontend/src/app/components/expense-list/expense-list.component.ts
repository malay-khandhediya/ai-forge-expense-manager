import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Expense } from '../../models/expense.model';
import { ExpenseService } from '../../services/expense.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-expense-list',
  templateUrl: './expense-list.component.html',
  styleUrls: ['./expense-list.component.css'],
  imports: [CommonModule,FormsModule,HttpClientModule],
    providers: [ExpenseService, HttpClient],
})
export class ExpenseListComponent implements OnInit {
  expenses: Expense[] = [];

  constructor(private expenseService: ExpenseService, private router: Router) {}

  ngOnInit() {
    this.loadExpenses();
  }

  loadExpenses() {
    this.expenseService.getExpenses().subscribe((data: Expense[]) => {
      this.expenses = data;
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
}