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

  acceptInvitation(email: string, password: string): Observable<any>{
    return this.http.post<any>(this.loginUrl + '/accept', {email, password}).pipe(
      tap(response =>{
        console.log('Se acepto correctamente la invitacion: ');
        console.log(response);
        }
      )
    );
  }

  /*Posibilidad de mejora: pararece texto: "Busque el mail de la invitacion que quiere rechazar:", luego
  al hacer click en bucar solicitudes, si existe, entonces aparece un boton que dice rechazar  */
  rejectInvitation(email: string): Observable<any>{
    return this.http.patch<any>(this.loginUrl, {email}).pipe(
      tap(response =>{
        console.log('Se rechazo correctamente la invitacion: ');
        }
      )
    );
  }
}
