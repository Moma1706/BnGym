import { AllGymWorkersComponent } from './all-gym-workers/all-gym-workers.component';
import { NgModule, Component } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddGymWorkerComponent } from './add-gym-worker.component';
import { GymWorkerComponent } from './gym-worker.component';

const routes: Routes = [
    {
        path: '', component: GymWorkerComponent,
        children: [
            { path: 'add-gym-worker', component: AddGymWorkerComponent },
            { path: 'all-gym-workers', component: AllGymWorkersComponent },

        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class GymWorkerRoutingModule { }