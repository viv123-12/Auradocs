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
  public getDocuments(folderId:string):Observable<HttpResponse<any>>
  {
    const URL = API_CONSTANTS.DOCUMENTS.GET_FILES + `/${folderId}`;
    return this.api.get(URL, this.options);
  }

  public getDocument(documentId:string):Observable<HttpResponse<any>>
  {
    const URL = API_CONSTANTS.DOCUMENTS.OPEN_DOCUMENT + `/${documentId}`;
    return this.api.get(URL, this.options);
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
    return this.api.post(URL,null,this.options);
  }

  public deleteDocument(documentId:string)
  {
    const URL = API_CONSTANTS.DOCUMENTS.DELETE_DOCUMENT + `/${documentId}`;
    return this.api.delete(URL,this.options);
  }

  public downloadDocument(documentId:string)
  {
    const URL = API_CONSTANTS.DOCUMENTS.DOWNLOAD_DOCUMENT + `/${documentId}`;
      const option = {
      observe: 'response' as const,
      responseType: 'blob' as const,
      withCredentials: true
    }; 
    return this.api.get(URL, option);
  }

  public getFolder(folderId:string):Observable<HttpResponse<any>>
  {
    const URL = API_CONSTANTS.DOCUMENTS.OPEN_FOLDER +  `/${folderId}`;
    return this.api.get(URL, this.options);
  }
}
