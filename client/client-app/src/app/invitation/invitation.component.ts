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
  errorAccept: string = '';
  errorReject: string = '';
  
  constructor(private invitationService: InvitationService) { }

  acceptInvitation() {
    this.invitationService.acceptInvitation(this.mail, this.password).subscribe(
      (response) => {
        console.log(response);
      },
      (error) => {
        this.errorAccept = error.error.errorMessage;
      }
    );
  }

  rejectInvitation() {
    this.invitationService.rejectInvitation(this.mailRejected).subscribe(
      (response) => {
        console.log(response);
      },
      (error) => {
        this.errorReject = error.error.errorMessage;
      }
    );
  }
}
