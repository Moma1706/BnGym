import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { GymUserService } from './../../_services/gym-user.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { catchError, first, map, startWith, switchMap } from 'rxjs';
import { AlertService } from 'src/app/_services/alert.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';


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
  isAllFrozen: boolean = false;

  model: any= {};
  submitted = false;

  displayedColumns: string[] = ['firstName','lastName','email','isInActive','isFrozen',
  'expiresOn', 'numberOfArrivalsCurrentMonth','numberOfArrivalsLastMonth','Buttons'];

  dataSource: MatTableDataSource<gymUser> = new MatTableDataSource();


  @ViewChild(MatPaginator) paginator?: MatPaginator;
  @ViewChild(MatSort) sort: MatSort = new MatSort();


  constructor(private gymUserService: GymUserService, private alertService: AlertService, private router: Router) {
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

    if (localStorage.getItem(('isAllFrozen')) == null || localStorage.getItem(('isAllFrozen')) == 'false') 
      this.isAllFrozen = false;
    else
      this.isAllFrozen = true;

    localStorage.setItem('isAllFrozen', this.isAllFrozen.toString());
    
    console.log(this.isAllFrozen);
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

  freezAll(){
    this.gymUserService.freezAll()
    .pipe(first())
    .subscribe({
        next: () => {
            this.isAllFrozen = true;
            localStorage.setItem('isAllFrozen', this.isAllFrozen.toString());

            const returnUrl ='/gym-user/all-gym-users';
            this.router.navigateByUrl(returnUrl);
            this.alertService.success('Svi korisnici zamrznuti!');

            if(this.sort.direction !== "desc")
              this.getData('', 0, '');
            else
              this.getData('', 1, '');
        },
        error: (error : HttpErrorResponse) => {
          this.alertService.error(error.error.message);
          this.loading = false;
        }
    });
  }

  activateAll(){
    this.gymUserService.activateAll()
    .pipe(first())
    .subscribe({
        next: () => {
            this.isAllFrozen = false;
            localStorage.setItem('isAllFrozen', this.isAllFrozen.toString());

            const returnUrl ='/gym-user/all-gym-users';
            this.router.navigateByUrl(returnUrl);
            this.alertService.success('Svi korisnici aktivirani!');
            
            if(this.sort.direction !== "desc")
              this.getData('', 0, '');
            else
              this.getData('', 1, '');
        },
        error: (error : HttpErrorResponse) => {
          this.alertService.error(error.error.message);
          this.loading = false;
        }
    });
  }

  clickMethodFreez() {
    if(confirm("Da li ste sigurni da želite sve da zamrznete? ")) {
      this.freezAll();
    }
  }
  clickMethodActivate() {
    if(confirm("Da li ste sigurni da želite sve da aktivirate? ")) {
      this.activateAll();
    }
  }
}
function observableOf(arg0: null): any {
  throw new Error('Function not implemented.');
}
