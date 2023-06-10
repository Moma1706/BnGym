import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs';
import { AccountService } from 'src/app/_services/account.service';
import { AlertService } from 'src/app/_services/alert.service';

@Component({
  selector: 'app-forgot-pass',
  templateUrl: './forgot-pass.component.html',
  styleUrls: ['./forgot-pass.component.css']
})
export class ForgotPassComponent {
  form: FormGroup;
  loading = false;
  submitted = false;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private accountService: AccountService,
    private alertService: AlertService){
    this.form = formBuilder.group({
      title: formBuilder.control('', Validators.required)
    });
  }

  ngOnInit() {
    this.form = this.formBuilder.group({
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required],
    });
  }

  get f() { return this.form.controls; }

  newPassword(){
    if(!this.form.valid)
      return;
    this.loading = true;
    var email = this.route.snapshot.queryParamMap.get('email')?.split('?')[0];
    var token = this.route.snapshot.queryParamMap.get('email')?.split('token=')[1];

    let model = {
      "password": this.f['password'].value,
      "confirmPassword": this.f['confirmPassword'].value,
      "token": email,
      "email": token
    }
      this.accountService.resetPassword(model)
      .pipe(first()).subscribe({
        next: () => {
          this.router.navigateByUrl('/home')
        },
        error: (error : HttpErrorResponse) => {
          this.alertService.error(error.error.error);
          this.loading = false;
      }});
  }
}
