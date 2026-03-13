import { Component, signal } from '@angular/core';
import { MainLayoutComponent } from './Pages/main-layout-component/main-layout-component';
import { RouterOutlet } from '@angular/router';
import { Navbar } from './components/navbar/navbar';
import { LoaderService } from './services/loader-service';
import { Loader } from './loader/loader';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [ MainLayoutComponent, RouterOutlet, Navbar, Loader, CommonModule ],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('auradocs-client');
  constructor(public loaderService:LoaderService){}
}
