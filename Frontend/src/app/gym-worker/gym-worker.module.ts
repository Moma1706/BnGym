import { AllGymWorkersComponent } from './all-gym-workers/all-gym-workers.component';
import { AddGymWorkerComponent } from './add-gym-worker.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GymWorkerComponent } from './gym-worker.component';
import { GymWorkerRoutingModule } from './gym-worker-routing.module';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { ViewGymWorkerComponent } from './view-gym-worker/view-gym-worker.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSortModule } from '@angular/material/sort';


@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    GymWorkerRoutingModule,
    MatTableModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatFormFieldModule,
    MatInputModule,
    MatSortModule,
  ],
  declarations: [
    GymWorkerComponent,
    AddGymWorkerComponent,
    AllGymWorkersComponent,
    ViewGymWorkerComponent,
  ]
})
export class GymWorkerModule { }
