import { Apartment } from "./apartment.model";
import { Category } from "./category.model";
import { User } from "./user.model";

export interface ServiceRequest {
    id?: string;

    description?: string;
    apartment?: Apartment;
    apartmentId?: string;
    category?: Category;
    categoryId?: string;
    status?: number;
    
    buildingId?: string;
    managerId?: string;
    maintainancePersonId?: string;
    maintenancePerson?: User;
    selectedMaintenancePersonId?: string; 

    
    totalCost?: number;
    startDate?: Date;
    endDate?: Date;
  }
  
  export enum ServiceRequestStatus {
    Open = 'Open',
    Attending = 'Attending',
    Closed = 'Closed'
  }
  