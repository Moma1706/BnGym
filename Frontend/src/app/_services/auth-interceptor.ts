import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Observable, catchError, throwError } from "rxjs";
import { AccountService } from '../_services/account.service';
import { Injectable } from "@angular/core";


@Injectable({ providedIn: 'root' })
export class AuthInterceptor implements HttpInterceptor {
    constructor(private authService: AccountService) {} 
   intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {  
      const token = this.authService.getAuthToken();
  
     if (token) {
       // If we have a token, we set it to the header
       request = request.clone({
          setHeaders: {Authorization: `Bearer ${token}`}
       });
    }
  
    return next.handle(request).pipe(
        catchError((err) => {
          if (err instanceof HttpErrorResponse) {
              if (err.status === 401) {
              // redirect user to the logout page
           }
        }
        return throwError(err);
      })
     )
    }
  }