import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { Router, ActivatedRoute} from '@angular/router';
import { Dropdown } from '../../components/dropdown/dropdown';
import { Button } from '../../components/button/button';
import { STRING_CONSTANTS } from '../../constants/string-constants';
import { CreateFolderRequest, CreateNewDocumentRequest, DocumentManagerService } from '../../services/document-manager-service';
import { DOCUMENT_EDIOR_MODES, DOCUMENT_SHARE_SCOPE_FILTER_OPTIONS, DOCUMENT_STATUS, DOCUMENT_STATUS_FILTER_OPTIONS, DOCUMENT_TYPES, DOCUMENT_TYPES_FILTER_OPTIONS, DropDownOptions, SHARE_SCOPE } from '../../constants/app-constants';
import { DocumentDetail } from '../../constants/Interfaces/DocumentDetail';
import { DocumentStateService } from '../../services/document-state-service';
import { AuradocsHelpler } from '../../constants/Hepler';
import { Popup } from "../../components/popup/popup";
import { FolderDetail } from '../../constants/Interfaces/FolderDetail';
import { resolve } from 'chart.js/helpers';
import { FormsModule } from '@angular/forms';

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
  public docTypeDDList:DropDownOptions<DOCUMENT_TYPES>[] = DOCUMENT_TYPES_FILTER_OPTIONS;
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
      this.documentManager.getFolders().subscribe({
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
      this.documentManager.getDocuments('').subscribe({
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
          if(res.body)
          {
            this.currentFolderDetails = res.body;
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
}
