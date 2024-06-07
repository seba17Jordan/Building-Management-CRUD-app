import { Component } from '@angular/core';
import { ServiceRequestService } from '../services/serviceRequest.service';
import { ServiceRequest } from '../models/serviceRequest.model';

@Component({
  selector: 'app-service-request-maintenance',
  templateUrl: './service-request-maintenance.component.html',
  styleUrls: ['./service-request-maintenance.component.css']
})
export class ServiceRequestMaintenanceComponent {

  serviceRequests: ServiceRequest[] = [];

  constructor(private serviceRequestService: ServiceRequestService) { }

  ngOnInit(): void {
    this.getServiceRequests();
  }

  getServiceRequests(): void {
    this.serviceRequestService.getAllServiceRequestsMaintenance()
      .subscribe(x => {
        this.serviceRequests = x
      }
    );
  }

}
