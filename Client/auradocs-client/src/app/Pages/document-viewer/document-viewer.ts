import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Button } from '../../components/button/button';
import { CommonModule } from '@angular/common';
import { DocumentEditor } from '../document-editor/document-editor';
import { ActivatedRoute, Router } from '@angular/router';
import { STRING_CONSTANTS } from '../../constants/string-constants';
import { DocumentManagerService, UpdateDocumentRequest } from '../../services/document-manager-service';
import { AuradocsHelpler } from '../../constants/Hepler';
import { DocumentDetail } from '../../constants/Interfaces/DocumentDetail';
import { DOCUMENT_EDIOR_MODES } from '../../constants/app-constants';

@Component({
  selector: 'app-document-viewer',
  imports: [Button,CommonModule, DocumentEditor],
  templateUrl: './document-viewer.html',
  styleUrl: './document-viewer.scss',
})
export class DocumentViewer implements OnInit{
  public backBtnTitle:string = "Back";
  public backBtnClass:string = "docuemnt-viewer-header-back-btn";
  public backBtnInnerText:string = "←";
  public duplicateBtnTitle:string = "Duplicate";
  public duplicateBtnInnerText:string = "Duplicate";
  public duplicateBtnClass:string = "document-viewer-duplicate-btn";
  public saveBtnTitle:string = "Save";
  public saveBtnInnerText:string = "Save";
  public saveBtnClass:string = "document-viewer-save-btn";
  public shareBtnTitle:string = "Share";
  public shareBtnInnerText:string = "Share";
  public shareBtnClass:string = "document-viewer-share-btn";
  public documentTitle:string = STRING_CONSTANTS.UNTITLED_DOCUMENT;

  public sidebarDownloadBtnClass:string = "sidebar-download-btn";
  public sidebarDownloadBtnTitle:string = "Download";
  public sidebarDownloadBtnInnerText:string = "Download";

  public sidebarEditBtnClass:string = "sidebar-edit-btn";
  public sidebarEditBtnTitle:string = "Edit";
  public sidebarEditBtnInnerText:string = "Edit";

  public sidebarCommentBtnClass:string = "sidebar-comment-btn";
  public sidebarCommentBtnTitle:string = "Comment";
  public sidebarCommentBtnInnerText:string = "Comment";

  public sidebarMoreBtnClass:string = "sidebar-more-btn";
  public sidebarMoreBtnTitle:string = "More";
  public sidebarMoreBtnInnerText:string = "More";

  public ReadingMode:boolean = true;
  public currentDocument:DocumentDetail = {
    documentId:'',
    documentTitle:'',
    documentContent:'',
    documentState:'',
    createdBy:'',
    ownedBy:'',
    currentDocumentVersion:0
  }; 
  private documentId:string | null = '';
  public saveDocument:UpdateDocumentRequest = {
      DocumentId:'',
      Title:'',
      Content:''
  }

  public constructor(private router:Router, public documentManager:DocumentManagerService, private activatedRoute:ActivatedRoute, private cdr: ChangeDetectorRef){
  }
  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe((param)=>{
      this.documentId = param.get('id');
      const mode = param.get('mode');

      this.ReadingMode = mode == DOCUMENT_EDIOR_MODES.READ;
    });
    AuradocsHelpler.ApiCallHelper(this.documentManager.getDocument.bind(this.documentManager), (res)=> {
      this.currentDocument = JSON.parse(res)
      this.cdr.markForCheck();
    }, ()=>{
    }, this.documentId);
  }

  public onTitleChange(event: Event)
  {
    this.currentDocument.documentTitle = (event.target as HTMLElement).innerText;
  }

  public onBackButtonClick()
  {
    this.router.navigate(['/documents'])
  }

  public onContentChange(editorContent:string)
  {
    this.currentDocument.documentContent = editorContent;
  }

  public onSaveBtnClicked()
  {
    this.saveDocument = {
      DocumentId:this.documentId ,
      Title: this.currentDocument.documentTitle,
      Content: this.currentDocument.documentContent
    }
    AuradocsHelpler.ApiCallHelper(this.documentManager.saveDocument.bind(this.documentManager), ()=> {
      console.log("Document Has saved");
    }, ()=>{
      
    }, this.saveDocument);
  }
  public onShareBtnClicked()
  {
    console.log("Share button clicked!!");
  }
  public onDuplicateBtnClicked()
  {
    AuradocsHelpler.ApiCallHelper(this.documentManager.duplicateDocument.bind(this.documentManager), () => {
      console.log("Document Duplicated")
    },()=>{
      
    },this.documentId);
    this.router.navigate(['/documents'])
  }
  public onClickEditBtn()
  {
    this.router.navigate(['/document-editor',this.documentId,'edit']);
  }
  public onClickDownloadBtn()
  {
    AuradocsHelpler.ApiCallHelper(this.documentManager.downloadDocument.bind(this.documentManager),(res)=>{
      const blob = new Blob([res], { type: 'application/pdf' });

      const fileURL = window.URL.createObjectURL(blob);

      const a = document.createElement('a');
      a.href = fileURL;
      a.download = 'document.pdf';
      a.click();

      window.URL.revokeObjectURL(fileURL);
    },()=> {}, this.documentId);
  }
  public onClickCommentBtn()
  {
    console.log("comment btn clicked");
  }
}

