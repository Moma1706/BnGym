import { GymWorkerService } from './../_services/gym-worker.service';
import { AlertService } from './../_services/alert.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { first } from 'rxjs';

@Component({
  selector: 'app-add-gym-worker',
  templateUrl: './add-gym-worker.component.html',
  styleUrls: ['./add-gym-worker.component.css']
})
export class AddGymWorkerComponent implements OnInit {

  model: any= {};
  form: FormGroup;
  submitted = false;
  loading: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private gymWorkerService: GymWorkerService,
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
      email: ['', Validators.required]
    });
  }

  get f() { return this.form.controls; }


  onSubmit() {
    this.submitted = true;
    this.alertService.clear();
    // stop here if form is invalid
    if (this.form.invalid) {
      return;
    }
  }
  
  save(){
    this.model.firstName=this.f['firstname'].value;
    this.model.lastName=this.f['lastname'].value;
    this.model.email=this.f['email'].value;
   
    console.log(this.model);

    this.gymWorkerService.addGymWorker(this.model)
    .pipe(first())
            .subscribe({
                next: () => {
                    // get return url from query parameters or default to home page
                    const returnUrl ='/gym-worker/all-gym-workers';
                    this.router.navigateByUrl(returnUrl);
                },
                error: (error : HttpErrorResponse) => {
                  this.alertService.error(error.error.error);
                  this.loading = false;
              }
            });

  }

}
