import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Button } from '../components/button/button';
import { CommonModule } from '@angular/common';
import { Authentication, ResetPasswordRequest } from '../services/authentication';
import { INPUTTYPES } from '../constants/app-constants';

@Component({
  selector: 'app-reset-password',
  imports: [ReactiveFormsModule, Button, CommonModule],
  templateUrl: './reset-password.html',
  styleUrl: './reset-password.scss',
})
export class ResetPassword implements OnInit {
  public updatePasswordBtnClass:string = "update-password-btn";
  public sendButtonTitle:string = "Update Password";
  public sendButtonInnerText:string = "Update Password";
  public newPasswordType:string = "password";
  public confirmPasswordType:string = "password";
  public isNewPasswordVisible:boolean = false;
  public isConfirmPasswordVisible:boolean = false;
  private resetPasswordRequest:ResetPasswordRequest;
  private token:string | null='';
  public resetPasswordForm:FormGroup = new FormGroup({
    password:new FormControl('',[Validators.minLength(8), Validators.required, Validators.pattern(/^(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$/)]),
    confirmPassword: new FormControl('',[Validators.minLength(8),Validators.required, Validators.pattern(/^(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$/)])
  });

  public constructor(private router:Router, private auth:Authentication, private activatedRoute:ActivatedRoute){
    this.resetPasswordRequest = {
      token:'',
      password: ''
    }
  }
  ngOnInit(): void {
    this.getToken();
  }

  get form(){
    return this.resetPasswordForm.controls;
  }

  public onClickNewPasswordShow()
  {
    if(!this.isNewPasswordVisible)
    {
      this.newPasswordType = INPUTTYPES.TEXT;
    }
    else{
      this.newPasswordType = INPUTTYPES.PASSWORD;
    }
    this.isNewPasswordVisible = !this.isNewPasswordVisible;
  }

  public onClickConfirmPasswordShow()
  {
    if(!this.isConfirmPasswordVisible)
    {
      this.confirmPasswordType = INPUTTYPES.TEXT;
    }
    else{
      this.confirmPasswordType = INPUTTYPES.PASSWORD;
    }
    this.isConfirmPasswordVisible = !this.isConfirmPasswordVisible;
  }

  public onClickUpdatePasswordBtn()
  {
    if(!this.resetPasswordForm.valid)
    {
      console.error("form values are incorrect!")
      return;
    }
    const form = this.resetPasswordForm.value;
    if(form.confirmPassword != form.password)
    {
      console.error("Both fields should have same values!")
      return;
    }
    this.resetPasswordRequest = {
      token:this.token,
      password: form.password
    }
    this.auth.ResetPassword(this.resetPasswordRequest).subscribe({
      next:
        res => {
          if(res.status == 200)
          {
            this.router.navigate(['/login']);
          }
        },
      error:
        error => {
            console.error(error);
        }
    })
    
  }

  private getToken()
  {
    this.activatedRoute.queryParamMap.subscribe(param => {
      this.token = param.get('token');
    })
  }
 
}
