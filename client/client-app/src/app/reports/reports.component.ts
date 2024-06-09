import { Component, OnInit } from '@angular/core';
import { ReportService } from '../services/report.service';
import { ReportResponse } from '../models/report-response.model';
import { BuildingService } from '../services/building.service';
import { MaintenanceReportResponse } from '../models/maintenance-report-response.model';
import { AdminService } from '../services/admin.service';


@Component({
  selector: 'app-report',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.css']
})
export class ReportComponent implements OnInit {
  reports: ReportResponse[] = [];
  maintenanceReports: MaintenanceReportResponse[] = [];
  building?: string = "";
  buildings: string[] = [];
  maintenancePersons: string[] = [];

  selectedBuilding: string = "";
  selectedBuildingForMaintenance: string = "";
  selectedMaintenancePerson: string = "";

  showWarning: boolean = false;

  constructor(private reportService: ReportService, private buildingService: BuildingService, private adminService: AdminService) { }

  ngOnInit(): void {
    this.getReports();
    this.getAllBuildingsByManager();
    this.getMaintenancePersons();
  }

  getAllBuildingsByManager(): void {
    this.buildingService.getAllBuildingsByManager()
      .subscribe(
        buildings => {
          this.buildings = buildings.map(building => building.name); 
          console.log('Buildings:', this.buildings); 
        },
        error => {
          console.error('Error fetching buildings:', error);
        }
      );
  }

  getMaintenancePersons(): void {
    this.adminService.getAllMaintenancePersons()
      .subscribe(
        persons => {
          this.maintenancePersons = persons.map(person => person.name);
          console.log('Maintenance Persons:', this.maintenancePersons);
        },
        error => {
          //console.error('Error fetching maintenance persons:', error);
          //console.log(error.error.innerMessage)
        }
      );
  }

  getReports(): void {
    this.reportService.getReport(this.selectedBuilding)
      .subscribe(
        reports => {
          this.reports = reports;
          console.log('Reports:', reports); // Para verificar la respuesta en la consola del navegador
        },
        error => {
          console.error('Error fetching reports:', error);
        }
      );
  }

  getMaintenanceReports(buildingName?: string, maintenanceName?: string): void {
    this.reportService.getMaintenanceReport(buildingName ?? this.selectedBuildingForMaintenance, maintenanceName ?? this.selectedMaintenancePerson)
      .subscribe(
        reports => {
          this.maintenanceReports = reports;
          console.log('Maintenance Reports:', reports); 
        },
        error => {
          console.error('Error fetching maintenance reports:', error);
          console.log("INNER ERROR: " + error.error.errorMessage);
        }
      );
  }

  filterMaintenanceReports(): void {
    if (!this.selectedBuildingForMaintenance) {
      this.showWarning = true;
      return;
    }

    this.showWarning = false;
    this.getMaintenanceReports(this.selectedBuildingForMaintenance, this.selectedMaintenancePerson);
  }

  filterReports(): void {
    this.getReports();
  }
}
