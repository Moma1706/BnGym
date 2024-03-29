import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { Router } from '@angular/router';
import { first } from 'rxjs';
import { AlertService } from 'src/app/_services/alert.service';
import { DailyTrainingService } from 'src/app/_services/daily-training.service';
import {formatDate} from '@angular/common'

@Component({
  selector: 'app-add-daily-training',
  templateUrl: './add-daily-training.component.html',
  styleUrls: ['./add-daily-training.component.css']
  
})
export class AddDailyTrainingComponent implements OnInit {

  model: any= {};
  form: FormGroup;
  submitted = false;
  loading: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private dailyTrainingService: DailyTrainingService,
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
      dateOfBirth: [formatDate(new Date(), "dd/MM/yyyy", 'en'), Validators.required]
    });
  }

  get f() { return this.form.controls; }

  onSubmit() {
    this.submitted = true;
    this.alertService.clear();

    if (this.form.invalid) {
      return;
    }
  }

  save(){
    this.model.firstName=this.f['firstname'].value;
    this.model.lastName=this.f['lastname'].value;
    this.model.dateOfBirth=this.f['dateOfBirth'].value;

    this.dailyTrainingService.addTraining(this.model)
    .pipe(first())
            .subscribe({
                next: () => {
                    // get return url from query parameters or default to home page
                    const returnUrl ='/daily-training/view-all-daily';
                    this.router.navigateByUrl(returnUrl);
                    this.alertService.success('Dodat dnevni korisnik!')
                },
                error: (error : HttpErrorResponse) => {
                  this.alertService.error(error.error.message);
                  this.loading = false;
              }
            });
  }

}
