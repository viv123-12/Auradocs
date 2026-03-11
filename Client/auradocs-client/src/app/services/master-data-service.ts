import { Injectable } from '@angular/core';
import { HttpClientService } from './http-client-service';
import { API_CONSTANTS } from '../constants/api-endpoints';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MasterDataService {
  constructor(private auth:HttpClientService){}
  
  getDomainList():Observable<HttpResponse<any>>
  {
    const options = {
     headers:new HttpHeaders({
        'Content-type':'application/json'
      }),
      observe:'response',
      responseType:'text',
      withCredentials:true
    }
    return this.auth.get(API_CONSTANTS.MASTER_DATA_SERVICE.DOMAIN_DROPDOWN,options);
  }

  getPracticeAreaList():Observable<HttpResponse<any>>
  {
    const options = {
     headers:new HttpHeaders({
        'Content-type':'application/json'
      }),
      observe:'response',
      responseType:'text',
      withCredentials:true
    }
    return this.auth.get(API_CONSTANTS.MASTER_DATA_SERVICE.DOMAIN_PRACTICE_AREA_DROPDOWN,options);
  }
}
