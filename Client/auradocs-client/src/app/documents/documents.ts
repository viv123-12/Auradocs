import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Button } from '../components/button/button';
import { Dropdown } from '../components/dropdown/dropdown';
import { CommonModule } from '@angular/common';
import { CreateFolderRequest, CreateNewDocumentRequest, DocumentManagerService } from '../services/document-manager-service';
import { DOCUMENT_SHARE_SCOPE_FILTER_OPTIONS, DOCUMENT_STATUS, DOCUMENT_STATUS_FILTER_OPTIONS, DOCUMENT_TYPES, DOCUMENT_TYPES_FILTER_OPTIONS, DropDownOptions, SHARE_SCOPE } from '../constants/app-constants';
import { Router, ActivatedRoute} from '@angular/router';
import { STRING_CONSTANTS } from '../constants/string-constants'; 

@Component({
  selector: 'app-documents',
  imports: [Button,Dropdown,CommonModule],
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
  public DocumentsSectionHeading:string = STRING_CONSTANTS.DOCUMENT_PAGE_TITLE;
  public myFoldersSectionHeader:string = STRING_CONSTANTS.MY_FOLDER_SECTION_HEADING;
  public myDocumentsSectionHeader:string = STRING_CONSTANTS.MY_DOCUMENTS_SECTION_HEADING;
  public docTypeDDList:DropDownOptions<DOCUMENT_TYPES>[] = DOCUMENT_TYPES_FILTER_OPTIONS;
  public statusTypeDDList: DropDownOptions<DOCUMENT_STATUS>[] = DOCUMENT_STATUS_FILTER_OPTIONS;
  public OwnerTypeDDList:DropDownOptions<SHARE_SCOPE>[] = DOCUMENT_SHARE_SCOPE_FILTER_OPTIONS;
  public docTypeDDLabel:number = 0;
  public docStatusDDLabel:number = 0;
  public docOwnerDDLabel:number = 0;
  public searchDocumentPlaceHolder:string = "Search Documents";
  public folderList:string[] = []; 
  public documentsArray:string[][] = [["PDF","Vivek","Approved"],["DOC","Team","Draft"],["DOC","Team","Draft"],["DOC","Team","Draft"],["DOC","Team","Draft"],["DOC","Team","Draft"],["DOC","Team","Draft"]];
  public documentsList:string[] = [];
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
  public currentFolderId:string|null ='';

  constructor(private documentManager:DocumentManagerService, private router:Router, private activatedRoute:ActivatedRoute, private cdr: ChangeDetectorRef){}
  ngOnInit(): void {
    this.getFoldersList();
    this.getDocumentsList();
    this.getRouteParameter();
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
              this.router.navigate(['/document-editor',res.body]);
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
      title: STRING_CONSTANTS.UNTITLED_FOLDER,
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

  private getFoldersList()
  {
    this.documentManager.getFolders().subscribe({
      next:
        res => {
          if(res.status == 200)
          {
            this.folderList = JSON.parse(res.body);
          }
        },
      error:
        error => {
          console.error(error);
        }
    });
  }

  private getDocumentsList()
  {
    this.documentManager.getDocuments().subscribe({
      next:
        res => {
          if(res.status == 200)
          {
            this.documentsList = JSON.parse(res.body);
            this.cdr.markForCheck();
          }
        },
      error:
        error => {
          console.error(error);
        }
    });
  }
}
