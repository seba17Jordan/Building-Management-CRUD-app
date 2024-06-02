import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, take, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InvitationService {

  constructor(private http: HttpClient, private router: Router) { }

  private loginUrl = 'https://localhost:7266/api/invitations';

  createInvitation(email: string, name: string, role: number, expirationDate: Date): Observable<any>{
    console.log('fecha de expiracion: '+ expirationDate);
    return this.http.post<any>(this.loginUrl, {email, name, role, expirationDate}).pipe(
      tap(response =>{
        console.log('Se creo correctamente la invitacion: ');
        console.log(response);
        }
      )
    );
  }
}
