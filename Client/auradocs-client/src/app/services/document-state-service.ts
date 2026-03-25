import { Injectable } from '@angular/core';
import { DocumentDetail } from '../constants/Interfaces/DocumentDetail';

@Injectable({
  providedIn: 'root',
})
export class DocumentStateService {
  private documents: DocumentDetail[] = [];
  private currentFolderId:string | null = null;
  public setCurrentFolderId(folderId: string)
  {
    this.currentFolderId = folderId;
  }
  public getCurrentFolderId()
  {
    return this.currentFolderId;
  }
  public setDocuments(data: DocumentDetail[]) {
    this.documents = data;
  }

  public getDocuments() {
    return this.documents;
  }
}
