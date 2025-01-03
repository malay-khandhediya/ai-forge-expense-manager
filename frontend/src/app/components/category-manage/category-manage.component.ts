import { Component } from '@angular/core';
import { Category } from '../../models/category.model';
import { CategoryService } from '../../services/category.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-category-manage',
  templateUrl: './category-manage.component.html',
     imports: [CommonModule,FormsModule,HttpClientModule],
    providers: [CategoryService, HttpClient],
  
})
export class CategoryManageComponent {
  category: Category = new Category();

  constructor(private categoryService: CategoryService) {}

  addCategory() {
    this.categoryService.addCategory(this.category).subscribe(() => {
      this.category = new Category();
    });
  }

  editCategory() {
    this.categoryService.editCategory(this.category).subscribe(() => {
      this.category = new Category();
    });
  }
}