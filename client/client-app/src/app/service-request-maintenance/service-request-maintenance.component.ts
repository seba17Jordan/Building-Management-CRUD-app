import { ChangeDetectorRef, Component } from '@angular/core';
import { ServiceRequestService } from '../services/serviceRequest.service';
import { ServiceRequest } from '../models/serviceRequest.model';

@Component({
  selector: 'app-service-request-maintenance',
  templateUrl: './service-request-maintenance.component.html',
  styleUrls: ['./service-request-maintenance.component.css']
})
export class ServiceRequestMaintenanceComponent {

  serviceRequests: ServiceRequest[] = [];
  totalCost?: number;

  constructor(private serviceRequestService: ServiceRequestService, private cdr: ChangeDetectorRef) { }

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

  //Quiero aceptar, entonces no mando monto
  accept(serviceRequest: ServiceRequest){
    this.serviceRequestService.updateServiceRequestStatus(serviceRequest.id!).subscribe(
        updatedRequest => {
          console.log('Service Request updated:', updatedRequest);
        const index = this.serviceRequests.findIndex(req => req.id === updatedRequest.id); //para actualizar en tiempo real
        if (index !== -1) {
          this.serviceRequests[index] = updatedRequest;
          this.cdr.detectChanges(); // Forzar la detección de cambios
        }
      }  
    );
  }

  //Quiero cerrar, entonces mando monto
  close(serviceRequest: ServiceRequest){
    this.serviceRequestService.updateServiceRequestStatus(serviceRequest.id!, serviceRequest.totalCost).subscribe(
        updatedRequest => {
        const index = this.serviceRequests.findIndex(req => req.id === updatedRequest.id); //para actualizar en tiempo real
        if (index !== -1) {
          this.serviceRequests[index] = updatedRequest;
          this.cdr.detectChanges(); // Forzar la detección de cambios
        }
      }  
    );
  }
}
