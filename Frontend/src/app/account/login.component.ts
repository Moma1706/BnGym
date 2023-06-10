import { AlertService } from './../_services/alert.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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
  previousEmail: string = '';

  constructor(
    private formBuilder: FormBuilder,
    private accountService: AccountService,
    private alertService: AlertService)   
  {
    this.form = formBuilder.group({
      title: formBuilder.control('', Validators.required)
    });
  }

  ngOnInit() {
    const storedEmail = localStorage.getItem('previousEmail');
    if (storedEmail) {
      this.previousEmail = storedEmail;
    };

    this.form = this.formBuilder.group({
      username: [this.previousEmail ?? '', Validators.required],
      password: ['', Validators.required]
    });

  }
  
  get f() { return this.form.controls; }
  
  onSubmit() {
    this.submitted = true;
    this.alertService.clear();
    // stop here if form is invalid
    if (this.form.invalid)
      return;
  }

  login() {
    if(this.form.valid){
      this.loading = true;
      this.accountService.login(this.f['username'].value, this.f['password'].value,)
      .pipe(first()).subscribe({
        next: () => {
          window.location.href="/checkIn-history/view-checkins-by-date"
          // Call the saveEmail() function to save the entered email
          const email = this.f['username'].value;
          this.saveEmail(email);
        },
        error: (error : HttpErrorResponse) => {
          this.alertService.error(error.error.error);
          this.loading = false;
      }});

    }
  }

  saveEmail(email: string) {
      this.previousEmail = email;
      localStorage.setItem('previousEmail', this.previousEmail);
  }
}