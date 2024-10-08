import { Component, Input, OnInit } from '@angular/core';
import { Building } from '../models/building.model';
import { BuildingService } from '../services/building.service';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { User } from '../models/user.model';
import { AdminService } from '../services/admin.service';

@Component({
  selector: 'app-building-detail',
  templateUrl: './building-detail.component.html',
  styleUrls: ['./building-detail.component.css']
})
export class BuildingDetailComponent implements OnInit{
  @Input() building? : Building;

  managers: User[] = [];
  selectedManager?: User;
  currentManagerName?: string;
  detailsError: string = '';
  managerError: string = '';
  

  constructor(private buildingService: BuildingService,
     private location: Location,
     private adminService: AdminService,
     private router: Router) {}

  ngOnInit(): void {
    this.getBuilding();
    this.currentManagerName = this.building?.managerName;
    this.getManagers();

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
      this.buildingService.updateBuilding(this.building).subscribe(
        () => {
          this.goBack() 
        },
        (error) => {
          this.detailsError = error.error.errorMessage;
        }
      );
    }
  }

  assignManager(buildingId: string): void {
    this.buildingService.assignManager(buildingId, this.selectedManager?.id!).subscribe(
      () => {
        this.goBack()
      },
      (error) => {
        this.managerError = error.error.errorMessage;
      }
    );
  }

  getManagers(): void {
    this.adminService.getManagers().subscribe(managers => this.managers = managers);
  }
}
