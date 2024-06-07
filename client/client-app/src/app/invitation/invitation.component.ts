import { Component } from '@angular/core';
import { InvitationService } from '../services/invitation.service';

@Component({
  selector: 'app-invitation',
  templateUrl: './invitation.component.html',
  styleUrls: ['./invitation.component.css']
})
export class InvitationComponent {
  mail: string = '';
  mailRejected: string = '';
  password: string = '';
  error: string = '';
  
  constructor(private invitationService: InvitationService) { }

  acceptInvitation() {
    this.invitationService.acceptInvitation(this.mail, this.password).subscribe(
      (response) => {
        console.log(response);
      },
      (error) => {
        this.error = error.error.message;
      }
    );
  }

  rejectInvitation() {
    this.invitationService.rejectInvitation(this.mailRejected).subscribe(
      (response) => {
        console.log(response);
      },
      (error) => {
        this.error = error.error.message;
      }
    );
  }
}
