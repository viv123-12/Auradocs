import { CommonModule } from '@angular/common';
import { Component, ViewChild } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ElementRef } from '@angular/core';
import { Renderer2 } from '@angular/core';
import { Authentication } from '../../services/authentication';

@Component({
  selector: 'app-side-bar',
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './side-bar.html',
  styleUrl: './side-bar.scss',
})
export class SideBar {
  public sideBarItems:string[] = ["Dashboard", "Documents", "Templates", "Workflows","Trash","Storage", "Settings"];
  public sidebarRouterLinks:string[] = ["/","/documents","/templates","/workflows","/trash","/storage","/settings"];
  @ViewChild('sideNavigationAnchor') sideNavigationEle!:ElementRef;
  public selectedIndex:number=0;
  ngAfterViewInit()
  {
    this.selectedIndex = 0;
  }

  constructor(private elementRef:ElementRef, private renderer:Renderer2, public auth:Authentication, private router:Router){}
  onClick(index:number)
  {
    this.selectedIndex = index;
  }

  onClickLogout()
  {
    this.auth.logout().subscribe(
      {
        next:
          res => {
            if(res.status == 200)
            {
              this.auth.isLoggedIn = false;
              this.router.navigate(['/']);
            }
          },
        error:
          error => {
            console.error(error);
            return;
          }
      }
    )
  }
}

