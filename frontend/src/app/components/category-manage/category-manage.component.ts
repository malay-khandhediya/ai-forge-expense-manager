import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoryService } from '../../services/category.service';
import { Category } from '../../models/category.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-category-manage',
  templateUrl: './category-manage.component.html',
  imports: [CommonModule,FormsModule,HttpClientModule],
  providers: [CategoryService, HttpClient],
})
export class CategoryManageComponent implements OnInit {
  category: Category = new Category();
  isEditMode: boolean = false;

  constructor(
    private categoryService: CategoryService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.loadCategory(params['id']);
      } else {
        this.isEditMode = false;
      }
    });
  }

  loadCategory(id: number) {
    this.categoryService.getCategoryById(id).subscribe((data: Category) => {
      this.category = data;
    });
  }

  saveCategory() {
    if (this.isEditMode) {
      this.editCategory();
    } else {
      this.addCategory();
    }
  }

  addCategory() {
    this.categoryService.addCategory(this.category).subscribe(() => {
      this.router.navigate(['/category-list']);
    });
  }

  editCategory() {
    this.categoryService.editCategory(this.category).subscribe(() => {
      this.router.navigate(['/category-list']);
    });
  }
}