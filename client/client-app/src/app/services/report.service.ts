import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ReportResponse } from '../models/report-response.model';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
    private baseUrl = 'https://localhost:7266/api/reports';

  constructor(private http: HttpClient) { }

  getReport(building?: string): Observable<ReportResponse[]> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', token!);
    console.log('Building:', building);
    return this.http.get<ReportResponse[]>(`${this.baseUrl}?building=${building}`, { headers });
  }
}
