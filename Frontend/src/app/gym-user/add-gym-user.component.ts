import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { first } from 'rxjs';
import { AlertService } from '../_services/alert.service';
import { GymUserService } from '../_services/gym-user.service';

@Component({
  selector: 'app-add-gym-user',
  templateUrl: './add-gym-user.component.html',
  styleUrls: ['./add-gym-user.component.css']
})
export class AddGymUserComponent implements OnInit {

  model: any= {};
  form: FormGroup;
  submitted = false;
  loading: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private gymUserService: GymUserService,
    private alertService: AlertService 
  ) 
  { 
    this.form = this.formBuilder.group({
      title: this.formBuilder.control('initial value', Validators.required)
    });
  }

  ngOnInit() {
    this.form = this.formBuilder.group({
      firstname: ['', Validators.required],
      lastname: ['', Validators.required],
      email: ['', Validators.required],
      address: ['', Validators.required],
      type: ['', Validators.required],
    });
  }

  onSubmit() {
    this.submitted = true;
    this.alertService.clear();
    // stop here if form is invalid
    if (this.form.invalid) {
      return;
    }
  }

  get f() { return this.form.controls; }

  add()
  {
    this.model.firstName=this.f['firstname'].value;
    this.model.lastName=this.f['lastname'].value;
    this.model.email=this.f['email'].value;
    this.model.address=this.f['address'].value;
    if (this.f['type'].value == 'HalfMonth'){
      this.model.type=0; 
    } else if(this.f['type'].value == 'Month'){
      this.model.type=1; 
    } else if(this.f['type'].value == 'ThreeMonts'){
      this.model.type=2; 
    } else if(this.f['type'].value == 'HalfYear'){
      this.model.type=3; 
    } else {
      this.model.type = 4;
    }

    this.gymUserService.addGymUser(this.model)
    .pipe(first())
      .subscribe({
          next: () => {
              const returnUrl ='/gym-user/all-gym-users';
              this.router.navigateByUrl(returnUrl);
              this.alertService.success('Dodat novi korisnik!');
          },
          error: (error : HttpErrorResponse) => {
            this.alertService.error(error.error.message);
            this.loading = false;
          }
          
      });
  }
}
