import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-popup',
  imports: [CommonModule],
  templateUrl: './popup.html',
  styleUrl: './popup.scss',
})
export class Popup {
  @Input() title :string = '';
  @Input() isOpen:boolean = false;
  @Input() showFooter:boolean = true;

  @Output() closePopup = new EventEmitter<void>();
  @Output() ConfimAction = new EventEmitter<void>();

  close()
  {
    this.closePopup.emit();
  }
  confirm()
  {
    this.ConfimAction.emit();
  }
}
