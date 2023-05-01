import { GymWorkerService } from './../../_services/gym-worker.service';
import { Component, OnInit, ViewChild, resolveForwardRef } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Router } from '@angular/router';
import { catchError, map, startWith, switchMap } from 'rxjs';


export interface gymWorker {
  id: number;
  firstName: string;
  lastName: string;
}

export interface EmployeeTable {
  items: gymWorker[];
  pageIndex: number;
  pageSize: number;
  count: number;
}

@Component({
  selector: 'app-all-gym-workers',
  templateUrl: './all-gym-workers.component.html',
  styleUrls: ['./all-gym-workers.component.css']
})
export class AllGymWorkersComponent implements OnInit {

  allGymWorkers: any = [];
  DataSource: any[] = [];
  visible: boolean = true;
  totalWorkers: number = 0;
  pageSize: number = 5;
  pageNumber: number = 1;
  loading: boolean = false;
  empTable?: EmployeeTable;
  EmpData: gymWorker[] = [];

  displayedColumns: string[] = ['FirstName','LastName','Email', 'Buttons'];
  dataSource: MatTableDataSource<gymWorker> = new MatTableDataSource(this.DataSource);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort: MatSort = new MatSort();

  constructor(private router: Router, private gymWorkerService: GymWorkerService ) 
  {
    
  }

  ngOnInit() {
  }

  getTableData$(pageNumber: number, pageSize: number, searchText: string) {
    return this.gymWorkerService.getAllWorkers(pageSize, pageNumber, '');
  }

  ngAfterViewInit(): void {
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
          ).pipe(catchError((errr) => this.observableOf(null)));
        }),
        map((empData) => {
          if (empData == null) return [];
          
          this.totalWorkers = (<EmployeeTable>empData).count;
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
  
  observableOf(arg0: null): any {
    throw new Error('Function not implemented.');
  }
}
