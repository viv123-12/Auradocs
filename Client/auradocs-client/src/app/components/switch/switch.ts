import { Component, ElementRef, EventEmitter, Input, Output, Renderer2, ViewChild } from '@angular/core';

@Component({
  selector: 'app-switch',
  imports: [],
  templateUrl: './switch.html',
  styleUrl: './switch.scss',
})
export class Switch {
  @ViewChild('switchPoint') switchPoint!:ElementRef;
  @ViewChild('switchBox') switchBox!:ElementRef;
  @ViewChild('leftOption') leftOption!:ElementRef;
  @ViewChild('rightOption') rightOption!:ElementRef;
  @Input() leftLabel = "Option 1";
  @Input() rightLabel = "Option 2";
  @Input() _isTootleOn:boolean = true;
  @Output() _valueChanged = new EventEmitter<string>();
  private currentValue?:string;
  constructor(private renderer:Renderer2){}
  
  public toogleSwitch(){
    if(this._isTootleOn)
    {
      this.renderer.setStyle(this.switchPoint.nativeElement,'right','0');
      this.renderer.setStyle(this.switchBox.nativeElement,'backgroundColor','#4F46E5');
      this.currentValue = this.rightLabel;
    }else{
      this.renderer.removeStyle(this.switchPoint.nativeElement,'right');
      this.renderer.setStyle(this.switchBox.nativeElement,'backgroundColor','#FFF');
      this.renderer.setStyle(this.switchBox.nativeElement,'backgroundColor','white');
      this.currentValue = this.leftLabel;
    }
    this._isTootleOn = !this._isTootleOn;
    this._valueChanged.emit(this.currentValue);
  }
}
