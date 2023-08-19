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
  pageSize: number = 25;
  pageNumber: number = 1;
  searchText : string = '';
  loading: boolean = false;
  totalUsers: number = 0;
  pageSizeOption: number[] = [5, 10, 25, 50, 100];
  EmpData: checkIn[] = [];
  empTable?: EmployeeTable;

  filterValue = '';

  date: string = '';
  selected = new Date();

  model: any= {};
  submitted = false;

  displayedColumns: string[] = ['firstName','lastName', 'Buttons'];
  dataSource: MatTableDataSource<checkIn> = new MatTableDataSource();


  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort: MatSort = new MatSort();

  constructor(private checkInService: CheckInService) { 
  }

  ngOnInit() {
  }

  ngAfterViewInit() {
    if(this.sort.direction !== "desc")
      this.getData('', 0);
    else
      this.getData('', 1);;
  }

  getTableData$(pageNumber: number, pageSize: number, searchText: string, sortDirect : number) {

    const timeZoneOffsetMs = this.selected.getTimezoneOffset() * 60 * 1000; // Convert minutes to milliseconds
    const adjustedDate = new Date(this.selected.getTime() - timeZoneOffsetMs);
    let isoDateString = adjustedDate.toISOString();

    this.date = isoDateString;

    return this.checkInService.getCheckInsByDate(isoDateString, pageSize, pageNumber, searchText, sortDirect)
  }

  applyFilter(event: Event) {
    this.filterValue = (event.target as HTMLInputElement).value;
    this.filterValue = this.filterValue.trim(); // Remove whitespace
    this.filterValue = this.filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    if(this.sort.direction !== "desc")
      this.getData(this.filterValue, 0);
    else
      this.getData(this.filterValue, 1);
  }

  getData(filter: string, sortDirect: number){
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
            filter,
            sortDirect
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
        this.dataSource = new MatTableDataSource(this.EmpData);
        this.dataSource.sort = this.sort;
      });
    }
  }
  updateTable(){
    if(this.sort.direction !== "desc")
      this.getData(this.filterValue, 0);
    else
      this.getData(this.filterValue, 1);
  }
}
function observableOf(arg0: null): any {
  throw new Error('Function not implemented.');
}

