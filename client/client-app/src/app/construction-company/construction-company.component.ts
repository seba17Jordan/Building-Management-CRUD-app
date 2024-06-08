
import { Component, OnInit } from '@angular/core';
import { ConstructionCompany } from '../models/constructionCompany.model';
import { ConstructionCompanyService } from '../services/constructionCompany.service';

@Component({
  selector: 'app-construction-company',
  templateUrl: './construction-company.component.html',
  styleUrls: ['./construction-company.component.css']
})
export class ConstructionCompanyComponent implements OnInit{

  newConstructionCompany: Partial<ConstructionCompany> = { name: ''};
  updateCompanyName: Partial<ConstructionCompany> = { name: ''};

    ngOnInit(): void {
      this.loadExistingConstructionCompany();
    }

  existingConstructionCompanyName: string | null = null;
  
  constructor(private constructionCompanyService: ConstructionCompanyService) { }


  //FALTA IMPLEMENTAR EN EL BACKEND
  ngOnInit(): void {
    this.constructionCompanyService.getConstructionCompany().subscribe(
      response => {
        console.log('Empresa de construccion:', response);
        this.existingConstructionCompanyName = response.name;
      },
      error => {
        console.error('Error al obtener la empresa de construccion', error);
      }
    );
  }

  createConstructionCompany(): void {
    // Verificar si se ha proporcionado un nombre de empresa
    if (!this.newConstructionCompany.name) {
      console.error('Please provide a name for the construction company.');
      return;
    }

    

    // Llamar al servicio para crear la empresa constructora
    this.constructionCompanyService.createConstructionCompany(this.newConstructionCompany as ConstructionCompany)
      .subscribe(
        response => {
          console.log('Empresa de construccion creada satisfactoriamente:', response);
          this.existingConstructionCompanyName = response.name; 
        },
        error => {
          console.error('Error al crear la empresa de construccion', error);
        }
      );
  }

  updateConstructionCompanyName(): void {
    if (!this.updateCompanyName.name) {
      console.error('Elige otro nombre por favor');
      return;
    }

    this.constructionCompanyService.updateConstructionCompanyName(this.updateCompanyName as ConstructionCompany)
      .subscribe(
        response => {
          console.log('Se actualizo el nombre de la empresa constructora satisfactoriamente', response);
        },
        error => {
          console.error('Error al actualizar el nombre de la empresa', error);
        }
      );
  }

  loadExistingConstructionCompany(): void {
    this.constructionCompanyService.getConstructionCompany()
      .subscribe(
        response => {
          this.existingConstructionCompanyName = ": "+response.name;
        },
        error => {
          console.error('Error loading construction company', error);
        }
      );
  }
}
