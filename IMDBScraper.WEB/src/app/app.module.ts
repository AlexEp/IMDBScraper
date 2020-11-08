import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ActorViewComponent } from './components/actor-view/actor-view.component';
import { ActorService } from './services/actor-service.service';
import { HttpClientModule } from '@angular/common/http';
import { ActorRolesComponent } from './components/actor-roles/actor-roles.component';

@NgModule({
  declarations: [
    AppComponent,
    ActorViewComponent,
    ActorRolesComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
  ],
  providers: [ActorService],
  bootstrap: [AppComponent]
})
export class AppModule { }
