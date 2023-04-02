import { ViewGymUserComponent } from './view-gym-user/view-gym-user.component';
import { AllGymUsersComponent } from './all-gym-users/all-gym-users.component';
import { AddGymUserComponent } from './add-gym-user.component';
import { GymUserComponent } from './gym-user.component';
import { NgModule, Component } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
    {
        path: '', component: GymUserComponent,
        children: [
            { path: 'add-gym-user', component: AddGymUserComponent },
            { path: 'all-gym-users', component: AllGymUsersComponent },
            { path: 'view-gym-user/:id', component: ViewGymUserComponent },
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class GymUserRoutingModule { }