import { Component, Input, OnInit } from '@angular/core';
import { InvitationService } from '../services/invitation.service';

@Component({
  selector: 'app-create-invitation',
  templateUrl: './create-invitation.component.html',
  styleUrls: ['./create-invitation.component.css']
})
export class CreateInvitationComponent implements OnInit{
  mail: string = '';
  name: string = '';
  @Input() role? : number;
  //pongo una fecha de expiracion para que no de error
  expirationDate: Date = new Date();

  constructor(private invitationService: InvitationService) { }

  ngOnInit(): void {}

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
}
