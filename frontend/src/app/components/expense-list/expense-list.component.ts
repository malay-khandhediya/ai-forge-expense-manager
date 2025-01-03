import { Component } from '@angular/core';
import { ExpenseService } from '../../services/expense.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule, HttpHandler } from '@angular/common/http';

@Component({
  selector: 'app-expense-list',
  templateUrl: './expense-list.component.html',
  styleUrls: ['./expense-list.component.css'],
  standalone: true,
    imports: [CommonModule,FormsModule,HttpClientModule],
    providers: [ExpenseService, HttpClient],
})
export class ExpenseListComponent {
  expenses: any[] = [];

  constructor(private expenseService: ExpenseService, private router: Router) {}

  ngOnInit() {
    this.getExpenses();
  }

  getExpenses() {
    this.expenseService.getExpenses().subscribe((data: any[]) => {
      this.expenses = data;
    });
  }

  deleteExpense(id: number) {
    this.expenseService.deleteExpense(id).subscribe(() => {
      this.getExpenses();
    });
  }

  editExpense(expense: any) {
    this.router.navigate(['/expense-entry'], { queryParams: { id: expense.id } });
  }
}