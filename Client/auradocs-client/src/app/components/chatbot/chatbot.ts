import { Component } from '@angular/core';
import { Button } from '../button/button';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-chatbot',
  imports: [Button,CommonModule],
  templateUrl: './chatbot.html',
  styleUrl: './chatbot.scss',
  
})
export class Chatbot {
  public sendButtonTitle:string = "Send";
  public sendButtonInnerText:string = "Send";
  public sendBbtnClass:string= "chatbot-send-btn";
  public chatwindowHeader:string = "AI Assistant";
  public messageInputPlaceholder:string="Type a Message..";
  public showChatWindow:boolean = false;
  public initialMessage:string="How may I help You?";

  constructor(){}

  onClickopenChatWindow(){
    this.showChatWindow = !this.showChatWindow;
  }
}
