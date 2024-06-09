import { Component } from '@angular/core';
import { AdminService } from '../services/admin.service';
import { User } from '../models/user.model';



@Component({
  selector: 'app-administrator',
  templateUrl: './administrator.component.html',
  styleUrls: ['./administrator.component.css']
})

export class AdministratorComponent {
  newAdmin: User = { email: '',lastName:'', password: '', name: ''};
  successMessage: string ="";
  errorMessage: string = "";

  constructor(private adminService: AdminService) { }

createAdmin(): void {
  this.adminService.createAdmin(this.newAdmin).subscribe(
    admin => {
      this.successMessage = 'Nuevo administrador creado con éxito.';
      this.errorMessage = '';
      this.resetForm();
    },
    error => {
        console.log('Error:', error.error.errorMessage);
        this.errorMessage = error.error.errorMessage;
      this.successMessage = "";
    }
  );
}

  private resetForm(): void {
    this.newAdmin = { email: '', lastName: '', password: '', name: '' };
  }
}
