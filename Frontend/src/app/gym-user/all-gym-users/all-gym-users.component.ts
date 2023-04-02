import { GymUserService } from './../../_services/gym-user.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

export interface gymUser {
  id: number;
  firstName: string;
  lastName: string;
}

@Component({
  selector: 'app-all-gym-users',
  templateUrl: './all-gym-users.component.html',
  styleUrls: ['./all-gym-users.component.css']
})

export class AllGymUsersComponent implements OnInit {

  allGymUsers: any = [];
  DataSource: any[] = [];
  visible: boolean = true;
  id : string = '';

  displayedColumns: string[] = ['FirstName','LastName','Email', 'Buttons'];
  dataSource: MatTableDataSource<gymUser> = new MatTableDataSource(this.DataSource);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort: MatSort = new MatSort();

  constructor(private gymUserService: GymUserService) { 
    this.getAllGymUsers();
  }

  ngOnInit() {
  }
  
  ngAfterViewInit(): void 
  {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  getAllGymUsers(){
    this.gymUserService.getAllUsers().subscribe((response:any) =>{

      this.allGymUsers = response;
      console.log(response);

      this.allGymUsers.forEach((element:{id:string, firstName:string, lastName:string, email:string}) => {
        this.DataSource.push(element);
        this.id = element.id;
      });
      this.dataSource = new MatTableDataSource(this.DataSource);
    })
  }
}
