import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DayliTrainingComponent } from './dayli-training.component';
import { DayliTrainingRoutingModule } from './dayli-training-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { AddDayliTrainingComponent } from './add-dayli-training/add-dayli-training.component';
import { ViewAllDayliComponent } from './view-all-dayli/view-all-dayli.component';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSortModule } from '@angular/material/sort';
import { ViewDayliUserComponent } from './view-dayli-user/view-dayli-user.component';

@NgModule({
  imports: [
    CommonModule,
    DayliTrainingRoutingModule,
    ReactiveFormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    MatSortModule
  ],
  declarations: 
  [ DayliTrainingComponent,
    AddDayliTrainingComponent,
    ViewAllDayliComponent,
    ViewDayliUserComponent
  ]
})
export class DayliTrainingModule { }
