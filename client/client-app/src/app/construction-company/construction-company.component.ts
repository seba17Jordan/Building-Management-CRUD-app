import { Component, OnInit } from '@angular/core';
import { ConstructionCompany } from '../models/constructionCompany.model';
import { ConstructionCompanyService } from '../services/constructionCompany.service';

@Component({
  selector: 'app-construction-company',
  templateUrl: './construction-company.component.html',
  styleUrls: ['./construction-company.component.css']
})
export class ConstructionCompanyComponent implements OnInit {

  newConstructionCompany: ConstructionCompany = { name: '' };
  updateCompanyName: ConstructionCompany = { name: '' };
  existingConstructionCompanyName: string ='';
  errorCreate: string ='';
  errorUpdate: string ='';
  successCreate: string ='';
  successUpdate: string ='';
  constructionCompanyName: string = '';

  constructor(private constructionCompanyService: ConstructionCompanyService) { }

  ngOnInit(): void {
    this.loadExistingConstructionCompany();
  }

  createConstructionCompany(): void {
    this.newConstructionCompany.name = this.constructionCompanyName;
    console.log(this.constructionCompanyName);
    this.constructionCompanyService.createConstructionCompany(this.newConstructionCompany)
      .subscribe(
        response => {
          this.successCreate = 'Construction company created successfully'; 
          this.errorCreate = '';
          this.existingConstructionCompanyName = response.name;
          this.resetForm();
        },
        error => {
          console.log(error.error.errorMessage);
          this.errorCreate = error.error.errorMessage;
          this.successCreate = '';
        }
      );
  }

  updateConstructionCompanyName(): void {
    this.updateCompanyName.name = this.constructionCompanyName;
    this.constructionCompanyService.updateConstructionCompanyName(this.updateCompanyName)
      .subscribe(
        response => {
          this.errorUpdate = '';
          this.successUpdate = 'Construction company name updated successfully';
        },
        error => {
          this.successUpdate = '';
          this.errorUpdate = error.error.errorMessage;
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
        }
      );
  }

  resetForm(): void {
    this.newConstructionCompany = { name: '' };
  }
}
