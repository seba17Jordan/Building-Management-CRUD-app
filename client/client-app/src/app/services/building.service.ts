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
    return this.http.get<Building[]>(this.buildingUrl, {headers});
  }

  setSelectedBuilding(building: Building): void {
    this.selectedBuildingSubject.next(building);
  }

  getSelectedBuilding(): Building | null {
    return this.selectedBuildingSubject.value;
  }

  updateBuilding(building: Building): Observable<Building> {
    const token = localStorage.getItem('token');
    this.httpOptions.headers.set('Authorization', token!);
    const url = `${this.buildingUrl}/${building.id}`;
    console.log('Sending to url: ', url);
    console.log('Sending building to update: ', building.id);
    return this.http.patch<Building>(url, building, this.httpOptions);
  }

  //AUN SIN HACER
  deleteBuilding(id: number): Observable<Building> {
    const url = `${this.buildingUrl}/${id}`;

    return this.http.delete<Building>(url, this.httpOptions);
  }

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };
}
