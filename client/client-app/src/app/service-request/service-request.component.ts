import { Component, Input, OnInit } from '@angular/core';
import { ServiceRequest, ServiceRequestStatus } from '../models/serviceRequest.model';
import { ServiceRequestService } from '../services/serviceRequest.service';
import { CategoryService } from '../services/category.service';
import { BuildingService } from '../services/building.service';
import { Apartment } from '../models/apartment.model';
import { Category } from '../models/category.model';
import {User} from '../models/user.model';
import { AdminService } from '../services/admin.service';
import { Observable } from 'rxjs';
@Component({
  selector: 'app-service-request',
  templateUrl: './service-request.component.html',
  styleUrls: ['./service-request.component.css']
})
export class ServiceRequestComponent implements OnInit {
  
  @Input() newServiceRequest: ServiceRequest = { apartmentId: '', categoryId: '', description: '' };

  showAssignControls: boolean = true; 


  serviceRequests: ServiceRequest[] = [];
  categories: Category[] = []; 
  apartments : Apartment[] = [];
  maintenancePersons: User[] = [];

  description: string = '';
  apartmentId?: string;
  categoryId?: string;
  selectedCategory?: string;  // Aquí se declara la propiedad selectedCategory
  error: string = '';

  constructor(
    private serviceRequestService: ServiceRequestService,
    private categoryService: CategoryService,
    private buildingService: BuildingService,
    private adminService: AdminService
  ) { }

  ngOnInit(): void {
    this.getAllCategories(); 
    this.getAllApartmentsForUser();
    this.getAllServiceRequests();
    this.getAllMaintenancePersons();
    console.log('Service requests:', this.serviceRequests);
  }

  createServiceRequest(): void {
    this.newServiceRequest.apartmentId = this.apartmentId;
    this.newServiceRequest.description = this.description;
    this.newServiceRequest.categoryId = this.categoryId;
    this.newServiceRequest.status = 0;
    this.serviceRequestService.createServiceRequest(this.newServiceRequest!).subscribe(
      (createdRequest) => {
        this.serviceRequests.push(createdRequest); 
        this.resetForm();
      },
      (error) => {
        this.error = error.error.errorMessage;
      }
    );
  }

  resetForm(): void {
    this.newServiceRequest = { apartmentId: '', categoryId: '', description: '' };
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

  getAllMaintenancePersons(): void {
    this.adminService.getAllMaintenancePersons()
      .subscribe(
        response => {
          console.log('Maintenance persons retrieved successfully:', response);
          this.maintenancePersons = response;
        },
        error => {
          console.error('Error retrieving maintenance persons:', error);
        }
      );
  }

  assignMaintenancePerson(requestId: string, maintenancePersonId: string): void {
    console.log(`Assigning maintenance person ID ${maintenancePersonId} to request ${requestId}`);
    
    if (maintenancePersonId && requestId) {
      this.serviceRequestService.assignMaintenancePerson(requestId, maintenancePersonId).subscribe(
        () => {
          console.log('Maintenance person assigned successfully');
          const updatedRequestIndex = this.serviceRequests.findIndex(request => request.id === requestId);
          if (updatedRequestIndex !== -1) {
            this.serviceRequests[updatedRequestIndex].selectedMaintenancePersonId = maintenancePersonId;
          }
          this.showAssignControls = false;
        },
        (error) => {
          console.error('Error assigning maintenance person:', error);
        }
      );
    } else {
      console.error('Maintenance person ID or request ID is missing');
    }
  }


  getMaintenancePersonName(request: ServiceRequest): string | undefined {
    if (request.selectedMaintenancePersonId) {
      const maintenancePerson = this.maintenancePersons.find(person => person.id === request.selectedMaintenancePersonId);
      return maintenancePerson ? maintenancePerson.name : undefined;
    }
    return undefined;
  }

  
  onCategoryChange(): void {
    this.getAllServiceRequests();
  }
}
