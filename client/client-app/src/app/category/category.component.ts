import { Component } from '@angular/core';
import { Category } from '../models/category.model';
import { CategoryService } from '../services/category.service';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent {
  newCategory: Partial<Category> = { name: ''};
  error: string = '';
  success: string = '';
  
  constructor(private categoryService: CategoryService) { }

  createCategory(): void {
    this.categoryService.createCategory(this.newCategory as Category)
      .subscribe(
        (response) => {
          this.error = '';
          this.newCategory = { name: '' }; // Clear the form
          this.success = 'Category created successfully';
        },
        (error) => {
          this.error = error.error.errorMessage;
        }
      );
  }
}
