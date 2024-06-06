import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Building } from '../models/building.model';
import { BuildingService } from '../services/building.service';
import { Apartment } from '../models/apartment.model';
import { Owner } from '../models/owner.model';

@Component({
  selector: 'app-create-building',
  templateUrl: './create-building.component.html',
  styleUrls: ['./create-building.component.css']
})
export class CreateBuildingComponent {
  newBuilding: Building = {
    name: '',
    address: '',
    constructionCompany: '',
    commonExpenses: 0,
    hasManager: false,
    managerName: '',
    apartments: []
  };

  constructor(private buildingService: BuildingService, private router: Router) { }

  createBuilding(): void {
    // Verifica el payload antes de enviarlo
    console.log(this.newBuilding);

    this.buildingService.createBuilding(this.newBuilding).subscribe({
      next: () => {
        this.router.navigate(['/buildings']);
      },
      error: (err) => {
        console.error('Error creating building:', err);
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
