import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-button',
  imports: [CommonModule],
  templateUrl: './button.html',
  styleUrl: './button.scss',
})
export class Button {
  @Input() btnClass:string = 'btn-primary-Class';
  @Input() iconClass:string = 'btn-icon';
  @Input() isIcon:boolean = false; 
  @Input() disabled:boolean = false;
  @Input() buttonTitle:string = "Primary-btn";
  @Input() buttonInnerText:string = "";
  @Output() clicked = new EventEmitter<Event>();

  onclick(event:Event)
  {
    if(!this.disabled){
      this.clicked.emit(event);
    }
  }
}
