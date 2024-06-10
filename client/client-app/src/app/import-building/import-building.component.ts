import { Component, Input, OnInit } from '@angular/core';
import { BuildingService } from '../services/building.service';
import { Router } from '@angular/router';
import { ImportRequest } from '../models/importRequest.model';

@Component({
  selector: 'app-import-building',
  templateUrl: './import-building.component.html',
  styleUrls: ['./import-building.component.css']
})
export class ImportBuildingComponent implements OnInit{

  importers: string[] = [];
  files: string[] = [];
  selectedImporter: string = '';
  selectedFile: string = '';
  errorMessage: string = '';
  successMessage: string = '';

  importRequest: ImportRequest = {importerName: '', fileName: ''};
  
  constructor(private buildingService: BuildingService, private router: Router) { }

  ngOnInit(): void {
    this.buildingService.getImporters().subscribe(importers => this.importers = importers);
    this.buildingService.getFiles().subscribe(files => this.files = files);
  }

  importBuilding(): void {
    this.errorMessage = '';
    this.importRequest.importerName = this.selectedImporter;
    this.importRequest.fileName = this.selectedFile;
    this.buildingService.importBuilding(this.importRequest).subscribe(
        (response) => {
          this.errorMessage = '';
          this.successMessage = 'Edificio importado con éxito.';
        },
        (error) => {
          this.successMessage = '';
          this.errorMessage = error.error.errorMessage;
        }
    );
  }
}
