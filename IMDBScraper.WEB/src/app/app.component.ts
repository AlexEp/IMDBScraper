import { Component } from '@angular/core';
import { Actor, ActorGender } from './models/actor.model';
import { ActorService } from './services/actor-service.service';



@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Actors';
  actors: Actor[];

  constructor(
    private actorService: ActorService) { }

  ngOnInit(): void {
    this.actorService.getActors().subscribe(replay => {
        this.actors = replay;
    })
  }

  getGender(getGenderType: number) {
    if (getGenderType == <number>ActorGender.Female)  {
      return "Female"
    }
    else if (getGenderType == <number>ActorGender.Male)  {
      return "Male"
    }

    return ""
  }
  
  hideActor(actor:Actor) {
      this.actorService.delete(actor).subscribe(
        replay => {
          const actors =  this.actors.filter(a => a.id != replay);
          this.actors = actors;
        }
      );
  }

  unhideAll(){
    this.actorService.unhideall().subscribe(
      replay => {
        this.actorService.getActors().subscribe(replay => {
          this.actors = replay;
      })
      }
    );
  }

}
