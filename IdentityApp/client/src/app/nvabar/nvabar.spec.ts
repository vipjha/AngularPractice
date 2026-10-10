import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Nvabar } from './nvabar';

describe('Nvabar', () => {
  let component: Nvabar;
  let fixture: ComponentFixture<Nvabar>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Nvabar],
    }).compileComponents();

    fixture = TestBed.createComponent(Nvabar);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
