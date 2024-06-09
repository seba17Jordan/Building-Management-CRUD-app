import { Component, Input, OnInit } from '@angular/core';
import { InvitationService } from '../services/invitation.service';
import { Invitation } from '../models/invitation.model';

@Component({
  selector: 'app-create-invitation',
  templateUrl: './create-invitation.component.html',
  styleUrls: ['./create-invitation.component.css']
})
export class CreateInvitationComponent implements OnInit{
  mail: string = '';
  name: string = '';
  @Input() role? : number = 1;
  expirationDate: Date = new Date();  //pongo una fecha de expiracion para que no de error
  error: string = '';
  success: string = '';
  errorDelete: string = '';
  
  invitations: Invitation[] = [];

  constructor(private invitationService: InvitationService) { }

  ngOnInit(): void {
    this.getInvitations();
  }

  createInvitation(): void{
    if(this.role == 3){
      this.role = 3;
    }
    if(this.role == 1){
      this.role = 1;
    }
    this.invitationService.createInvitation(this.mail, this.name, this.role!, this.expirationDate).subscribe(
      (response) => {
        this.error = '';
        this.success = 'Invitation created successfully';
        this.invitations.push(response);
      },
      (error) => {
        this.success = '';
        this.error = error.error.errorMessage;
      }
    );
  }

  delete(invitation: Invitation){
    this.invitationService.deleteInvitation(invitation.id!).subscribe(//Borrado fisico
      (response) => {
        this.errorDelete = '';
        this.invitations = this.invitations.filter(i => i !== invitation);  //Borrado visual
      },
      (error) => {
        this.errorDelete = error.error.errorMessage;
      }
    ); 
  }

  getInvitations(): void {
    this.invitationService.getInvitations()
      .subscribe(x => {
        this.invitations = x
      }
    );
  }
}
