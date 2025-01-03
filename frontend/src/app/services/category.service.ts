import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { Category } from '../models/category.model';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private apiUrl = 'http://localhost:5034/api/Category'; // Replace with your API URL

  constructor(private http: HttpClient) {}

  getCategories(): Observable<Category[]> {
    return this.http.get<{ $id: string; $values: Category[] }>(this.apiUrl).pipe(
      map(response => response.$values) // Extract the $values array
    );
  }

  // Get a single category by ID
  getCategoryById(id: number): Observable<Category> {
    return this.http.get<any>(`${this.apiUrl}/${id}`).pipe(
      map(response => response) // Extract the first item from $values
    );
  }

  // Add a new category
  addCategory(category: Category): Observable<Category> {
    return this.http.post<any>(this.apiUrl, category).pipe(
      map(response => response) // Extract the first item from $values
    );
  }

  // Edit an existing category
  editCategory(category: Category): Observable<Category> {
    return this.http.put<any>(
      `${this.apiUrl}/${category.categoryId}`,
      category
    ).pipe(
      map(response => response) // Extract the first item from $values
    );
  }

  // Delete a category by ID
  deleteCategory(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}