import { Component, Input, OnInit } from '@angular/core';
import { Actor } from 'src/app/models/actor.model';

@Component({
  selector: 'app-actor-view',
  templateUrl: './actor-view.component.html',
  styleUrls: ['./actor-view.component.scss']
})
export class ActorViewComponent implements OnInit {
  @Input('actor') actor : Actor;

  constructor() { }

  ngOnInit(): void {
  }

}
