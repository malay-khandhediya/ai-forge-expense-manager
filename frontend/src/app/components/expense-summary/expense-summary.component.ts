import { Component } from '@angular/core';
import { ExpenseService } from '../../services/expense.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-expense-summary',
  templateUrl: './expense-summary.component.html',
  styleUrls: ['./expense-summary.component.css'],
    imports: [CommonModule,FormsModule],
  
})
export class ExpenseSummaryComponent {
  expenses: any[] = [];
  summary: { [key: string]: number } = {};
  expenseCategories: string[] = [];

  constructor(private expenseService: ExpenseService) {}

  ngOnInit() {
    this.loadExpenses();
  }

  loadExpenses() {
    this.expenseService.getExpenses().subscribe((data: any[]) => {
      this.expenses = data;
      this.calculateSummary();
      this.extractCategories();
    });
  }

  calculateSummary() {
    this.summary = {};
    this.expenses.forEach(expense => {
      const category = expense.category;
      if (!this.summary[category]) {
        this.summary[category] = 0;
      }
      this.summary[category] += expense.amount;
    });
  }

  extractCategories() {
    this.expenseCategories = [...new Set(this.expenses.map(expense => expense.category))];
  }
}