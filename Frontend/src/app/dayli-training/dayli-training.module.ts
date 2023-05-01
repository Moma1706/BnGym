import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DayliTrainingComponent } from './dayli-training.component';
import { DayliTrainingRoutingModule } from './dayli-training-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { AddDayliTrainingComponent } from './add-dayli-training/add-dayli-training.component';

@NgModule({
  imports: [
    CommonModule,
    DayliTrainingRoutingModule,
    ReactiveFormsModule
  ],
  declarations: 
  [ DayliTrainingComponent,
    AddDayliTrainingComponent
  ]
})
export class DayliTrainingModule { }
