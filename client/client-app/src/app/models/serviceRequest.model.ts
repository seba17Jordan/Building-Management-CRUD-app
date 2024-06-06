export interface ServiceRequest {
    id?: string;
    description: string;
    apartment: string;
    category: string;
    categoryName: string;
    status: ServiceRequestStatus;
    buildingId: string;
    managerId: string;
    maintainancePersonId?: string | null;
    totalCost?: number | null;
    startDate?: Date | null;
    endDate?: Date | null;
  }
  
  export enum ServiceRequestStatus {
    Open = 'Open',
    Attending = 'Attending',
    Closed = 'Closed'
  }
  