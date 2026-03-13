import { Injectable } from '@angular/core';
import { HttpClientService } from './http-client-service';
import { Observable } from 'rxjs';
import { HttpHeaders, HttpResponse } from '@angular/common/http';
import { API_CONSTANTS } from '../constants/api-endpoints';

export interface CreateNewDocumentRequest
{
  title:string;
  content:string;
  status:number;
  folderId:string|null;
}

export interface CreateFolderRequest
{
  title:string;
  parentFolderId:string|null;
}

export interface UpdateDocumentRequest
{
  DocumentId:string|null,
  Title:string,
  Content :string
}

@Injectable({
  providedIn: 'root',
})
export class DocumentManagerService {
  public options = {
     headers:new HttpHeaders({
        'Content-type':'application/json'
      }),
      observe:'response',
      responseType:'text',
      withCredentials:true
    }
  constructor(private api:HttpClientService){}

  public getFolders():Observable<HttpResponse<any>>
  {
    return this.api.get(API_CONSTANTS.DOCUMENTS.GET_FOLDERS, this.options);
  }
  public getDocuments():Observable<HttpResponse<any>>
  {
    return this.api.get(API_CONSTANTS.DOCUMENTS.GET_FILES, this.options);
  }

  public createNewDocument(newDocument: CreateNewDocumentRequest):Observable<HttpResponse<any>>
  {
    return this.api.post(API_CONSTANTS.DOCUMENTS.CREATE_DOCUMENTS, newDocument, this.options);
  }

  public createNewFolder(newFolder: CreateFolderRequest):Observable<HttpResponse<any>>
  {
    return this.api.post(API_CONSTANTS.DOCUMENTS.CREATE_FOLDER, newFolder, this.options);
  }

  public saveDocument(updateDocumentRequest:UpdateDocumentRequest)
  {
    const body = updateDocumentRequest;
    return this.api.put(API_CONSTANTS.DOCUMENTS.UPDATE_DOCUMENT, body, this.options);
  }

  public duplicateDocument(documentId:string | null)
  {
    const URL = API_CONSTANTS.DOCUMENTS.DUPLICATE_DOCUMENT + `/${documentId}`;
    return this.api.put(URL,this.options);
  }
}
