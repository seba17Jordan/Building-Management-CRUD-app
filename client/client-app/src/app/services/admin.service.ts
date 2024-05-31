import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  private baseUrl = 'api/admins'; 

  constructor(private http: HttpClient) { }

  // Método para crear un nuevo administrador
  createAdmin(admin: User): Observable<User> {
    return this.http.post<User>(`${this.baseUrl}`, admin);
  }
}
