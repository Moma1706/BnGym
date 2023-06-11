import { GymUserService } from './../../_services/gym-user.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CheckInService } from 'src/app/_services/check-in.service';
import { first } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { AlertService } from 'src/app/_services/alert.service';

@Component({
  selector: 'app-view-gym-user',
  templateUrl: './view-gym-user.component.html',
  styleUrls: ['./view-gym-user.component.css']
})
export class ViewGymUserComponent implements OnInit {

  model:any = {};
  id: string | null = '' ;
  splited: string[] = [];
  numberOfMonths: number = 0;
  mjeseci: number[] = [0.5,1,3,6,12];
  form: FormGroup;
  submitted: boolean = false;
  loading: boolean = false;
  extend: boolean = false;
  formExtend: FormGroup;
  index: number = 0;


  constructor(private route: ActivatedRoute,private router:Router, private gymUserService: GymUserService,private alertService:AlertService, private formBuilder: FormBuilder,private formBuilder1: FormBuilder, private checkInService: CheckInService) {
    this.form = this.formBuilder.group({
      title: this.formBuilder.control('initial value', Validators.required)
    });

    this.formExtend = this.formBuilder1.group({
      title: formBuilder1.control('type', Validators.required)
    });
  }

  ngOnInit() 
  {
    this.id = this.route.snapshot.paramMap.get('id');
    this.getUser(this.id ?? '');

    this.form = this.formBuilder.group({
      firstname: ['', Validators.required],
      lastname: ['', Validators.required],
      email: ['', Validators.required],
      address: ['', Validators.required],
      userType: ['', Validators.required],
    });

    this.formExtend = this.formBuilder.group({
      type: ['', Validators.required],
    });
  }

  get f() { return this.form.controls; }
  get g() { return this.formExtend.controls; }

  getUser(id : string)
  {
    this.gymUserService.getUser(id).subscribe((response:any) =>{

      this.model = response;

      let ExpiresOn: string = this.model.expiresOn.toString();
      this.splited = ExpiresOn.split("T",2);
      this.model.expiresOn = this.splited[0];

      let lastCheckin:string = this.model.lastCheckIn.toString();
      this.splited = lastCheckin.split("T",2);
      this.model.lastCheckIn = this.splited[0];

      if (this.model.lastCheckIn == "0001-01-01" || this.model.lastCheckIn === "null"){
        this.model.lastCheckIn = "Nema ni jedan dolazak..."
      }
      else {
        const newDate = new Date(this.model.lastCheckIn);
        const timeZoneOffsetMs = newDate.getTimezoneOffset() * 60 * 1000; // Convert minutes to milliseconds
        const adjustedDate = new Date(newDate.getTime() - timeZoneOffsetMs);
        let isoDateString = adjustedDate.toLocaleString();
        this.model.lastCheckIn = isoDateString;
      }

      if (this.model.isFrozen == false){
        this.model.isFrozen = 'Ne';
        this.model.freezeDate = '';
      }
      else {
        this.model.isFrozen = 'Da';
        let freezDate: string = this.model.freezeDate.toString();
        this.splited = freezDate.split("T",2);

        let date = this.splited[0];
        const newDate = new Date(date);
        const timeZoneOffsetMs = newDate.getTimezoneOffset() * 60 * 1000; // Convert minutes to milliseconds
        const adjustedDate = new Date(newDate.getTime() - timeZoneOffsetMs);
        let isoDateString = adjustedDate.toLocaleString();

        this.model.freezeDate = isoDateString;
      }

      if(this.model.type == 0){
        this.model.type = 'Pola mjeseca';
        this.index = 0;
      }else if(this.model.type == 1){
        this.model.type = 'Mjesec dana';
        this.index = 1;
      }else if(this.model.type == 2){
        this.model.type = 'Tri mjeseca';
        this.index = 2;
      }else if(this.model.type == 3){
        this.model.type = 'Pola godine';
        this.index = 3;
      }else {
        this.model.type = 'Godinu dana';
        this.index = 4;
      }
    })
  }

  Freez(){
      this.gymUserService.freeze(this.model.id ?? '').subscribe((response:any) =>{
        const returnUrl ='/gym-user/all-gym-users';
        this.router.navigateByUrl(returnUrl);
        this.alertService.success(`Članarina korisnika ${this.model.firstName} je zamrznuta!`);
        
      });
  }

  Activate(){
    this.gymUserService.Activate(this.model.id ?? '').subscribe((response:any) =>{
      const returnUrl ='/gym-user/all-gym-users';
      this.router.navigateByUrl(returnUrl);
      this.alertService.success(`Korisnika ${this.model.firstName} aktiviran`);
    });;
  }

  Extend(){
    this.extend = true;
  }

  ExtendMembersip(){
    
    let type: any = this.g['type'].value;
    
    if(type == 'HalfMonth'){
      type = 0
    }else if(type == 'Month'){
      type = 1
    }else if(type == 'ThreeMonts'){
      type = 2
    }else if(type == 'HalfYear'){
      type = 3
    }else {
      type = 4
    }

    this.gymUserService.Extend(this.model.id ?? '', {'Type':type}).subscribe((response:any) =>{
      const returnUrl ='/gym-user/all-gym-users';
      this.router.navigateByUrl(returnUrl);
      this.alertService.success(`Uspiješno produžena članarina za člana: ${this.model.firstName} ${this.model.lastName}`);
    });;
  }

  Update(){
    if(this.f['firstname'].value != ''){
      this.model.firstName = this.f['firstname'].value;
    }
    if(this.f['lastname'].value != ''){
    this.model.lastName=this.f['lastname'].value;
    }
    if(this.f['email'].value != ''){
    this.model.email=this.f['email'].value;
    }
    if(this.f['address'].value != ''){
      this.model.address=this.f['address'].value;
    }
    if(this.f['userType'].value !=''){
      this.model.type = this.f['userType'].value;
    }

    if(this.model.type == 'HalfMonth'){
      this.model.type = 0
    }else if(this.model.type == 'Month'){
      this.model.type = 1
    }else if(this.model.type == 'ThreeMonts'){
      this.model.type = 2
    }else if(this.model.type == 'HalfYear'){
      this.model.type = 3
    }else {
      this.model.type = 4
    }

    this.gymUserService.Update(this.model.id?? '', this.model)
    .pipe(first())
      .subscribe({
          next: () => {
              const returnUrl ='/gym-user/all-gym-users';
              this.router.navigateByUrl(returnUrl);
              this.alertService.success('Korisnikov profil promenjen!')
          },
          error: (error : HttpErrorResponse) => {
            this.alertService.error(error.error.message);
            this.loading = false;
        }
      });
  }

  checkIn(){
  this.checkInService.checkIn(this.model.id)
    .pipe(first())
      .subscribe({
          next: () => {
              const returnUrl ='/checkIn-history/view-checkins-by-date';
              this.router.navigateByUrl(returnUrl);
              this.alertService.success(`Korisnik ${this.model.firstName} evidentiran!`)
          },
          error: (error : HttpErrorResponse) => {
            this.alertService.error(error.error.message);
            this.loading = false;
        }
      });
    
  }
}
