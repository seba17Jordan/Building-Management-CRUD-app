import { Component, OnInit } from '@angular/core';
import { ReportService } from '../services/report.service';
import { ReportResponse } from '../models/report-response.model';
import { BuildingService } from '../services/building.service';
import { MaintenanceReportResponse } from '../models/maintenance-report-response.model';


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
  selectedBuilding: string = "";
  maintenanceName?: string = "";

  constructor(private reportService: ReportService, private buildingService: BuildingService) { }

  ngOnInit(): void {
    this.getReports();
    this.getBuildings();
    this.getMaintenanceReports();
  }

  getBuildings(): void {
    this.buildingService.getAllBuildings()
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

  getMaintenanceReports(): void {
    if (this.selectedBuilding) {
      this.reportService.getMaintenanceReport(this.selectedBuilding, this.maintenanceName)
        .subscribe(
          maintenanceReports => {
            this.maintenanceReports = maintenanceReports;
            console.log('Maintenance Reports:', maintenanceReports);
          },
          error => {
            console.error('Error fetching maintenance reports:', error);
          }
        );
    }
  }

  filterReports(): void {
    this.getReports();
    this.getMaintenanceReports(); 
  }

  filterMaintenanceReports(): void {
    this.getMaintenanceReports();
  }
}
