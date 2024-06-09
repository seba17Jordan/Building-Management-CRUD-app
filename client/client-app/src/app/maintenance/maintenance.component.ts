import { Component } from '@angular/core';
import { User } from '../models/user.model';
import { MaintenanceService } from '../services/maintenance.service';

@Component({
  selector: 'app-maintenance',
  templateUrl: './maintenance.component.html',
  styleUrls: ['./maintenance.component.css']
})
export class MaintenanceComponent {
  newMaintenance: User = { email: '', lastName:'', password: '', name: ''};
  successMessage: string = "";
  errorMessage: string = "";

  constructor(private maintenanceService: MaintenanceService) { }

  createMaintenance(): void {
    this.maintenanceService.createMaintenance(this.newMaintenance).subscribe(
      maintenance => {
        this.successMessage = 'Nueva persona de mantenimiento creada con éxito.';
        this.errorMessage = '';
        this.resetForm();
      },
      error => {
        console.error('Error al crear persona de mantenimiento:', error);
        this.errorMessage = 'Error al crear persona de mantenimiento.';
        this.successMessage = '';
      }
    );
  }

  private resetForm(): void {
    this.newMaintenance = { email: '', lastName: '', password: '', name: '' };
  }
}
