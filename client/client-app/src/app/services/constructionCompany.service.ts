import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ConstructionCompany } from '../models/constructionCompany.model';


@Injectable({
  providedIn: 'root'
})
export class ConstructionCompanyService {

  private apiUrl = 'https://localhost:7266/api/constructionCompanies';

  constructor(private http: HttpClient) { }

  createConstructionCompany(constructionCompany: ConstructionCompany): Observable<ConstructionCompany> { 
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', token!);
    return this.http.post<ConstructionCompany>(`${this.apiUrl}`, constructionCompany, { headers });
  }

  updateConstructionCompanyName(newCompanyName: ConstructionCompany): Observable<ConstructionCompany> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', token!);
    return this.http.patch<ConstructionCompany>(this.apiUrl, newCompanyName, { headers });
  }

  getConstructionCompany(): Observable<ConstructionCompany> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', token!);
    return this.http.get<ConstructionCompany>(this.apiUrl, { headers });
  }
}
