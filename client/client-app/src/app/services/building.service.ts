import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Building } from '../models/building.model';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BuildingService {

  private buildingUrl = 'https://localhost:7266/api/buildings';

  constructor(private http: HttpClient, private router: Router) { }

  getBuildings(): Observable<Building[]> {
    const token = localStorage.getItem('token');
    console.log(token);
    const headers = new HttpHeaders().set('Authorization', token!);
    return this.http.get<Building[]>(this.buildingUrl, {headers});
  }

  deleteBuilding(id: number): Observable<Building> {
    const url = `${this.buildingUrl}/${id}`;

    return this.http.delete<Building>(url, this.httpOptions);
  }

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };
}
