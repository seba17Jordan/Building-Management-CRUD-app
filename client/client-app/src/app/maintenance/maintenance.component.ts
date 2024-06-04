import { Component } from '@angular/core';
import { User } from '../models/user.model';
import { MaintenanceService } from '../services/maintenance.service';

@Component({
  selector: 'app-maintenance',
  templateUrl: './maintenance.component.html',
  styleUrls: ['./maintenance.component.css']
})
export class MaintenanceComponent {
  newMaintenance: User = { Email: '',LastName:'', Password: '', Name: ''};

  constructor(private maintenanceService: MaintenanceService) { }

  createMaintenance(): void {
    this.maintenanceService.createMaintenance(this.newMaintenance).subscribe(maintenance => {
      console.log('Nueva persona de mantenimiento creada:', maintenance);
    });
  }
}
