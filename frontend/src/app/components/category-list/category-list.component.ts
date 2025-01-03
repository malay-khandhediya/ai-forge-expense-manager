import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Category } from '../../models/category.model';
import { CategoryService } from '../../services/category.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  imports: [CommonModule, FormsModule, HttpClientModule],
  providers: [CategoryService, HttpClient],
})
export class CategoryListComponent implements OnInit {
  categories: Category[] = [];

  constructor(private categoryService: CategoryService, private router: Router) {}

  ngOnInit() {
    this.loadCategories();
  }

  loadCategories() {
    debugger
    this.categoryService.getCategories().subscribe((data: Category[]) => {
      this.categories = data;
    });
  }

  deleteCategory(id: number) {
    this.categoryService.deleteCategory(id).subscribe(() => {
      this.loadCategories();
    });
  }

  navigateToAddCategory() {
    this.router.navigate(['/category-manage']);
  }

  navigateToEditCategory(id: number) {
    this.router.navigate(['/category-manage'], { queryParams: { id: id } });
  }
}