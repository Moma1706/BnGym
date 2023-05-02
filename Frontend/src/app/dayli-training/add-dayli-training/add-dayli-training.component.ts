import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { first } from 'rxjs';
import { AlertService } from 'src/app/_services/alert.service';
import { DayliTrainingService } from 'src/app/_services/dayli-training.service';

@Component({
  selector: 'app-add-dayli-training',
  templateUrl: './add-dayli-training.component.html',
  styleUrls: ['./add-dayli-training.component.css']
})
export class AddDayliTrainingComponent implements OnInit {

  model: any= {};
  form: FormGroup;
  submitted = false;
  loading: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private dayliTrainingService: DayliTrainingService,
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
      dateOfBirth: ['', Validators.required]
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
   
    console.log(this.model);

    this.dayliTrainingService.addTraining(this.model)
    .pipe(first())
            .subscribe({
                next: () => {
                    // get return url from query parameters or default to home page
                    const returnUrl ='/dayli-training/view-all-dayli';
                    this.router.navigateByUrl(returnUrl);
                },
                error: (error : HttpErrorResponse) => {
                  this.alertService.error(error.error.error);
                  this.loading = false;
              }
            });
  }

}
