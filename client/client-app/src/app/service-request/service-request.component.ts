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
  selectedCategory?: string;  // Aquí se declara la propiedad selectedCategory

  constructor(
    private serviceRequestService: ServiceRequestService,
    private categoryService: CategoryService,
    private buildingService: BuildingService
  ) { }

  ngOnInit(): void {
    this.getAllCategories(); 
    this.getAllApartmentsForUser();
    this.getAllServiceRequests();
  }

  createServiceRequest(): void {
    this.newServiceRequest.apartment = this.apartmentId;
    this.newServiceRequest.description = this.description;
    this.newServiceRequest.category = this.categoryId;
    this.newServiceRequest.status = 0;
    this.serviceRequestService.createServiceRequest(this.newServiceRequest!).subscribe(
      (createdRequest) => {
        this.serviceRequests.push(createdRequest); 
        this.resetForm();
      },
      (error) => {
        console.error('Error al crear service request:', error);
      }
    );
  }

  resetForm(): void {
    this.newServiceRequest = { apartment: '', category: '', description: '' };
    this.description = '';
    this.apartmentId = '';
    this.categoryId = '';
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

  getAllServiceRequests(): void {
    this.serviceRequestService.getAllServiceRequestsManager(this.selectedCategory)
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

  onCategoryChange(): void {
    this.getAllServiceRequests();
  }
}
