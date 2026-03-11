import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Button } from '../components/button/button';
import { UserType } from '../shared/enums/user-type.enum';
import { Switch } from '../components/switch/switch';
import { CommonModule } from '@angular/common';
import { Authentication, VerifyAccountRequest as VerifyAccountRequest } from '../services/authentication';

@Component({
  selector: 'app-verify-account',
  imports: [ReactiveFormsModule , Button, Switch, CommonModule],
  templateUrl: './verify-account.html',
  styleUrl: './verify-account.scss',
})
export class VerifyAccount implements OnInit{
  public verifyAccountFormGroup = new FormGroup({
    email: new FormControl(''),
    organizationId:new FormControl('')
  });
  public isCurrentViewVerifyAccount:boolean = false;
  public isOrganizationUserType:boolean = false;
  public selectedUserType:string = UserType.INDIVIDUAL;
  public verifySendLinkBtnInnerText: string = "Verify & Send Link";
  public verifySendLinkBtnTitle:string = "Verify & Send Link";
  public verifySendLinkBtnClass:string = "verify-send-link-btn";
  public backBtnClass:string = "verify-account-back-btn";
  public backBtnTitle:string = "Back";
  public backBtnInnerText:string = "Back";
  public modesOfRegistration:string[] = [UserType.INDIVIDUAL,UserType.ORGANIZATION];
  private verifyAccountRequest: VerifyAccountRequest;
  constructor(private auth:Authentication){
    this.verifyAccountRequest = {
      userType : '',
      identifier: ''
    }
  }
  ngOnInit(): void {
    const emailIdControl = this.verifyAccountFormGroup.get('email');
    emailIdControl?.setValue('');
  }
  
  onClickVerifyBtn(){
    if (!this.verifyAccountFormGroup.valid)
    {
      console.error("Form is invalid");
      return;
    }
    const formData = this.verifyAccountFormGroup.value;
    this.verifyAccountRequest = {
      userType : this.selectedUserType ?? '',
      identifier: (this.selectedUserType == UserType.INDIVIDUAL ? formData.email ?? '' : formData.organizationId ?? '')
    }
    this.auth.verifyAccount(this.verifyAccountRequest).subscribe(
      {
        next:
          res => {
            if(res.status == 200)
            {
              this.isCurrentViewVerifyAccount = true;
            }
          },
        error: 
          error => {
            console.error(error);
          }
      }
    );
  }

  onClickBackBtn()
  {
    this.isCurrentViewVerifyAccount = false;
  }

  get form()
  {
    return this.verifyAccountFormGroup.controls;
  }

  public onToogleValuechange(value:string){
    this.selectedUserType = value;
    const organizationIdControl = this.verifyAccountFormGroup.get('organizationId');
    const emailIdControl = this.verifyAccountFormGroup.get('email');
    if(value == UserType.INDIVIDUAL)
    { 
      this.isOrganizationUserType = false;
      organizationIdControl?.setValue('');
    }else{
      this.isOrganizationUserType = true;
      emailIdControl?.setValue('');
    } 
 }
}
