import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DailyTrainingComponent } from './daily-training.component';
import { DailyTrainingRoutingModule } from './daily-training-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { AddDailyTrainingComponent } from './add-daily-training/add-daily-training.component';
import { ViewAllDailyComponent } from './view-all-daily/view-all-daily.component';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSortModule } from '@angular/material/sort';
import { ViewDailyUserComponent } from './view-daily-user/view-daily-user.component';

@NgModule({
  imports: [
    CommonModule,
    DailyTrainingRoutingModule,
    ReactiveFormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    MatSortModule
  ],
  declarations: 
  [ DailyTrainingComponent,
    AddDailyTrainingComponent,
    ViewAllDailyComponent,
    ViewDailyUserComponent
  ]
})
export class DailyTrainingModule { }
