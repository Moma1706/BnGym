import { Component } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';
import { Router } from '@angular/router';
import jwt_decode from 'jwt-decode';

@Component({
  selector: 'app',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  title = 'bn-gym';
  decodedToken: any;
  userId: number = 0;
 
  constructor(private router: Router) {
    this.getUserInfo();
  }

  getUserInfo() {
    const token = this.getToken();
    
    this.decodedToken = jwt_decode(token!);

    const userId = this.decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
    this.userId = userId;
  }

  getToken() {
    return localStorage.getItem("token");
  }

  logOut(){
    localStorage.setItem('token', '');
    this.router.navigate(['login']);
  }
}

