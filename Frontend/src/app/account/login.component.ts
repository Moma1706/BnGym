import { AlertService } from './../_services/alert.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { first } from 'rxjs/operators';
import { AccountService } from '../_services/account.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
})

export class LoginComponent implements OnInit {
  
  form: FormGroup;
  loading = false;
  submitted = false;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private accountService: AccountService,
    private alertService: AlertService
  )   
  {
    this.form = formBuilder.group({
      title: formBuilder.control('initial value', Validators.required)
  });
  }

  ngOnInit() {
    this.form = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
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

  login(){
    if(this.form.valid){
      this.loading = true;
      this.accountService.login(this.f['username'].value,this.f['password'].value,)
      .pipe(first())
            .subscribe({
                next: () => {
                    // get return url from query parameters or default to home page
                    window.location.reload();
                },
                error: (error : HttpErrorResponse) => {
                  this.alertService.error(error.error.error);
                  this.loading = false;
              },
            });
            const returnUrl ='/home';
            this.router.navigateByUrl(returnUrl);
    }
  }
}
