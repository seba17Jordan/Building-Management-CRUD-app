import { Component, Input, OnInit } from '@angular/core';
import { Building } from '../models/building.model';
import { BuildingService } from '../services/building.service';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { User } from '../models/user.model';

@Component({
  selector: 'app-building-detail',
  templateUrl: './building-detail.component.html',
  styleUrls: ['./building-detail.component.css']
})
export class BuildingDetailComponent implements OnInit{
  @Input() building? : Building;
  @Input() manager? : User;

  constructor(private buildingService: BuildingService,
     private location: Location,
     private router: Router) {}

  ngOnInit(): void {
    this.getBuilding();
  }

  getBuilding(): void {
    this.building = this.buildingService.getSelectedBuilding()!;
  }

  goBack(): void {
    this.location.back();
  }

  save(): void {
    console.log('Building common expenses new:', this.building?.commonExpenses);
    if (this.building) {
      this.buildingService.updateBuilding(this.building).subscribe(() => this.goBack());
    }
  }

  assignManager(buildingId: string): void {
    this.buildingService.assignManager(buildingId, this.manager?.Id!).subscribe(() => this.goBack());
  }
}
