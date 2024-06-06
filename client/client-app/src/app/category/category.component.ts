import { Component } from '@angular/core';
import { Category } from '../models/category.model';
import { CategoryService } from '../services/category.service';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent {
  newCategory: Partial<Category> = {
    name: ''
  };

  constructor(private categoryService: CategoryService) { }

  createCategory(): void {
    if (!this.newCategory.name) {
      console.error('Please provide a name for the category.');
      return;
    }

    this.categoryService.createCategory(this.newCategory as Category)
      .subscribe(
        response => {
          console.log('Category created successfully:', response);
          this.newCategory = { name: '' }; // Clear the form
        },
        error => {
          console.error('Error creating category:', error);
        }
      );
  }
}
