import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class MaintenanceService {

  private baseUrl = 'https://localhost:7266/api/maintenances';

  constructor(private http: HttpClient) { }

  createMaintenance(maintenance: User): Observable<User> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', token!);
    return this.http.post<User>(`${this.baseUrl}`, maintenance, {headers});
  }
}
