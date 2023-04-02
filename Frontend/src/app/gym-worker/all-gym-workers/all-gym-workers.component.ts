import { GymWorkerService } from './../../_services/gym-worker.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Router } from '@angular/router';


export interface gymWorker {
  id: number;
  firstName: string;
  lastName: string;
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

  displayedColumns: string[] = ['FirstName','LastName','Email', 'Buttons'];
  dataSource: MatTableDataSource<gymWorker> = new MatTableDataSource(this.DataSource);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort: MatSort = new MatSort();

  constructor(private router: Router, private gymWorkerService: GymWorkerService ) 
  {
    this.getAllWorkers();
  }

  ngOnInit() {
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  getAllWorkers()
  {
    this.gymWorkerService.getAllWorkers().subscribe((response:any) =>{

      this.allGymWorkers = response;
      console.log(response);
      this.allGymWorkers.forEach((element:{firstName:string, lastName:string, Email:string}) => {
        this.DataSource.push(element);
      });
      this.dataSource = new MatTableDataSource(this.DataSource);
    })
  }

}
