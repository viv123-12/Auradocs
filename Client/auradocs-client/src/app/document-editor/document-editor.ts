import { Component, EventEmitter, Output } from '@angular/core';
import { ChangeEvent, CKEditorModule } from '@ckeditor/ckeditor5-angular';

import {
  ClassicEditor,
  Essentials,
  Paragraph,
  Bold,
  Italic,
  Underline,
  Strikethrough,
  Heading,
  Link,
  List,
  TodoList,
  BlockQuote,
  Code,
  CodeBlock,
  Table,
  TableToolbar,
  TableProperties,
  TableCellProperties,
  Image,
  ImageToolbar,
  ImageCaption,
  ImageStyle,
  ImageResize,
  ImageUpload,
  MediaEmbed,
  HorizontalLine,
  FindAndReplace,
  Autoformat,
  PasteFromOffice,
  Markdown,
  FontFamily,
  FontColor,
  FontBackgroundColor,
  Base64UploadAdapter
} from 'ckeditor5';

@Component({
  selector: 'app-document-editor',
  standalone: true,
  imports: [CKEditorModule],
  templateUrl: './document-editor.html',
  styleUrl: './document-editor.scss'
})
export class DocumentEditor {

  public Editor = ClassicEditor;
  public isReadOnly  = false;
  public content = '';
  private editorInstance: any;
  @Output() ContentValueChanged = new EventEmitter<string>(); 

  constructor(){}

  public config = {
    licenseKey: 'GPL',

    plugins: [
      Essentials,
      Paragraph,
      Heading,

      Bold,
      Italic,
      Underline,
      Strikethrough,
      Code,
      CodeBlock,

      Link,
      List,
      TodoList,

      BlockQuote,
      HorizontalLine,

      Table,
      TableToolbar,
      TableProperties,
      TableCellProperties,

      Image,
      ImageToolbar,
      ImageCaption,
      ImageStyle,
      ImageResize,
      ImageUpload,

      MediaEmbed,

      FindAndReplace,
      Autoformat,
      PasteFromOffice,
      Markdown,

      FontFamily,
      FontColor,
      FontBackgroundColor,
      Base64UploadAdapter 
    ],

    toolbar: {
      items: [
        'undo', 'redo',
        '|',
        'heading',
        '|',
        'bold', 'italic', 'underline', 'strikethrough', 'code',
        '|',
        'bulletedList', 'numberedList', 'todoList',
        '|',
        'link', 'blockQuote', 'horizontalLine',
        '|',
        'insertTable', 'uploadImage', 'mediaEmbed',
        '|',
        'codeBlock', 'findAndReplace',
        '|',
        'fontFamily', 'fontSize', 'fontColor', 'fontBackgroundColor'
      ],
      shouldNotGroupWhenFull: true
    },

    table: {
      contentToolbar: [
        'tableColumn',
        'tableRow',
        'mergeTableCells',
        'tableProperties',
        'tableCellProperties'
      ]
    },

    image: {
      toolbar: [
        'imageTextAlternative',
        'toggleImageCaption',
        'imageStyle:inline',
        'imageStyle:block',
        'imageStyle:side'
      ]
    }
  };

  /** CKEditor lifecycle hook */
  onReady(editor: any) {
    this.editorInstance = editor;
    this.applyReadOnlyState();
  }

  /** 🔁 toggle method */
  toggleReadOnly() {
    this.isReadOnly = !this.isReadOnly;
    this.applyReadOnlyState();
  }

  /** ✅ proper API usage */
  private applyReadOnlyState() {
    if (!this.editorInstance) return;

    if (this.isReadOnly) {
      this.editorInstance.enableReadOnlyMode('app-readonly');
    } else {
      this.editorInstance.disableReadOnlyMode('app-readonly');
    }
  }

  onEditorValueChange(event:ChangeEvent<ClassicEditor>)
  {
    this.content = event.editor.getData();
    this.ContentValueChanged.emit(this.content);
  }
}