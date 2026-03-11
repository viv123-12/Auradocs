import { Component } from '@angular/core';
import { Navbar } from '../components/navbar/navbar';
import { ReactiveFormsModule } from '@angular/forms';
import { SideBar } from '../components/side-bar/side-bar';
import { Button } from '../components/button/button';
import { DocumentViewer } from '../document-viewer/document-viewer';
import { Router } from '@angular/router';

@Component({
  selector: 'app-main-layout-component',
  imports: [Navbar,
    ReactiveFormsModule,
    SideBar,
    Button
  ],
  templateUrl: './main-layout-component.html',
  styleUrl: './main-layout-component.scss',
})
export class MainLayoutComponent {

  public constructor(private router:Router){}
  onGetStartedButtonClicked()
  {
    this.router.navigate(['/login']);
  }
  onWatchDemoButtonClicked()
  {
    
  }
}
