import { Component, OnInit } from '@angular/core';
import { Form, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { JwtHelperService, JWT_OPTIONS} from '@auth0/angular-jwt';
import jwt_decode from 'jwt-decode';
import { AccountService } from 'src/app/_services/account.service';
import { GymWorkerService } from 'src/app/_services/gym-worker.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  decodedToken: any;
  userid: number = 0;
  model: any = {};
  form: FormGroup = new FormGroup({
    firstName: new FormControl('', Validators.required),
    lastName: new FormControl('', Validators.required),
    email: new FormControl('', Validators.required),
  });
  submitted: boolean = false;
  loading: boolean = false;
  userId: string = '';

  constructor(private route: ActivatedRoute, private userService: UserService, private formBuilder: FormBuilder, private gymWorkerService: GymWorkerService) {

    
  }

  ngOnInit() {
    this.userId = this.route.snapshot.paramMap.get('id')!;
    this.getUserInfo();
  }

  getUserInfo() {
    this.userService.getUserData(+this.userId).subscribe((response:any) =>{
      console.log(response);
      this.model = response;
    });
  }

  get f() { return this.form.controls; }

  update(){

    if(this.f['firstName'].value != ''){
      this.model.firstName = this.f['firstName'].value;
    }
    if(this.f['lastName'].value != ''){
    this.model.lastName=this.f['lastName'].value;
    }
    if(this.f['email'].value != ''){
    this.model.email=this.f['email'].value;
    }
    
    this.gymWorkerService.update(this.model.userId, this.model).subscribe((response:any) =>{
      console.log(response);
    })
  }

}
