import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthService } from './services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate { //CanActivate para decidir si permitir acceso a una ruta

  constructor(private authService: AuthService, private router: Router) {} //AuthService para ver si esta autenticado con rol correcto

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const expectedRole = next.data['expectedRole'];
    const currentRole = this.authService.getUserRole();

    if (!this.authService.isLoggedIn()) { //Si no está logeado o no tiene el rol esperado lo mando a home
      console.log('Access denied, not authenticated. Redirecting to home page...');
      this.router.navigate(['/login']);
      return false;
    }
    if((expectedRole && (currentRole !== expectedRole))){
      console.log('Access denied, not authorized. Redirecting to home page...');
      this.router.navigate(['/home']);
      return false;
    }
    return true;
  }
}
