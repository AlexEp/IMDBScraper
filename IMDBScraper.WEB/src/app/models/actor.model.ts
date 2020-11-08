export class Actor {

    constructor(public id : string,
        public name : string ,
        public birthday : string,
        public roles : string[],
        public genderType : number,
        public imageURL : string) {
    }
}

export enum ActorGender {
    Unknown,
    Male,
    Female,
  }
