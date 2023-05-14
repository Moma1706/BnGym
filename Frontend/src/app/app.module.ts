import { AlertComponent } from './_components/alert/alert.component';
import { HomeComponent } from './home/home.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AccountService } from './_services/account.service';
import { AuthInterceptor } from './_services/auth-interceptor';
import { MatFormFieldModule} from '@angular/material/form-field';  
import { OpenLayersMapComponent } from './home/map/open-layers-map/open-layers-map.component';

// called on every request to retrieve the token
export function jwtOptionsFactory(tokenService: AccountService) {
  return tokenService.getAuthToken();
}

@NgModule({
  imports: [
    BrowserAnimationsModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatFormFieldModule,
  ],
  declarations: [	
    AppComponent,
    HomeComponent,
    AlertComponent,
    OpenLayersMapComponent,
   ],
  providers: [{provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true}, JwtHelperService],
  bootstrap: [AppComponent]
})
export class AppModule { }
