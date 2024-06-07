import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdministratorComponent } from './administrator/administrator.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './auth.guard';
import { InvitationComponent } from './invitation/invitation.component';
import { CreateInvitationComponent } from './create-invitation/create-invitation.component';
import { MaintenanceComponent } from './maintenance/maintenance.component';
import { BuildingsComponent } from './buildings/buildings.component';
import { BuildingDetailComponent } from './building-detail/building-detail.component';
import { CreateBuildingComponent } from './create-building/create-building.component';
import { ImportBuildingComponent } from './import-building/import-building.component';
import { ConstructionCompanyComponent } from './construction-company/construction-company.component';
import { CategoryComponent } from './category/category.component';
import { ServiceRequestComponent } from './service-request/service-request.component';
import { ReportsComponent } from './reports/reports.component';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' }, 
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
  { path: 'administrator', component: AdministratorComponent, canActivate: [AuthGuard], data: { expectedRole: 0 } },
  { path: 'create-invitation', component: CreateInvitationComponent, canActivate: [AuthGuard], data: { expectedRole: 0 } },
  { path: 'buildings', component: BuildingsComponent, canActivate: [AuthGuard]},
  { path: 'login', component: LoginComponent },
  { path: 'invitations', component: InvitationComponent },
  { path: 'maintenance', component: MaintenanceComponent, canActivate: [AuthGuard], data: { expectedRole: 1 } },
  { path: 'buildings/detail/:id', component: BuildingDetailComponent },
  { path: 'buildings/import', component: ImportBuildingComponent, canActivate: [AuthGuard], data: { expectedRole: 3 } },
  { path: 'buildings/create', component: CreateBuildingComponent, canActivate: [AuthGuard], data: { expectedRole: 3 } },
  { path: 'construction-company', component: ConstructionCompanyComponent, canActivate: [AuthGuard], data: {expectedRole: 3}},
  { path: 'category', component: CategoryComponent, canActivate: [AuthGuard], data: {expectedRole: 0}},
  { path: 'service-request', component: ServiceRequestComponent, canActivate: [AuthGuard], data: {expectedRole: 1}},
  { path: 'reports', component: ReportsComponent, canActivate: [AuthGuard], data: {expectedRole: 1}},
  { path: '**', redirectTo: '/login' } //Este siempre al final, para que redirija a la página de inicio si no encuentra la ruta
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
