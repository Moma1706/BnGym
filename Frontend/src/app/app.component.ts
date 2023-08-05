import { Component } from '@angular/core';
import { Router } from '@angular/router';
import jwt_decode from 'jwt-decode';
import { HttpTransportType, HubConnectionBuilder } from '@microsoft/signalr';
import { AlertService } from './_services/alert.service';
import {v4 as uuid} from 'uuid'

@Component({
  selector: 'app',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  title = 'bn-gym';
  decodedToken: any;
  userId: number = 0;
  token: string = '';
  role: string = '';
  notifications = new Map<string, string>();
  showNotifications = false;
 
  constructor(private router: Router, private alertService: AlertService) {
    this.getUserInfo();
  }

  getUserInfo() {
    const token = this.getToken();
    this.token = token;
    this.role = localStorage.getItem('role') ?? '';

    if(token != ''){
      this.decodedToken = jwt_decode(token!);
      const userId = this.decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
      this.userId = userId;
    }
  }

  getToken() {
    return localStorage.getItem('token') ?? '';
  }

  logOut(){
    localStorage.setItem('token', '');
    this.token = '';
    this.router.navigate(['login']);
  }

  ngOnInit(): void {
    const connection = new HubConnectionBuilder()
    .withUrl('https://localhost:7085/hub/notification', {skipNegotiation:true, transport: HttpTransportType.WebSockets})
    .build();
    
    connection.start().then(function(){
      console.log("Connected!");
    }).catch(function(err){
      return console.error(err.toString());
    });

    connection.on("messageSent", (response) => { 
      
      this.alertService.info(response);
      this.notifications.set(uuid(),response);

      console.log(response);
    });
  }

  ShowNotifications(){
    
    if(this.showNotifications == false)
      this.showNotifications = true;
    else
      this.showNotifications = false;
  }
}