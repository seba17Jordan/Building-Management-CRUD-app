
import { Component } from '@angular/core';
import { ConstructionCompany } from '../models/constructionCompany.model';
import { ConstructionCompanyService } from '../services/constructionCompany.service';

@Component({
  selector: 'app-construction-company',
  templateUrl: './construction-company.component.html',
  styleUrls: ['./construction-company.component.css']
})
export class ConstructionCompanyComponent {
  newConstructionCompany: ConstructionCompany = {
    name: ''
  };

  constructor(private constructionCompanyService: ConstructionCompanyService) { }

  createConstructionCompany(): void {
    // Verificar si se ha proporcionado un nombre de empresa
    if (!this.newConstructionCompany.name) {
      console.error('Please provide a name for the construction company.');
      return;
    }

    // Llamar al servicio para crear la empresa constructora
    this.constructionCompanyService.createConstructionCompany(this.newConstructionCompany)
      .subscribe(
        response => {
          console.log('Construction company created successfully:', response);
        },
        error => {
          console.error('Error creating construction company:', error);
        }
      );
  }
}
