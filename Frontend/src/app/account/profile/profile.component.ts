import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { first } from 'rxjs';
import { AccountService } from 'src/app/_services/account.service';
import { AlertService } from 'src/app/_services/alert.service';
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
  changePasswordModel: any = {};

  form: FormGroup = new FormGroup({
    firstName: new FormControl('', Validators.required),
    lastName: new FormControl('', Validators.required),
    email: new FormControl('', Validators.required),
  });

  formPasswordChange: FormGroup = new FormGroup({
    currentPassword: new FormControl('', Validators.required),
    newPassword: new FormControl('', Validators.required),
    confirmNewPassword: new FormControl('', Validators.required),
  });

  submitted: boolean = false;
  loading: boolean = false;
  userId: string = '';
  changePasswordVisible: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private userService: UserService, 
    private gymWorkerService: GymWorkerService,
    private alertService: AlertService,
    private accountService: AccountService) {
  }

  ngOnInit() {
    this.userId = this.route.snapshot.paramMap.get('id')!;
    this.getUserInfo();
  }

  getUserInfo() {
    this.userService.getUserData(+this.userId).subscribe((response:any) =>{
      this.model = response;
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
  get g() { return this.formPasswordChange.controls; }

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

    this.gymWorkerService.update(this.model.id, this.model)
    .pipe(first())
      .subscribe({
        next: (response: any) => {
          this.alertService.success("Profil korisnika uspiješno ažuriran!");
        },
        error: (error : HttpErrorResponse) => {
          this.alertService.error(error.error.message);
          this.loading = false;
        }
      })
  }

  changePasswordVisibleEnable(){
    this.changePasswordVisible = true;
  }

  changePassword(){

    if(this.g['currentPassword'].value != ''){
      this.changePasswordModel.currentPassword = this.g['currentPassword'].value;
    }
    if(this.g['newPassword'].value != ''){
      this.changePasswordModel.newPassword=this.g['newPassword'].value;
    }
    if(this.g['confirmNewPassword'].value != ''){
      this.changePasswordModel.confirmNewPassword=this.g['confirmNewPassword'].value;
    }
    this.changePasswordModel.id = this.model.id;

    this.accountService.changePassword(this.changePasswordModel)
    .pipe(first())
      .subscribe({
        next: (response: any) => {
          this.alertService.success('Promijenjna lozinka!');
          this.changePasswordVisible = false;

        },
        error: (error : HttpErrorResponse) => {
          this.alertService.error(error.error.message);
          this.loading = false;
        }
      })
  }

}
