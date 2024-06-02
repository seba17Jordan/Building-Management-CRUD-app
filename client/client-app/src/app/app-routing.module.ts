import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdministratorComponent } from './administrator/administrator.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './auth.guard';
import { InvitationComponent } from './invitation/invitation.component';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' }, 
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
  { path: 'administrator', component: AdministratorComponent, canActivate: [AuthGuard], data: { expectedRole: 0 } },
  { path: 'login', component: LoginComponent },
  { path: 'invitations', component: InvitationComponent },
  { path: '**', redirectTo: '/login' } //Este siempre al final, para que redirija a la página de inicio si no encuentra la ruta
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
