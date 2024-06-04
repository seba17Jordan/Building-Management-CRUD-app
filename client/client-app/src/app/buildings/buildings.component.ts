import { Component } from '@angular/core';
import { BuildingService } from '../services/building.service';
import { Building } from '../models/building.model';

@Component({
  selector: 'app-buildings',
  templateUrl: './buildings.component.html',
  styleUrls: ['./buildings.component.css']
})
export class BuildingsComponent {

  buildings: Building[] = [];

  constructor(private buildingService: BuildingService) { }

  ngOnInit(): void {
    //Agrego un edificio harcodeado
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

  delete(building: Building){
    this.buildings = this.buildings.filter(b => b !== building);
    this.buildingService.deleteBuilding(building.Id).subscribe();
  }
}
