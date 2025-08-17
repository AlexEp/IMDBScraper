import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { ActorViewComponent } from './actor-view.component';

describe('ActorViewComponent', () => {
  let component: ActorViewComponent;
  let fixture: ComponentFixture<ActorViewComponent>;

  beforeEach(waitForAsync(() => {
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
