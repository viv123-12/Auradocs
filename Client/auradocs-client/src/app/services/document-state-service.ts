import { Injectable } from '@angular/core';
import { DocumentDetail } from '../constants/Interfaces/DocumentDetail';

@Injectable({
  providedIn: 'root',
})
export class DocumentStateService {
  private documents: DocumentDetail[] = [];
  public setDocuments(data: DocumentDetail[]) {
    this.documents = data;
  }

  public getDocuments() {
    return this.documents;
  }
}
