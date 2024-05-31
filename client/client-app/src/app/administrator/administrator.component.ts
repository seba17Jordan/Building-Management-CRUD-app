import { Component } from '@angular/core';
import { AdminService } from '../services/admin.service';
import { User } from '../models/user.model';



@Component({
  selector: 'app-administrator',
  templateUrl: './administrator.component.html',
  styleUrls: ['./administrator.component.css']
})

export class AdministratorComponent {
  newAdmin: User = { Email: '',LastName:'', Password: '', Name: ''};

  constructor(private adminService: AdminService) { }

  createAdmin(event: Event): void {
    event.preventDefault(); 
    this.adminService.createAdmin(this.newAdmin).subscribe(admin => {
      console.log('Nuevo administrador creado:', admin);
    });
  }
}
