import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { catchError, map, startWith, switchMap } from 'rxjs';
import { DailyTrainingService } from 'src/app/_services/daily-training.service';

export interface User {
  id: number;
  firstName: string;
  lastName: string;
  birthDay?: Date;
}

export interface UsersTable {
  items: User[];
  pageIndex: number;
  pageSize: number;
  count: number;
  numberOfDayliArrivalsLastMonth: number;
  numberOfDayliArrivalsCurrentMonth: number;
}

@Component({
  selector: 'app-view-all-daily',
  templateUrl: './view-all-daily.component.html',
  styleUrls: ['./view-all-daily.component.css']
})
export class ViewAllDailyComponent implements OnInit {

  visible: boolean = true;
  id : string = '';
  pageSize: number = 5;
  pageNumber: number = 1;
  searchText : string = '';
  loading: boolean = false;
  totalUsers: number = 0;
  pageSizeOption: number[] = [5, 10, 25, 50, 100];
  EmpData: User[] = [];
  empTable!: UsersTable;
  filterValue = '';
  numberOfDayliArrivalsCurrentMonth = 0;
  numberOfDayliArrivalsLastMonth = 0;

  model: any= {};
  form!: FormGroup;
  submitted = false;

  displayedColumns: string[] = ['firstName','lastName','dateOfBirth', 'Buttons'];

  dataSource: MatTableDataSource<User> = new MatTableDataSource();


  @ViewChild(MatPaginator) paginator?: MatPaginator;
  @ViewChild(MatSort) sort: MatSort = new MatSort();

  constructor(private dailyService: DailyTrainingService) {
  }

  ngOnInit() {
  }

  ngAfterViewInit() {
    if(this.sort.direction !== "desc")
      this.getData('', 0);
    else
      this.getData('', 1);
  }

  getTableData$(pageNumber: number, pageSize: number, searchText: string, sortDirect : number) {
    return this.dailyService.getAllDailyTrainings(pageSize, pageNumber, searchText, sortDirect);
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
          this.totalUsers = (<UsersTable>empData).count;
          this.numberOfDayliArrivalsCurrentMonth = (<UsersTable>empData).numberOfDayliArrivalsCurrentMonth;
          this.numberOfDayliArrivalsLastMonth = (<UsersTable>empData).numberOfDayliArrivalsLastMonth;
          this.loading = false;
          return (empData as UsersTable).items;
        })
      )
      .subscribe((empData) => {
        this.EmpData = empData;
        this.dataSource = new MatTableDataSource(this.EmpData);
        this.dataSource.sort = this.sort;
      });
    }
  }

  getRecord(){
    if(this.sort.direction !== "desc")
      this.getData(this.filterValue, 0);
    else
      this.getData(this.filterValue, 1);
  }
}

function observableOf(arg0: null): any {
  throw new Error('Function not implemented.');
}


