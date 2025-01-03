import { Component } from '@angular/core';
import { ExpenseService } from '../../services/expense.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Expense } from '../../models/expense.model';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css'],
  imports: [CommonModule,FormsModule],
  
})
export class SearchComponent {
  searchQuery: string = '';
  results: Expense[] = [];

  constructor(private expenseService: ExpenseService) {}

  searchExpenses() {
    if (this.searchQuery) {
      this.expenseService.searchExpenses(this.searchQuery).subscribe((data: Expense[]) => {
        this.results = data;
      });
    } else {
      this.results = [];
    }
  }
}