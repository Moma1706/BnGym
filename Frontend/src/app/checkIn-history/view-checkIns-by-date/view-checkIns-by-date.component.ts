import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { catchError, map, startWith, switchMap } from 'rxjs';
import { CheckInService } from 'src/app/_services/check-in.service';
import {MatTableModule} from '@angular/material/table';
import { TmplAstBoundAttribute } from '@angular/compiler';
import { MatDatepicker } from '@angular/material/datepicker';
import { FormControl, FormGroup } from '@angular/forms';


export interface checkIn {
  id: string;
  userId: string;
  firstName: string;
  lastName: string;
  email: string;
  checkInDate: Date;
  gymUserId: number;
}

export interface EmployeeTable {
  items: checkIn[];
  pageIndex: number;
  pageSize: number;
  count: number;
}

@Component({
  selector: 'app-view-checkIns-by-date',
  templateUrl: './view-checkIns-by-date.component.html',
  styleUrls: ['./view-checkIns-by-date.component.css']
})

export class ViewCheckInsByDateComponent implements OnInit {

  visible: boolean = true;
  id : string = '';
  pageSize: number = 5;
  pageNumber: number = 1;
  searchText : string = '';
  loading: boolean = false;
  totalUsers: number = 0;
  pageSizeOption: number[] = [5, 10, 25, 50, 100];
  EmpData: checkIn[] = [];
  empTable?: EmployeeTable;


  date: string = '';
  selected = new Date();

  model: any= {};
  submitted = false;

  displayedColumns: string[] = ['firstName','lastName', 'Buttons'];
  dataSource: MatTableDataSource<checkIn> = new MatTableDataSource();


  @ViewChild('paginator') paginator!: MatPaginator;
  @ViewChild('MatSort') sort!: MatSort;

  constructor(private checkInService: CheckInService) { 
  }

  ngOnInit() {
  }

  ngAfterViewInit() {
    this.getData();
  }

  getTableData$(pageNumber: number, pageSize: number, searchText: string) {

    let isoDateString = this.selected.toISOString();
    console.log(isoDateString) 

    this.date = isoDateString;

    return this.checkInService.getCheckInsByDate(isoDateString,pageSize, pageNumber, '')
  }

  getData(){
    this.dataSource.paginator = this.paginator!;
    if(this.paginator){
    this.paginator!.page
      .pipe(
        startWith({}),
        switchMap(() => {
          this.loading = true;
          return this.getTableData$(
            this.paginator!.pageIndex + 1,
            this.paginator!.pageSize, 
            ''
          ).pipe(catchError(() => observableOf(null)));
        }),
        map((empData) => {
          if (empData == null) return [];
          
          this.totalUsers = (<EmployeeTable>empData).count;
          this.loading = false;
          return (empData as EmployeeTable).items;
        })
      )
      .subscribe((empData) => {
        this.EmpData = empData;
        console.log(empData);
        this.dataSource = new MatTableDataSource(this.EmpData);
        this.dataSource.sort = this.sort;
      });
    }
  }
  updateTable(){
    this.getData();
  }
}
function observableOf(arg0: null): any {
  throw new Error('Function not implemented.');
}

