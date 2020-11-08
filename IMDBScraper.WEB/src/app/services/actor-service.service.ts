import { Injectable } from '@angular/core';
import { HttpClient, HttpEvent, HttpErrorResponse, HttpEventType, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';

/* Rxjs */
import { Observable, of, throwError } from 'rxjs';
import { map, retry, catchError } from 'rxjs/operators';
import { Actor } from '../models/actor.model';


@Injectable({
  providedIn: 'root'
})
export class ActorService {

  private SERVER_API_URL: string = `${environment.backend.apiUrl}/actors`;

  constructor(private http: HttpClient) { }


  getActors() : Observable<Actor[]> {
    const url = `${this.SERVER_API_URL}`;
 
    return this.http.get<Actor[]>(url).pipe(
      map(res => res)
    )
  }
  
  public delete(actor: Actor) : Observable<string> {
    const uploadURL = `${this.SERVER_API_URL}`;

    return this.http.delete<string>(`${uploadURL}/${actor.id}`).pipe(  
       map(res => res),
    catchError(err => {
      //... Some ganeric code to deal with error
      return throwError(err);
    }),
    );
  }

  public unhideall() : Observable<void> {
    const uploadURL = `${this.SERVER_API_URL}`;

    return this.http.post<void>(`${uploadURL}/unhideall`,{}).pipe(  
       map(res => res),
    catchError(err => {
      //... Some ganeric code to deal with error
      return throwError(err);
    }),
    );
  }

  
  handleError(handleError: any): import("rxjs").OperatorFunction<string, any> {
    throw new Error("Method not implemented.");
  }
  
}






//  this.http.post("URL",fileData, { reportProgress: true, observe: 'events' })
//  .subscribe( event => {
//    if( event === HttpEventType.UploadProgress){
//     console.log(event.loaded / event.total * 100)
//    } 
//    else if ( event === HttpEventType.Sent){
//     console.log(event)
//    }
//  });