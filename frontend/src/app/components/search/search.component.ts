import { Component } from '@angular/core';
import { ExpenseService } from '../../services/expense.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css'],
  imports: [CommonModule,FormsModule],
  
})
export class SearchComponent {
  searchQuery: string = '';
  results: any[] = [];

  constructor(private expenseService: ExpenseService) {}

  onSearch() {
    if (this.searchQuery) {
      this.expenseService.searchExpenses(this.searchQuery).subscribe((data: any[]) => {
        this.results = data;
      });
    } else {
      this.results = [];
    }
  }
}