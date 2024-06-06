import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Category } from '../models/category.model';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private apiUrl = 'https://localhost:7266/api/categories'; 

  constructor(private http: HttpClient) { }

  createCategory(category: Category): Observable<Category> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', token!);
    return this.http.post<Category>(this.apiUrl, category, { headers });
  }
}
