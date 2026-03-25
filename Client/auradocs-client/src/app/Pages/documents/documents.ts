import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { Router, ActivatedRoute} from '@angular/router';
import { Dropdown } from '../../components/dropdown/dropdown';
import { Button } from '../../components/button/button';
import { STRING_CONSTANTS } from '../../constants/string-constants';
import { CreateFolderRequest, CreateNewDocumentRequest, DocumentManagerService, UploadFileRequest } from '../../services/document-manager-service';
import { DOCUMENT_EDIOR_MODES, DOCUMENT_SHARE_SCOPE_FILTER_OPTIONS, DOCUMENT_STATUS, DOCUMENT_STATUS_FILTER_OPTIONS, FILE_TYPES, DOCUMENT_TYPES_FILTER_OPTIONS, DropDownOptions, SHARE_SCOPE, DOCUMENT_TYPES } from '../../constants/app-constants';
import { DocumentDetail } from '../../constants/Interfaces/DocumentDetail';
import { DocumentStateService } from '../../services/document-state-service';
import { AuradocsHelpler } from '../../constants/Hepler';
import { Popup } from "../../components/popup/popup";
import { FolderDetail } from '../../constants/Interfaces/FolderDetail';
import { resolve } from 'chart.js/helpers';
import { FormsModule } from '@angular/forms';
import { HttpEvent, HttpEventType } from '@angular/common/http';

@Component({
  selector: 'app-documents',
  imports: [Button, Dropdown, CommonModule, Popup, FormsModule],
  templateUrl: './documents.html',
  styleUrl: './documents.scss',
})
export class Documents implements OnInit{
  public newBtnTitle:string = "New";
  public newBtnClass:string = "new-doc-folder-btn";
  public newBtnIconClass:string = "new-doc-folder-icon-btn";
  public uploadTitle:string = "Upload";
  public uploadBtnClass:string = "upload-doc-folder-btn";
  public uploadBtnIconClass:string = "upload-doc-folder-icon-btn";
  public newFolderTitle:string = "New Folder";
  public newFolderBtnClass:string = "new-folder-btn";
  public myFoldersSectionHeader:string = STRING_CONSTANTS.MY_FOLDER_SECTION_HEADING;
  public myDocumentsSectionHeader:string = STRING_CONSTANTS.MY_DOCUMENTS_SECTION_HEADING;
  public docTypeDDList:DropDownOptions<FILE_TYPES>[] = DOCUMENT_TYPES_FILTER_OPTIONS;
  public statusTypeDDList: DropDownOptions<DOCUMENT_STATUS>[] = DOCUMENT_STATUS_FILTER_OPTIONS;
  public OwnerTypeDDList:DropDownOptions<SHARE_SCOPE>[] = DOCUMENT_SHARE_SCOPE_FILTER_OPTIONS;
  public docTypeDDLabel:number = 0;
  public docStatusDDLabel:number = 0;
  public docOwnerDDLabel:number = 0;
  public searchDocumentPlaceHolder:string = STRING_CONSTANTS.SEARCH_DOCUMENT_PLACEHOLDER;
  public folderList:FolderDetail[] = []; 
  public documentsList:DocumentDetail[] = [];
  public DeleteDocumentPopupHeader:string  = STRING_CONSTANTS.DELETE_POPUP;
  public openDeletePopup:boolean = false;
  public selectedDeleteDocumentId:string='';
  
  public NewFolderPopupHeader:string = STRING_CONSTANTS.NEW_FOLDER;
  public openNewFolderPopup:boolean = false;
  public currentFolderId:string|null ='';
  public newFoldername:string='';

  public UploadDocumentPopupHeader:string  = STRING_CONSTANTS.UPLOAD_FILE;
  public openUploadFilePopup:boolean = false;
  public uploadFileRequest:UploadFileRequest= {};
  public uploadFileMetadatShow:boolean = false;
  public uploadProgress:number = 0;
  public isUploading:boolean = false;
  public isUploadingFailed:boolean = false;

