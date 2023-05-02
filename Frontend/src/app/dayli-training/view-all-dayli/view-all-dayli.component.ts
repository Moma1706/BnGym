import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { catchError, map, startWith, switchMap } from 'rxjs';
import { DayliTrainingService } from 'src/app/_services/dayli-training.service';

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
}

@Component({
  selector: 'app-view-all-dayli',
  templateUrl: './view-all-dayli.component.html',
  styleUrls: ['./view-all-dayli.component.css']
})
export class ViewAllDayliComponent implements OnInit {

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

  model: any= {};
  form!: FormGroup;
  submitted = false;

  displayedColumns: string[] = ['firstName','lastName','dateOfBirth', 'Buttons'];

  dataSource: MatTableDataSource<User> = new MatTableDataSource();


  @ViewChild('paginator') paginator?: MatPaginator;
  @ViewChild('MatSort') sort!: MatSort;

  constructor(private dayliService: DayliTrainingService) {
  }

  ngOnInit() {
  }

  ngAfterViewInit() {
    this.getData();
  }

  getTableData$(pageNumber: number, pageSize: number, searchText: string) {
    return this.dayliService.getAllDayliTrainings(pageSize, pageNumber, '');
  }
  
  applyFilter(event: Event) {
    let filterValue = (event.target as HTMLInputElement).value;
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.dataSource.filter = filterValue;
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
          
          this.totalUsers = (<UsersTable>empData).count;
          this.loading = false;
          return (empData as UsersTable).items;
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


}

function observableOf(arg0: null): any {
  throw new Error('Function not implemented.');
}


