import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ActorRolesComponent } from './actor-roles.component';

describe('ActorRolesComponent', () => {
  let component: ActorRolesComponent;
  let fixture: ComponentFixture<ActorRolesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ActorRolesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ActorRolesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
