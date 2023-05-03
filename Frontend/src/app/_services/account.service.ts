import { LoginModel } from './../_models/login';
import { JwtHelperService } from '@auth0/angular-jwt';

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  decodedToken: any;
  userToken: any;
  jwtHelper = new JwtHelperService();

  baseUrlLogin = 'https://localhost:7085/api/Auth/login';
  baseUrl = 'https://localhost:7085/api/Auth';

  constructor(private http: HttpClient) { }

  login(email: any, password: any) {

    let loginModel = new LoginModel();
    loginModel.email = email;
    loginModel.password = password;
    
    return this.http.post(this.baseUrlLogin, loginModel).pipe(
      map((response: any) => {
        const user = response;
        console.log(user);
        if (user) {
          localStorage.setItem('token', user.token);
          this.decodedToken = this.jwtHelper.decodeToken(user.token);
          this.userToken = user.token;
        }
      })
    );
  }

  getAuthToken():string {
    return localStorage.getItem('token') ?? ''
  }

  changePassword(model: any){
    return this.http.post(this.baseUrl + '/change-password', model);
  }

}