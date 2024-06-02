import { Component } from '@angular/core';

@Component({
  selector: 'app-invitation',
  templateUrl: './invitation.component.html',
  styleUrls: ['./invitation.component.css']
})
export class InvitationComponent {
  mail: string = '';
  password: string = '';
  error: string = '';
  
  acceptInvitation() {
    console.log('Invitation accepted');
  }
}
