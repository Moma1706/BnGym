import { Component } from '@angular/core';
import { Router } from '@angular/router';
import jwt_decode from 'jwt-decode';
import { HttpTransportType, HubConnectionBuilder } from '@microsoft/signalr';
import { AlertService } from './_services/alert.service';
import { BASE_HUB_URL } from './config/api-hub-url.config';
import { NotificationService } from './_services/notification.service';
import { Notification } from './notification/interfaces/notification.interface';

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
  notifications: Notification[] = [];
  showNotifications = false;
  notificationsExist = false;
 
  constructor(private router: Router, private alertService: AlertService, private notificationService: NotificationService) {
    this.getUserInfo();
  }

  getUserInfo() {
    const token = this.getToken();
    this.token = token;
    this.role = localStorage.getItem('role') ?? '';

    if (token != ''){
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
    this.showNotifications = false;
  }

  ngOnInit(): void {
    const connection = new HubConnectionBuilder()
    .withUrl(`${BASE_HUB_URL}/notification`, {skipNegotiation:true, transport: HttpTransportType.WebSockets})
    .build();
    
    connection.start().then(function(){
      console.log("Connected!");
    }).catch(function(err){
      return console.error(err.toString());
    });

    // only logged in user can see notifications
    if (this.token) {
      // check if notification exists
      this.notificationService.getAll()
      .subscribe((response:any) => {
        this.notifications = response['notifications'];
        if (this.notifications.length !== 0)
          this.notificationsExist = true;
      });
    }

    // receives notifications from API
    connection.on("messageSent", (response) => { 
      this.notificationsExist = true;
      const notif = this.SplitValue(response)
      this.alertService.info(notif);

      this.notificationService.getAll()
        .subscribe((response:any) => {
          this.notifications = response['notifications'];
        });
    });

    // ovo sluzi samo za mijenjanje slike za bell
    connection.on("noNotifications", (response) => {
      this.notificationsExist = false;
    });
  }

  ShowNotifications() {
    if (this.showNotifications == false) {
      this.notificationService.getAll()
        .subscribe((response:any) => {
          this.showNotifications = true;
          this.notifications = response['notifications'];
        });
    }
    else
      this.showNotifications = false;
  }

  refreshNotification(){
    this.notificationService.getAll()
    .subscribe((response:any) => {
      this.notifications = response['notifications'];
    });
  }

  SplitValue(value: string): string {
    const splitStatus = value.split('Vrijeme:');
    const message: string = splitStatus[0].trim();
    return message;
  }
}