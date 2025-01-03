import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { Expense } from '../models/expense.model';

@Injectable({
  providedIn: 'root'
})
export class ExpenseService {
  private apiUrl = 'http://localhost:5034/api/Expense'; // Replace with your API URL

  constructor(private http: HttpClient) {}

  // Get all expenses
  getExpenses(): Observable<Expense[]> {
    return this.http.get<{ $id: string; $values: Expense[] }>(this.apiUrl).pipe(
      map(response => response.$values) // Extract the $values array
    );
  }

  // Get a single expense by ID
  getExpenseById(id: number): Observable<Expense> {
    return this.http.get<any>(`${this.apiUrl}/${id}`).pipe(
      map(response => response) // Extract the first item from $values
    );
  }

  // Add a new expense
  addExpense(expense: Expense): Observable<Expense> {
    return this.http.post<{ $id: string; $values: Expense[] }>(this.apiUrl, expense).pipe(
      map(response => response.$values[0]) // Extract the first item from $values
    );
  }

  // Update an existing expense
  updateExpense(expense: Expense): Observable<Expense> {
    return this.http.put<any>(
      `${this.apiUrl}/${expense.expenseId}`,
      expense
    ).pipe(
      map(response => response) // Extract the first item from $values
    );
  }

  // Delete an expense by ID
  deleteExpense(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  // Search expenses
  searchExpenses(query: string): Observable<Expense[]> {
    return this.http.get<{ $id: string; $values: Expense[] }>(`${this.apiUrl}?search=${query}`).pipe(
      map(response => response.$values) // Extract the $values array
    );
  }
}