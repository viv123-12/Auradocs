import { Injectable } from '@angular/core';
import  { HttpClientService} from "./http-client-service";
import { API_CONSTANTS } from '../constants/api-endpoints';
import { Observable } from 'rxjs';
import { HttpHeaders, HttpResponse } from '@angular/common/http';
import { UserType } from '../shared/enums/user-type.enum';
export interface RegisterRequest
{
  userId:string;
  phoneNumber:string;
  accountType:string;
  domainType:number;
  practiceArea:number;
}

export interface LoginRequest{
  userId:string;
  userType:string;
  password:string;
}

export interface VerifyAccountRequest{
  userType:string;
  identifier:string;
}

export interface ResetPasswordRequest{
  token:string|null;
  password: string;
}

@Injectable({
  providedIn: 'root',
})

export class Authentication {
  public isLoggedIn:boolean = false;
  public options = {
     headers:new HttpHeaders({
        'Content-type':'application/json'
      }),
      observe:'response',
      responseType:'text',
      withCredentials:true
    }
  constructor(private api:HttpClientService){
    this.CheckLoginState().subscribe((res) => {
      if(res.status == 200)
      {
        this.isLoggedIn = true;
      }
    });
  }
  public registerUser(registerUserRequest:RegisterRequest, userType:string):Observable<HttpResponse<any>>
  {
    const body = registerUserRequest
    return this.api.post(API_CONSTANTS.AUTH.REGISTER_USER,body,this.options);
  }

  public login(loginRequest:LoginRequest, userType:string):Observable<HttpResponse<any>>
  {
    const body = loginRequest;
    return this.api.post(API_CONSTANTS.AUTH.LOGIN,body,this.options);
  }

  public logout():Observable<HttpResponse<any>>
  {
    return this.api.get(API_CONSTANTS.AUTH.LOGOUT,this.options);
  }

  public verifyAccount(verifyAccount:VerifyAccountRequest):Observable<HttpResponse<any>>
  {
    const verifyAccountOtions = {
     headers:new HttpHeaders({
        'Content-type':'application/json'
      }),
      params: {
        userType: verifyAccount.userType,
        identifier: verifyAccount.identifier
      },
      observe:'response',
      responseType:'text',
      withCredentials:true
    }
    return this.api.get(API_CONSTANTS.AUTH.VERIFY_ACCOUNT, verifyAccountOtions);
  }

  public ResetPassword(resetPassword:ResetPasswordRequest):Observable<HttpResponse<any>>
  { 
    const body = resetPassword;
    return this.api.post(API_CONSTANTS.AUTH.RESET_PASSWORD, body, this.options);
  }

  public CheckLoginState():Observable<HttpResponse<any>>
  {
    return this.api.get(API_CONSTANTS.CHECK_LOGIN.CHECK_LOGIN,this.options);
  }
}
