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

  displayedColumns: string[] = ['FirstName','LastName','Email','isInactive','isFrozen','ExpiresOn', 'Buttons'];

  dataSource: MatTableDataSource<gymUser> = new MatTableDataSource();


  @ViewChild('paginator') paginator?: MatPaginator;


  constructor(private gymUserService: GymUserService) { 
  }

  ngOnInit() {
  }

  getTableData$(pageNumber: number, pageSize: number, searchText: string) {
    return this.gymUserService.getAllUsers(pageSize, pageNumber, '');
  }

  ngAfterViewInit() {
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
      });
    }
  }


}
function observableOf(arg0: null): any {
  throw new Error('Function not implemented.');
}

