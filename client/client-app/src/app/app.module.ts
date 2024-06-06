import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { FooterComponent } from './footer/footer.component';
import { MenuComponent } from './menu/menu.component';
import { AdministratorComponent } from './administrator/administrator.component';
import { AppRoutingModule } from './app-routing.module';
import { HomeComponent } from './home/home.component'; 
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { LoginComponent } from './login/login.component';
import { InvitationComponent } from './invitation/invitation.component';
import { CreateInvitationComponent } from './create-invitation/create-invitation.component';
import { MaintenanceComponent } from './maintenance/maintenance.component';
import { BuildingsComponent } from './buildings/buildings.component';
import { CreateBuildingComponent } from './create-building/create-building.component';
import { BuildingDetailComponent } from './building-detail/building-detail.component';
import { ConstructionCompanyComponent } from './construction-company/construction-company.component';
import { CategoryComponent } from './category/category.component';
import { ServiceRequestComponent } from './service-request/service-request.component';


@NgModule({
  declarations: [
    AppComponent,
    MenuComponent,
    FooterComponent,
    AdministratorComponent,
    HomeComponent,
    LoginComponent,
    InvitationComponent,
    CreateInvitationComponent,
    MaintenanceComponent,
    BuildingsComponent,
    CreateBuildingComponent,
    BuildingDetailComponent,
    ConstructionCompanyComponent,
    CategoryComponent,
    ServiceRequestComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
