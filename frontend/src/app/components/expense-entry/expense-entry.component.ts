import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ExpenseService } from '../../services/expense.service';
import { CategoryService } from '../../services/category.service';
import { Expense } from '../../models/expense.model';
import { Category } from '../../models/category.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-expense-entry',
  templateUrl: './expense-entry.component.html',
  styleUrls: ['./expense-entry.component.css'],
  standalone: true,
      imports: [CommonModule,FormsModule,HttpClientModule],
      providers: [ExpenseService,CategoryService, HttpClient],
  
})
export class ExpenseEntryComponent implements OnInit {
  expense: Expense = new Expense();
  categories: Category[] = [];
  isEditMode: boolean = false;

  constructor(
    private expenseService: ExpenseService,
    private categoryService: CategoryService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.loadExpense(params['id']);
      } else {
        this.isEditMode = false;
      }
    });
    this.loadCategories();
  }

  loadExpense(id: number) {
    this.expenseService.getExpenseById(id).subscribe((data: Expense) => {
      this.expense = data;
    });
  }

  loadCategories() {
    this.categoryService.getCategories().subscribe((data: Category[]) => {
      this.categories = data;
    });
  }

  onSubmit() {
    if (this.isEditMode) {
      this.editExpense();
    } else {
      this.addExpense();
    }
  }

  addExpense() {
    this.expenseService.addExpense(this.expense).subscribe(() => {
      this.router.navigate(['/expense-list']);
    });
  }

  editExpense() {
    this.expenseService.updateExpense(this.expense).subscribe(() => {
      this.router.navigate(['/expense-list']);
    });
  }
}