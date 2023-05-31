import { NgModule, Component } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from './layout.component';
import { LoginComponent } from './login.component';
import { ProfileComponent } from './profile/profile.component';
import { ForgotPassComponent } from './forgot-password/forgot-pass/forgot-pass.component';

const routes: Routes = [
    {
        path: '', component: LayoutComponent,
        children: [
            { path: 'login', component: LoginComponent },
            { path: 'profile/:id', component: ProfileComponent },
            { path: 'forgot-password', component: ForgotPassComponent },
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AccountRoutingModule { }
