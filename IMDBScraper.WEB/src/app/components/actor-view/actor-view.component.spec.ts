import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ActorViewComponent } from './actor-view.component';

describe('ActorViewComponent', () => {
  let component: ActorViewComponent;
  let fixture: ComponentFixture<ActorViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ActorViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ActorViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
