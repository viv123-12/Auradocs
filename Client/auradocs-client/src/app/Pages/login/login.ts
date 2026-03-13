import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserType } from '../../shared/enums/user-type.enum';
import { Switch } from '../../components/switch/switch';
import { CommonModule } from '@angular/common';
import { Authentication, LoginRequest } from '../../services/authentication';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, Switch, CommonModule],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  private loginRequest:LoginRequest;
  loginForm = new FormGroup({
    email:new FormControl('',[Validators.required,Validators.email]),
    organizationId: new FormControl('',Validators.pattern(/^\d{10}$/)),
    password: new FormControl('',[Validators.required]),
    rememberMe: new FormControl(false)
  });
  public userLoginTypes:string[] = [UserType.INDIVIDUAL,UserType.ORGANIZATION];
  public selectedUserType:string = UserType.INDIVIDUAL;
  public isOrganizationUserType:boolean = false;

  constructor(private router: Router, private auth:Authentication) {
    this.loginRequest = {
      userId: '',
      userType:'',
      password:''
    }
  }

  get f() {
    return this.loginForm.controls;
  }

  onFormSubmission(){
    if (!this.loginForm.valid)
    {
      console.error("Login form is invalid!");
      return ;
    }
    const loginFormValue = this.loginForm.value;
    this.loginRequest = {
      userId: (this.isOrganizationUserType ? loginFormValue.organizationId : loginFormValue.email) ?? '',
      password: loginFormValue.password ?? '',
      userType: this.selectedUserType
    }
    this.auth.login(this.loginRequest, this.selectedUserType).subscribe({
      next:
        res => {
          if(res.status == 200)
          {
            this.router.navigate(['/']);
            this.auth.isLoggedIn = true;
          }
        },
      error:
        error => {
          this.loginForm.reset();
        }
    })
  }

  onClickForgotPassword()
  {
    this.router.navigate(['/verify-account']);
  }

  onClickCreateAccount()
  {
      this.router.navigate(['/register']);
  }

  public onToogleValuechange(value:string){
    this.selectedUserType = value;
    if(value == UserType.INDIVIDUAL)
    { 
      this.isOrganizationUserType = false;
      const organizationIdControl = this.loginForm.get('organizationId');
      organizationIdControl?.setValidators([
        Validators.pattern(/^\d{10}$/)])
    }else{
      this.isOrganizationUserType = true;
      const organizationIdControl = this.loginForm.get('organizationId');
      organizationIdControl?.setValidators([
        Validators.required,
        Validators.pattern(/^\d{10}$/)])
      }
 }
}
