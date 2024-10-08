import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  mail: string = '';
  password: string = '';
  error: string = '';

  constructor (private authService: AuthService, private router: Router) {}

  login(): void {
    this.authService.login(this.mail, this.password).subscribe(
      (response) => {
        this.router.navigate(['/home']);
      },
      (error) => {
        this.error = error.error.errorMessage;
      }
    );
  }
}
