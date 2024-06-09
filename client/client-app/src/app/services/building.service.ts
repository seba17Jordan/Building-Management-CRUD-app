import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Building } from '../models/building.model';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { ImportRequest } from '../models/importRequest.model';
import { Apartment } from '../models/apartment.model';
import { User } from '../models/user.model';

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
    return this.http.patch<Building>(url, building, {headers});
  }

  deleteBuilding(id: string): Observable<Building> {
  //CAMBIAR EL DELETE EN EL BACKEND PARA QUE SEA PARA ADMINS DE COMPANY
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', token!);
    const url = `${this.buildingUrl}/${id}`;
    return this.http.delete<Building>(url, {headers});
  }

  getAllApartmentsForManager(): Observable<Apartment[]> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', token!);
    const url = `${this.buildingUrl}/manager/apartments`;
    return this.http.get<Apartment[]>(url, { headers });
  }

  getAllBuildings(): Observable<Building[]> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', token!);
    return this.http.get<Building[]>(this.buildingUrl, { headers });
  }

  getAllBuildingsByManager(): Observable<Building[]> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', token!);
    const url = `${this.buildingUrl}/manager`;
    return this.http.get<Building[]>(url, { headers });
  }

  assignManager(buildingId: string, managerId : string): Observable<Building> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', token!);
    const url = `${this.buildingUrl}/${buildingId}`;
    return this.http.patch<Building>(url, {id: managerId}, { headers });
  }
 
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  importBuilding(importRequest: ImportRequest): Observable<any> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `${token}`,
      'Content-Type': 'application/json'
    });
    const url = `${this.buildingUrl}/import`;
    return this.http.post<any>(url, importRequest ,{ headers });
  }
  
  getImporters(): Observable<string[]> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', token!);
    const url = `${this.buildingUrl}/importers`;
    return this.http.get<string[]>(url, { headers });
  }

  getFiles(): Observable<string[]> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', token!);
    const url = `${this.buildingUrl}/files`;
    return this.http.get<string[]>(url, { headers });
  }
}
