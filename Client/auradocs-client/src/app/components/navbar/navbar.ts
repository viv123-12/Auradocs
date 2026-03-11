import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { Component } from '@angular/core';
import { Authentication } from '../../services/authentication';

@Component({
  selector: 'app-navbar',
  imports: [
    CommonModule,
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss',
})
export class Navbar {
  leftNavItems = ["Home" ,"Features", "About", "Contact Us", "Blog"]
  letfNavRoutes = ["/","/features","/about","/contact","/blog"]
  rightNavItems = ["Login","Signup"];
  rightNavRoutes = ["/login","/register"]
  constructor(public auth:Authentication, private router:Router){
    
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
