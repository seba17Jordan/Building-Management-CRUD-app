import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ServiceRequest } from '../models/serviceRequest.model';

@Injectable({
  providedIn: 'root'
})
export class ServiceRequestService {
    private baseUrl = 'https://localhost:7266/api/serviceRequests';

  constructor(private http: HttpClient) { }

  createServiceRequest(serviceRequest: ServiceRequest): Observable<ServiceRequest> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', token!);
    console.log('Service Request status:', serviceRequest.status);
    console.log('Service Request description:', serviceRequest.description);
    console.log('Service Request apartmentId:', serviceRequest.apartment);
    console.log('Service Request categoryId:', serviceRequest.category);
    return this.http.post<ServiceRequest>(this.baseUrl, serviceRequest, { headers });
  }

  getAllServiceRequestsManager(category?: string): Observable<ServiceRequest[]> {
    const token = localStorage.getItem('token');
    let headers = new HttpHeaders();
    if (token) {
      headers = headers.set('Authorization', token);
    }
  
    let params = new HttpParams();
    if (category) {
      params = params.set('category', category);
    }
  
    return this.http.get<ServiceRequest[]>(`${this.baseUrl}/manager-requests`, { headers, params });
  }

  getAllServiceRequestsMaintenance(): Observable<ServiceRequest[]> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', token!);
    return this.http.get<ServiceRequest[]>(`${this.baseUrl}`, { headers });
  }

  assignMaintenancePerson(requestId: string, maintenancePersonId: string): Observable<any> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', token!);
    const body = { id: maintenancePersonId };
    console.log('HTTP PATCH request body:', body);
    return this.http.patch<any>(`${this.baseUrl}/${requestId}/assign-request`, body, { headers });
  }

  //Si mando con totalCost es para cerrar, si mando sin, es para aceptar
  updateServiceRequestStatus(id: string, totalCost?: number): Observable<ServiceRequest> {
    console.log('Service Request totalCost:', totalCost);
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', token!);
    return this.http.patch<ServiceRequest>(`${this.baseUrl}/${id}`, { totalCost }, { headers });
  }
}
