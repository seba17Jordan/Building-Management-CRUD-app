import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private loginUrl = 'https://localhost:7266/api/session';

  constructor(private http: HttpClient, private router: Router) { }

  login(email: string, password: string): Observable<any> {
    return this.http.post<any>(this.loginUrl, { email, password }).pipe(
      tap(response =>{
        localStorage.setItem('token', response.token.toString());
        localStorage.setItem('role', response.role);
        localStorage.setItem('Name', response.Name);
        localStorage.setItem('Email', response.Email);
      })
    );
  }
  
  logout(): void {
    const token = localStorage.getItem('token');
    if (token) {
      const headers = new HttpHeaders().set('Authorization', token);
      this.http.delete(this.loginUrl, { headers }).subscribe(() => {
          localStorage.removeItem('token');
          localStorage.removeItem('role');
          this.router.navigate(['/login']);
        },
        error => {
          console.error('Logout error:', error);
          // Manejar el error si es necesario
          localStorage.removeItem('token');
          localStorage.removeItem('role');
          this.router.navigate(['/login']);
        }
      );
    } else {
      localStorage.removeItem('token');
      localStorage.removeItem('role');
      this.router.navigate(['/login']);
    }
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  getUserRole(): string | null {
    return localStorage.getItem('role');
  }

  getUsername(): string | null {
    return localStorage.getItem('Name');
  }

  getEmail(): string | null {
    return localStorage.getItem('Email');
  }

  isAdmin(): boolean {
    return this.getUserRole() === '0';
  }

  isManager(): boolean {
    return this.getUserRole() === '1';
  }

  isMaintenance(): boolean {
    return this.getUserRole() === '2';
  }

  isCompanyAdmin(): boolean {
    return this.getUserRole() === '3';
  }
}
