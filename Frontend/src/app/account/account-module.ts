import { LayoutComponent } from './layout.component';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { AccountRoutingModule } from './account-routing.module';
import { LoginComponent } from './login.component';
import { ProfileComponent } from './profile/profile.component';
import { JWT_OPTIONS, JwtHelperService } from '@auth0/angular-jwt';
import { ForgotPassComponent } from './forgot-password/forgot-pass/forgot-pass.component';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        AccountRoutingModule,

    ],
    declarations: [
        LoginComponent,
        LayoutComponent,
        ProfileComponent,
        ForgotPassComponent
    ],
    providers:[
        { provide: JWT_OPTIONS, useValue: JWT_OPTIONS },
        JwtHelperService
    ]
})
export class AccountModule { }
