import { Component, OnInit } from '@angular/core';
import { ConstructionCompany } from '../models/constructionCompany.model';
import { ConstructionCompanyService } from '../services/constructionCompany.service';

@Component({
  selector: 'app-construction-company',
  templateUrl: './construction-company.component.html',
  styleUrls: ['./construction-company.component.css']
})
export class ConstructionCompanyComponent implements OnInit {

  newConstructionCompany: Partial<ConstructionCompany> = { name: '' };
  updateCompanyName: Partial<ConstructionCompany> = { name: '' };
  existingConstructionCompanyName: string | null = null;
  errorMessage: string | null = null;
  successMessage: string | null = null;

  constructor(private constructionCompanyService: ConstructionCompanyService) { }

  ngOnInit(): void {
    this.loadExistingConstructionCompany();
  }

  createConstructionCompany(): void {
    if (!this.newConstructionCompany.name) {
      this.errorMessage = 'Please provide a name for the construction company.';
      return;
    }

    this.constructionCompanyService.createConstructionCompany(this.newConstructionCompany as ConstructionCompany)
      .subscribe(
        response => {
          console.log('Construction company created successfully:', response);
          this.existingConstructionCompanyName = response.name;
          this.errorMessage = null;
          this.successMessage = 'Construction company created successfully.';
          this.resetForm();
        },
        error => {
          console.error('Error creating construction company', error);
          this.errorMessage = 'Error creating construction company: ' + error.error.message;
        }
      );
  }

  updateConstructionCompanyName(): void {
    if (!this.updateCompanyName.name) {
      console.error('Please choose another name.');
      return;
    }

    this.constructionCompanyService.updateConstructionCompanyName(this.updateCompanyName as ConstructionCompany)
      .subscribe(
        response => {
          console.log('Construction company name updated successfully', response);
        },
        error => {
          console.error('Error updating construction company name', error);
        }
      );
  }

  loadExistingConstructionCompany(): void {
    this.constructionCompanyService.getConstructionCompany()
      .subscribe(
        response => {
          this.existingConstructionCompanyName = response.name;
        },
        error => {
          console.error('Error loading construction company', error);
        }
      );
  }

  resetForm(): void {
    this.newConstructionCompany = { name: '' };
  }
}
