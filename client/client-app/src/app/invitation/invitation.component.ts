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
  successReject: string = '';
  successAccept: string = '';
  
  constructor(private invitationService: InvitationService) { }

  acceptInvitation() {
    this.invitationService.acceptInvitation(this.mail, this.password).subscribe(
      (response) => {
        this.errorAccept = '';
        this.successAccept = 'Invitation accepted successfully';
      },
      (error) => {
        this.successAccept = '';
        this.errorAccept = error.error.errorMessage;
      }
    );
  }

  rejectInvitation() {
    this.invitationService.rejectInvitation(this.mailRejected).subscribe(
      (response) => {
        console.log(response);
        this.errorReject = '';
        this.successReject = 'Invitation rejected successfully';
      },
      (error) => {
        this.successReject = '';
        this.errorReject = error.error.errorMessage;
      }
    );
  }
}
