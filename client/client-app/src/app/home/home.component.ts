import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  username: string | null = '';
  email: string | null = '';
  role: string | null = '';
  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.username = this.authService.getUsername();
    this.email = this.authService.getEmail();
    this.role = this.authService.getUserRole();
  }

  getRoleLabel(state: string | undefined): string {
    switch(state) {
      case '0':
        return 'Administrator';
      case '1':
        return 'Manager';
      case '2':
        return 'Maintenance';
      case '3':
        return 'Construction Company Administrator';
      default:
        return 'Unknown';
    }
  }
}
