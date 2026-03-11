import { Component, OnInit } from '@angular/core';
import { Button } from '../components/button/button';
import { CommonModule } from '@angular/common';
import { DocumentEditor } from '../document-editor/document-editor';
import { ActivatedRoute, Router } from '@angular/router';
import { STRING_CONSTANTS } from '../constants/string-constants';
import { DocumentManagerService, UpdateDocumentRequest } from '../services/document-manager-service';
import { AuradocsHelpler } from '../constants/Hepler';

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

  public EditingMode:boolean = true;
  public content:string = ''; 
  private documentId:string | null = '';
  public saveDocument:UpdateDocumentRequest = {
      documentId:'',
      title:'',
      content:''
  }

  public constructor(private router:Router, public documentManager:DocumentManagerService, private activatedRoute:ActivatedRoute){
    console.log("inside the constructor")
  }
  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe((param)=>{
      this.documentId = param.get('id');
    })
    console.log("inside the constructor")
  }

  public onTitleChange(event: Event)
  {
    this.documentTitle = (event.target as HTMLElement).innerText;
  }

  public onBackButtonClick()
  {
    this.router.navigate(['/documents'])
  }

  public onContentChange(editorContent:string)
  {
    this.content = editorContent;
  }

  public onSaveBtnClicked()
  {
    this.saveDocument = {
      documentId:'' ,
      title: this.documentTitle,
      content: this.content
    }
    AuradocsHelpler.ApiCallHelper(this.documentManager.saveDocument, ()=> {
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
    console.log("Duplicate Button clicked");
  }
}

