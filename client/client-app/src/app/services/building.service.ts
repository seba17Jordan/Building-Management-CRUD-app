import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Building } from '../models/building.model';
import { BehaviorSubject, Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BuildingService {

  private buildingUrl = 'https://localhost:7266/api/buildings';

  constructor(private http: HttpClient, private router: Router) { }

  //Este es el edificio seleccionado actualmente
  private selectedBuildingSubject = new BehaviorSubject<Building | null>(null);
  selectedBuilding$ = this.selectedBuildingSubject.asObservable();

  getBuildings(): Observable<Building[]> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', token!);
    const url = `${this.buildingUrl}/company-admin`;
    return this.http.get<Building[]>(url, {headers});
  }

  setSelectedBuilding(building: Building): void {
    this.selectedBuildingSubject.next(building);
  }

  getSelectedBuilding(): Building | null {
    return this.selectedBuildingSubject.value;
  }

  createBuilding(building: Building): Observable<Building> {
    const token = localStorage.getItem('token');
    const headers = this.httpOptions.headers.set('Authorization', token!);
    return this.http.post<Building>(this.buildingUrl, building, { headers });
  }

  updateBuilding(building: Building): Observable<Building> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', token!);
    const url = `${this.buildingUrl}/detail/${building.id}`;
    console.log("url: "+url);
    return this.http.patch<Building>(url, building, {headers});
  }

  //AUN SIN HACER
  deleteBuilding(id: string): Observable<Building> {
  //CAMBIAR EL DELETE EN EL BACKEND PARA QUE SEA PARA ADMINS DE COMPANY
    const url = `${this.buildingUrl}/${id}`;
    return this.http.delete<Building>(url, this.httpOptions);
  }
 
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };
  
}
