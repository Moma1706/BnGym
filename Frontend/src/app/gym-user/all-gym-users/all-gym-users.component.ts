import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { GymUserService } from './../../_services/gym-user.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { catchError, map, startWith, switchMap } from 'rxjs';


export interface gymUser {
  id: number;
  firstName: string;
  lastName: string;
}

export interface EmployeeTable {
  items: gymUser[];
  pageIndex: number;
  pageSize: number;
  count: number;
}

@Component({
  selector: 'app-all-gym-users',
  templateUrl: './all-gym-users.component.html',
  styleUrls: ['./all-gym-users.component.css']
})

export class AllGymUsersComponent implements OnInit {

  visible: boolean = true;
  id : string = '';
  pageSize: number = 5;
  pageNumber: number = 1;
  searchText : string = '';
  loading: boolean = false;
  totalUsers: number = 0;
  pageSizeOption: number[] = [5, 10, 25, 50, 100];
  EmpData: gymUser[] = [];
  empTable?: EmployeeTable;
  filterValue = '';

  model: any= {};
  submitted = false;

  displayedColumns: string[] = ['firstName','lastName','email','isInactive','isFrozen',
  'expiresOn', 'numberOfArrivalsCurrentMonth','numberOfArrivalsLastMonth','Buttons'];

  dataSource: MatTableDataSource<gymUser> = new MatTableDataSource();


  @ViewChild(MatPaginator) paginator?: MatPaginator;
  @ViewChild(MatSort) sort: MatSort = new MatSort();


  constructor(private gymUserService: GymUserService) {
  }

  ngOnInit() {
  }

  getTableData$(pageNumber: number, pageSize: number, searchText: string, sortDirect : number, sortParam: string) {
    return this.gymUserService.getAllUsers(pageSize, pageNumber, searchText, sortDirect, sortParam);
  }

  ngAfterViewInit() {
    if(this.sort.direction !== "desc")
      this.getData('', 0, '');
    else
      this.getData('', 1, '');
  }

  applyFilter(event: Event) {
    this.filterValue = (event.target as HTMLInputElement).value;
    this.filterValue = this.filterValue.trim(); // Remove whitespace
    this.filterValue = this.filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    if(this.sort.direction !== "desc")
      this.getData(this.filterValue, 0, '');
    else
      this.getData(this.filterValue, 1, '');
  }

  getData(filter: string, sortDirect: number, sortParam: string){
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
            sortDirect, sortParam
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
  getRecord(sortParam: string){
    if(this.sort.direction !== "desc")
      this.getData(this.filterValue, 0, sortParam);
    else
      this.getData(this.filterValue, 1, sortParam);
  }
}
function observableOf(arg0: null): any {
  throw new Error('Function not implemented.');
}
