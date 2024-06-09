import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Building } from '../models/building.model';
import { BuildingService } from '../services/building.service';
import { Apartment } from '../models/apartment.model';
import { Owner } from '../models/owner.model';
import { ConstructionCompanyService } from '../services/constructionCompany.service';

@Component({
  selector: 'app-create-building',
  templateUrl: './create-building.component.html',
  styleUrls: ['./create-building.component.css']
})
export class CreateBuildingComponent implements OnInit{
  newBuilding: Building = {
    name: '',
    address: '',
    constructionCompany: '',
    commonExpenses: 0,
    hasManager: false,
    managerName: '',
    apartments: []
  };

  error: string = '';

  constructor(private buildingService: BuildingService, private router: Router, private constructionCompanyService: ConstructionCompanyService) { }


  ngOnInit(): void {
    this.constructionCompanyService.getConstructionCompany().subscribe({
      next: (company) => {
        this.newBuilding.constructionCompany = company.name;
        console.log('Empresa constructora obtenida:', this.newBuilding.constructionCompany);
      },
      error: (err) => {
        console.error('Error al obtener la empresa constructora', err);
      }
    });
  }

  createBuilding(): void {
    this.buildingService.createBuilding(this.newBuilding).subscribe({
      next: () => {
        this.router.navigate(['/buildings']);
      },
      error: (err) => {
        this.error = err.error.errorMessage;
      }
    });
  }

  cancel(): void {
    this.router.navigate(['/buildings']);
  }

  addApartment(): void {
    const newApartment: Apartment = {
      floor: 0,
      number: 0,
      owner: {
        name: '',
        lastName: '',
        email: ''
      },
      rooms: 0,
      bathrooms: 0,
      hasTerrace: false
    };
    this.newBuilding.apartments.push(newApartment);
  }

  removeApartment(index: number): void {
    this.newBuilding.apartments.splice(index, 1);
  }
}
