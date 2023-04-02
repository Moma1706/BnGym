import { ViewGymUserComponent } from './view-gym-user/view-gym-user.component';
import { AllGymUsersComponent } from './all-gym-users/all-gym-users.component';
import { GymUserRoutingModule } from './gym-user-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GymUserComponent } from './gym-user.component';
import { AddGymUserComponent } from './add-gym-user.component';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    GymUserRoutingModule,
    MatTableModule,
    MatPaginatorModule
  ],
  declarations: [
    GymUserComponent,
    AddGymUserComponent,
    AllGymUsersComponent,
    ViewGymUserComponent
  ]
})
export class GymUserModule { }
