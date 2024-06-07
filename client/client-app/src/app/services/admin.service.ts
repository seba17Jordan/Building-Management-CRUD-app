import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  private baseUrl = 'https://localhost:7266/api/admins';

  constructor(private http: HttpClient) { }

  createAdmin(admin: User): Observable<User> {
    return this.http.post<User>(`${this.baseUrl}`, admin);
  }

  getManagers(): Observable<User[]> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', token!);
    return this.http.get<User[]>(`${this.baseUrl}/managers`, {headers});
  }
}
