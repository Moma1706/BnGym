import { Component } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  title = 'bn-gym';

 
  constructor(private router: Router) {

  }

  logOut(){
    localStorage.setItem('token', '');
    this.router.navigate(['login']);
  }
}
