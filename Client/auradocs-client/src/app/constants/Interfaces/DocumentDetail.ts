export interface DocumentDetail
{
    documentId:string;
    documentTitle:string;
    documentContent:string;
    documentState:string;
    createdBy:string;
    ownedBy:string;
    currentDocumentVersion:number;
}