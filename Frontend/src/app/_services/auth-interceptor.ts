import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Observable, catchError, throwError } from "rxjs";
import { AccountService } from '../_services/account.service';
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";


@Injectable({ providedIn: 'root' })
export class AuthInterceptor implements HttpInterceptor {
    constructor(private authService: AccountService, private router: Router) {}
   intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      const token = this.authService.getAuthToken();

     if (token) {
       request = request.clone({
          setHeaders: {Authorization: `Bearer ${token}`}
       });
    }

    return next.handle(request).pipe(
        catchError((err) => {
          if (err instanceof HttpErrorResponse) {
              if (err.status === 401 && !request.url.includes("login")) {
              // redirect user to the logout page
              localStorage.setItem('token', '');
              window.location.href=''
           }
        }
        return throwError(err);
      })
     )
    }
  }
