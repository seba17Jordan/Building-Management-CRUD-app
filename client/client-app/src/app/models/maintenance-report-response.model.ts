export interface MaintenanceReportResponse {
    name: string;
    pendingRequests: number;
    inProgressRequests: number;
    doneRequests: number;
    averageTime: string;
  }
  