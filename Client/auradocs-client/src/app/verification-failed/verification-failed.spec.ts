import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VerificationFailed } from './verification-failed';

describe('VerificationFailed', () => {
  let component: VerificationFailed;
  let fixture: ComponentFixture<VerificationFailed>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VerificationFailed]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VerificationFailed);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
