import { Component } from "@angular/core";
import { Expense } from "../../models/expense.model";
import { ExpenseService } from "../../services/expense.service";
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { HttpClient, HttpClientModule } from "@angular/common/http";

@Component({
  selector: 'app-expense-entry',
  templateUrl: './expense-entry.component.html',
  styleUrls: ['./expense-entry.component.css'],
  standalone: true,
      imports: [CommonModule,FormsModule,HttpClientModule],
      providers: [ExpenseService, HttpClient],
  })
export class ExpenseEntryComponent {
  expense: Expense = new Expense(); // Initialize with a new instance of Expense
  isLoading: boolean = false; // To manage loading state
  errorMessage: string | null = null; // To handle errors

  constructor(private expenseService: ExpenseService, private route: ActivatedRoute) {
    this.route.queryParams.subscribe(params => {
      if (params['id']) {
        this.loadExpense(params['id']);
      }
    });
  }

  /**
   * Load an expense entry by ID.
   * @param expenseId - The ID of the expense to load.
   */
  loadExpense(expenseId: number): void {
    this.expenseService.getExpenseById(expenseId).subscribe({
      next: (response) => {
        this.expense = response;
      },
      error: (error) => {
        this.handleError('Failed to load expense. Please try again.');
      }
    });
  }

  /**
   * Add a new expense entry.
   */
  addExpense(): void {
    this.resetError(); // Clear any previous error messages
    this.isLoading = true;

    this.expenseService.addExpense(this.expense).subscribe({
      next: (response) => {
        console.log('Expense added successfully:', response);
        this.resetForm();
      },
      error: (error) => {
        this.handleError('Failed to add expense. Please try again.');
      },
      complete: () => (this.isLoading = false),
    });
  }

  /**
   * Edit an existing expense entry.
   */
  editExpense(): void {
    this.resetError();
    this.isLoading = true;

    this.expenseService.updateExpense(this.expense).subscribe({
      next: (response) => {
        console.log('Expense updated successfully:', response);
      },
      error: (error) => {
        this.handleError('Failed to update expense. Please try again.');
      },
      complete: () => (this.isLoading = false),
    });
  }

  /**
   * Delete an expense entry by ID.
   * @param expenseId - The ID of the expense to delete.
   */
  deleteExpense(expenseId: number): void {
    this.resetError();
    this.isLoading = true;

    this.expenseService.deleteExpense(expenseId).subscribe({
      next: (response) => {
        console.log('Expense deleted successfully:', response);
        this.resetForm();
      },
      error: (error) => {
        this.handleError('Failed to delete expense. Please try again.');
      },
      complete: () => (this.isLoading = false),
    });
  }

  /**
   * Handle form submission.
   */
  onSubmit(): void {
    if (this.expense.id) {
      this.editExpense();
    } else {
      this.addExpense();
    }
  }

  /**
   * Reset the form to its initial state.
   */
  resetForm(): void {
    this.expense = new Expense();
  }

  /**
   * Reset the error message.
   */
  resetError(): void {
    this.errorMessage = null;
  }

  /**
   * Handle errors by setting the error message.
   * @param message - The error message to set.
   */
  handleError(message: string): void {
    this.errorMessage = message;
  }
}
