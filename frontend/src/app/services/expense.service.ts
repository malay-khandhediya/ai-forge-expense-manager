import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { Expense } from "../models/expense.model";
import { Injectable } from "@angular/core";
@Injectable()
export class ExpenseService {
  private apiUrl = 'https://api.example.com/expenses'; // Replace with your API URL

  constructor(private http: HttpClient) {}

  getExpenses(): Observable<Expense[]> {
    return this.http.get<Expense[]>(this.apiUrl);
  }

  addExpense(expense: Expense): Observable<Expense> {
    return this.http.post<Expense>(this.apiUrl, expense);
  }

  updateExpense(expense: Expense): Observable<Expense> {
    return this.http.put<Expense>(`${this.apiUrl}/${expense.id}`, expense);
  }

  deleteExpense(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  searchExpenses(query: string): Observable<Expense[]> {
    return this.http.get<Expense[]>(`${this.apiUrl}?search=${query}`);
  }
  getExpenseById(id: number): Observable<Expense> {
    return this.http.get<Expense>(`${this.apiUrl}/${id}`);
  }

}