  public DocumentsSectionHeading:string = STRING_CONSTANTS.HOME_PAGE_TITLE;
  public createNewDocumentRequest:CreateNewDocumentRequest={
      title:'',
      content:'',
      status: DOCUMENT_STATUS.DRAFT,
      folderId:''
    };
  public createNewFolderRequest: CreateFolderRequest = {
      title:'',
      parentFolderId:''
    };
  public currentFolderDetails:FolderDetail ={
    folderId : '',
    folderTitle:'',
    createdBy:'',
    ownedBy:''
  }

  constructor(private documentManager:DocumentManagerService, private documentState: DocumentStateService, private router:Router, private activatedRoute:ActivatedRoute, private cdr: ChangeDetectorRef){}
  ngOnInit(): void {
    this.InitializeDocumentsPage();
  }
  private async InitializeDocumentsPage()
  {
    this.getRouteParameter();
    await this.getFolderDetails();
    await this.getFoldersList();
    await this.getDocumentsList();
    this.currentFolderDetails.folderTitle = this.currentFolderId ? this.currentFolderDetails.folderTitle : this.DocumentsSectionHeading;
  }
  public createNewDocument()
  {
    this.createNewDocumentRequest = {
      title: STRING_CONSTANTS.UNTITLED_DOCUMENT,
      content:'',
      status: DOCUMENT_STATUS.DRAFT,
      folderId: this.currentFolderId
    }
    this.documentManager.createNewDocument(this.createNewDocumentRequest).subscribe({
      next:
        res => {
          if(res.status == 200)
            {
              this.router.navigate(['/document-editor',res.body,'edit']);
            }
          },
      error:
        error => {
          console.error(error);
        }
    });
  }

  public createNewFolder()
  {
    this.createNewFolderRequest = {
      title: this.newFoldername,
      parentFolderId: this.currentFolderId
    }
    this.documentManager.createNewFolder(this.createNewFolderRequest).subscribe({
      next:
        res => {
          if (res.status == 200)
          {
            this.getFoldersList();
          }
        },
      error:
        error => {
          console.error(error);
          return;
        }
    });
  }

  private getRouteParameter()
  {
    this.activatedRoute.paramMap.subscribe((params) =>
      {
        this.currentFolderId = params.get('folderId');
      } 
    );
  }

  private getFoldersList():Promise<void>
  {
     return new Promise<void>((resolve, reject)=>{
      this.documentManager.getFolders(this.currentFolderId).subscribe({
        next:
          res => {
            if(res.status == 200)
            {
              this.folderList = JSON.parse(res.body);
            }
            resolve();
          },
        error:
          error => {
            console.error(error);
            reject();
          }
      });
    });
  }

