import { Component, Input, OnInit } from '@angular/core';
import { ServiceRequest, ServiceRequestStatus } from '../models/serviceRequest.model';
import { ServiceRequestService } from '../services/serviceRequest.service';
import { CategoryService } from '../services/category.service';
import { BuildingService } from '../services/building.service';
import { Apartment } from '../models/apartment.model';
import { Category } from '../models/category.model';
@Component({
  selector: 'app-service-request',
  templateUrl: './service-request.component.html',
  styleUrls: ['./service-request.component.css']
})
export class ServiceRequestComponent implements OnInit {
  
  @Input() newServiceRequest: ServiceRequest = {apartment: '', category: '', description: ''};

  serviceRequests: ServiceRequest[] = [];
  categories: Category[] = []; 
  apartments : Apartment[] = [];

  description?: string;
  apartmentId?: string;
  categoryId?: string;

  constructor(
    private serviceRequestService: ServiceRequestService,
    private categoryService: CategoryService,
    private buildingService: BuildingService
  ) { }

  ngOnInit(): void {
    this.getAllCategories(); 
    this.getAllApartmentsForUser();
  }

  createServiceRequest(): void {
    this.newServiceRequest.apartment = this.apartmentId;
    this.newServiceRequest.description = this.description;
    this.newServiceRequest.category = this.categoryId;
    this.newServiceRequest.status = 0;
    this.serviceRequestService.createServiceRequest(this.newServiceRequest!).subscribe();
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

  getAllApartmentsForUser(): void {
    this.buildingService.getAllApartmentsForManager()
      .subscribe(
        (response) => {
          console.log('Apartments retrieved successfully:', response);
          this.apartments = response;
        },
        (error) => {
          console.error('Error retrieving apartments:', error);
        }
      );
  }
}
