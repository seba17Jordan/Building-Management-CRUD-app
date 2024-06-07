export interface ServiceRequest {
    id?: string;

    description?: string;
    apartment?: string;
    category?: string;
    status?: number;
    
    buildingId?: string;
    managerId?: string;
    maintainancePersonId?: string;
    
    totalCost?: number;
    startDate?: Date;
    endDate?: Date;
  }
  
  export enum ServiceRequestStatus {
    Open = 'Open',
    Attending = 'Attending',
    Closed = 'Closed'
  }
  