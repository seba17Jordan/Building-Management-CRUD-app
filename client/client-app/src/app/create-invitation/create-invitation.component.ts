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
  @Input() role? : number;
  expirationDate: Date = new Date();  //pongo una fecha de expiracion para que no de error
  
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
        console.log(response);
      },
      (error) => {
        console.log(error.error.message);
        console.log('Entro en un errorrrr');
      }
    );
  }

  delete(invitation: Invitation){
    this.invitationService.deleteInvitation(invitation.id!).subscribe(
      (response) => {
        this.invitations = this.invitations.filter(i => i !== invitation);  //Borrado visual
      },
      (error) => {
        console.log(error.error.message);
      }
    ); //Borrado fisico
  }

  getInvitations(): void {
    this.invitationService.getInvitations()
      .subscribe(x => {
        this.invitations = x
      }
    );
  }
}
