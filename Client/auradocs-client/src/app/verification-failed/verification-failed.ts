import { Component } from '@angular/core';
import { Button } from '../components/button/button';
import { CommonModule } from '@angular/common';
import { VERIFICATION_FAILURE_RESULTS } from '../constants/app-constants';
import { AURADOCS_BUTTON_CLASSES, STRING_CONSTANTS } from '../constants/string-constants';
import { Router } from '@angular/router';

@Component({
  selector: 'app-verification-failed',
  imports: [Button, CommonModule],
  templateUrl: './verification-failed.html',
  styleUrl: './verification-failed.scss',
})
export class VerificationFailed {
  public backBtn:string = AURADOCS_BUTTON_CLASSES.VEFICATION_FAILED_BACK_BTN_CLASS;
  public backBtnTitle:string = STRING_CONSTANTS.BACK_TO_LOGIN;
  public backBtnInnerText:string = STRING_CONSTANTS.BACK_TO_LOGIN;
  public verificationFailureReasons:string[] = VERIFICATION_FAILURE_RESULTS;

  public constructor(private router:Router){};
  public onClickBackBtn()
  {
    this.router.navigate(['/login']);
  }
}
