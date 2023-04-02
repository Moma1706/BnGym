import { GymUserRoutingModule } from './gym-user-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GymUserComponent } from './gym-user.component';
import { AddGymUserComponent } from './add-gym-user.component';

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    GymUserRoutingModule
  ],
  declarations: [
    GymUserComponent,
    AddGymUserComponent
  ]
})
export class GymUserModule { }
