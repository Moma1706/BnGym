import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CheckInHistoryComponent } from './checkIn-history.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSortModule } from '@angular/material/sort';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { CheckInHistoryRoutingModule } from './checkIn-history-routing.module';
import { ViewCheckInsByDateComponent } from './view-checkIns-by-date/view-checkIns-by-date.component';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule} from '@angular/material/datepicker';
import { MatCardModule } from '@angular/material/card';



@NgModule({
  imports: [
    CommonModule,
    CheckInHistoryRoutingModule,
    ReactiveFormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    MatSortModule,
    MatNativeDateModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatCardModule
  ],
  declarations: [
    CheckInHistoryComponent,
    ViewCheckInsByDateComponent
  ]
})
export class CheckInHistoryModule { }
