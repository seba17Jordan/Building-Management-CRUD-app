import { Component, OnInit } from '@angular/core';
import { BuildingService } from '../services/building.service';
import { Building } from '../models/building.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-buildings',
  templateUrl: './buildings.component.html',
  styleUrls: ['./buildings.component.css']
})
export class BuildingsComponent implements OnInit{

  buildings: Building[] = [];
  error: string = '';

  constructor(private buildingService: BuildingService, private router: Router) { }

  ngOnInit(): void {
    this.getBuildings();
  }

  getBuildings(): void {
    this.buildingService.getBuildings()
      .subscribe(x => {
        this.buildings = x
        console.log('Se imprimieron los edificio');
      }
    );
  }

  selectBuilding(building: Building): void{
    this.buildingService.setSelectedBuilding(building);
    this.router.navigate(['/buildings/detail', building.id]);
  }

  delete(building: Building){
    this.buildings = this.buildings.filter(b => b !== building);   //Borrado visual
    this.buildingService.deleteBuilding(building.id!).subscribe(
      (response) => {
        console.log('Edificio eliminado');
      },
      (error) => {
        this.error = error.error.errorMessage;
        console.log('ERROR MENSAJE', error.error.errorMessage);
        console.log('ERROR INNER', error.error.innerException);
        console.log('ERROR GENERAL', error.error);
        this.getBuildings();
      }
    );
  }
}
