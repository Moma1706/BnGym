import { AddGymUserComponent } from './add-gym-user.component';
import { GymUserComponent } from './gym-user.component';
import { NgModule, Component } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
    {
        path: '', component: GymUserComponent,
        children: [
            { path: 'add-gym-user', component: AddGymUserComponent },
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class GymUserRoutingModule { }