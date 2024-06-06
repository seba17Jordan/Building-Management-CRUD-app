import { Component, OnInit } from '@angular/core';
import { ServiceRequest, ServiceRequestStatus } from '../models/serviceRequest.model';
import { ServiceRequestService } from '../services/serviceRequest.service';
import { CategoryService } from '../services/category.service';

@Component({
  selector: 'app-service-request',
  templateUrl: './service-request.component.html',
  styleUrls: ['./service-request.component.css']
})
export class ServiceRequestComponent implements OnInit {
  newServiceRequest: ServiceRequest = {
    description: '',
    apartment: '',
    category: '',
    categoryName: '',
    status: ServiceRequestStatus.Open,
    buildingId: '',
    managerId: ''
  };

  serviceRequests: ServiceRequest[] = [];
  categories: any[] = []; 

  constructor(
    private serviceRequestService: ServiceRequestService,
    private categoryService: CategoryService
  ) { }

  ngOnInit(): void {
    this.getAllServiceRequestsManager();
    this.getAllCategories(); 
  }

  createServiceRequest(): void {
    this.serviceRequestService.createServiceRequest(this.newServiceRequest)
        .subscribe(
            response => {
                console.log('Service request created successfully:', response);
                this.getAllServiceRequestsManager();
                this.getAllCategories(); // Refresh categories after creating a service request
                this.newServiceRequest = {
                    description: '',
                    apartment: '',
                    category: '', 
                    categoryName: '', 
                    status: ServiceRequestStatus.Open, 
                    buildingId: '', 
                    managerId: '' 
                };
            },
            error => {
                console.error('Error creating service request:', error);
            }
        );
  }

  getAllServiceRequestsManager(category?: string): void {
    this.serviceRequestService.getAllServiceRequestsManager(category)
      .subscribe(
        response => {
          console.log('Service requests retrieved successfully:', response);
          this.serviceRequests = response;
        },
        error => {
          console.error('Error retrieving service requests:', error);
        }
      );
  }

  getAllCategories(): void {
    this.categoryService.getAllCategories()
      .subscribe(
        response => {
          console.log('Categories retrieved successfully:', response);
          this.categories = response;
        },
        error => {
          console.error('Error retrieving categories:', error);
        }
      );
  }
}
