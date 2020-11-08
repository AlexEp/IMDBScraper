import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-actor-roles',
  templateUrl: './actor-roles.component.html',
  styleUrls: ['./actor-roles.component.scss']
})
export class ActorRolesComponent implements OnInit {

  @Input('roles') roles : string[];

  constructor() { }

  ngOnInit(): void {
  }

}
