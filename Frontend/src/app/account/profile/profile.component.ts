import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  decodedToken: any;

  constructor() {
    this.getUserInfo();
   }

  ngOnInit() {
  }

  getUserInfo() {
    const token = this.getToken();
    let payload;
    if (token) {
      payload = token.split(".")[1];
      payload = window.atob(payload);
      this.decodedToken =  JSON.parse(payload);
    }
  }
  
  getToken() {
    return localStorage.getItem("token");
  }

}
