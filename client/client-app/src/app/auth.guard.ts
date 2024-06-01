import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) {}

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    /*const expectedRole = next.data.expectedRole;
    const currentRole = this.authService.getUserRole();

    if (!this.authService.isLoggedIn() || (expectedRole && currentRole !== expectedRole)) {
      this.router.navigate(['/home']); // Redirigir a home si no tiene el rol esperado
      return false;
    }*/
    return true;
  }
}