  private getDocumentsList():Promise<void>
  {
    return new Promise<void>((resolve, reject)=>{
      this.documentManager.getDocuments(this.currentFolderId).subscribe({
        next:
          res => {
            if(res.status == 200)
            {
              if(res.body){
                this.documentsList = JSON.parse(res.body);
              }
              this.cdr.markForCheck();
              resolve();
            }
          },
        error:
          error => {
            console.error(error);
            reject();
          }
      });
    });
  }
  private getFolderDetails():Promise<void>
  {
    return new Promise<void>((resolve, reject)=>{
        if(!this.currentFolderId)
        {
          resolve();
          return;
        }
        AuradocsHelpler.ApiCallHelper(this.documentManager.getFolder.bind(this.documentManager),(res) => {
          if(res)
          {
            this.currentFolderDetails = JSON.parse(res);
            this.documentState.setCurrentFolderId(this.currentFolderDetails.folderId);
          }
          resolve();
        }
        ,()=>{
          reject();
        },this.currentFolderId);
    });
  }
  public OnClickFileTile(documentId:string)
  {
    this.router.navigate(['/document-editor', documentId, DOCUMENT_EDIOR_MODES.READ]);
  }
  public async OnClickFolderTile(folderId:string)
  {
    this.currentFolderId = folderId;
    await this.getFolderDetails();
    this.router.navigate(['/documents', this.currentFolderDetails.folderId])
    await this.getFoldersList();
    await this.getDocumentsList();
    
  }
  public onClickDelete(event:MouseEvent, documentId:string)
  {
    event.stopPropagation();
    this.openDeletePopup = true;
    this.selectedDeleteDocumentId = documentId;
  }
  public onClickEditFile(event:MouseEvent, documentId:string)
  {
    event.stopPropagation();
    this.router.navigate(['/document-editor', documentId, DOCUMENT_EDIOR_MODES.EDIT]);
  }
  public onClickSharePoupOpen(event:MouseEvent, documentId:string)
  {
    event.stopPropagation();
  }
  public onClickCloseDeletePopup()
  {
    this.openDeletePopup = false;
  }
  public onClickDeletePopupConfirmBtn()
  {
    AuradocsHelpler.ApiCallHelper(this.documentManager.deleteDocument.bind(this.documentManager),()=>{},()=>{},this.selectedDeleteDocumentId);
    this.documentsList = this.documentsList.filter(d => d.documentId !== this.selectedDeleteDocumentId);
    this.openDeletePopup = false;
  }
  public onClickNewFolderBtn()
  {
    this.openNewFolderPopup = true;
  }
  public onClickCloseNewFolderPopup()
  {
    this.openNewFolderPopup = false;
  }
  public onClickCreateFolderConfimBtn()
  {
    this.createNewFolder();
    this.getFoldersList(); 
    this.openNewFolderPopup = false;
  }

  public onClickUploadDocument()
  {
    this.openUploadFilePopup = true;
  }

  public onClickUploadDocumentBtn()
  {
    this.isUploading = true;
    const uploadFormData = this.buildFormData();
    this.documentManager.uploadDocument(uploadFormData).subscribe(
      {
        next:
          (event:HttpEvent<any>) => {
            switch (event.type)
            {
              case HttpEventType.UploadProgress:
                if (event.total) {
                  this.uploadProgress = Math.round((100 * event.loaded) / event.total);
                }
                break;
              case HttpEventType.Response:
                this.isUploading = false;
                this.openUploadFilePopup = false;
                this.getDocumentsList();
                break;
            }
          },
        error:
          (error) =>{
            this.isUploading = false;
            this.isUploadingFailed = true;
            console.error(error);
          }
      }
    )
  }

  public onClickCloseUploadDocumentPopup()
  {
    this.openUploadFilePopup = false;
  }

  public onFileSelected(event:Event)
  {
    const inputFiles = event.target as HTMLInputElement;
    if(!inputFiles.files || inputFiles.files.length == 0)
    {
      return;
    }

    const file = inputFiles.files[0];

    this.uploadFileRequest.file = file;
    this.uploadFileRequest.Title = file.name;
    this.uploadFileRequest.FileType = file.type;
    this.uploadFileRequest.DocumentType = DOCUMENT_TYPES[2];
    this.uploadFileRequest.FolderId = this.currentFolderId;

    this.uploadFileMetadatShow = true;
  }

  private buildFormData():FormData
  {
    const formData = new FormData();
    this.uploadFileRequest.file && formData.append('File', this.uploadFileRequest.file);
    this.uploadFileRequest.Title && formData.append('Title', this.uploadFileRequest.Title);
    this.uploadFileRequest.FileType && formData.append('Filetype', this.uploadFileRequest.FileType);
    this.uploadFileRequest.DocumentType && formData.append('DocumentType', this.uploadFileRequest.DocumentType);
    if(this.uploadFileRequest.FolderId)
    {
      formData.append('FolderId', this.uploadFileRequest.FolderId);
    }
    return formData;
  }
}
