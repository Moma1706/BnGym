import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { DayliTrainingService } from 'src/app/_services/dayli-training.service';
import { Router } from '@angular/router';
import { AlertService } from 'src/app/_services/alert.service';
import { first } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-view-dayli-user',
  templateUrl: './view-dayli-user.component.html',
  styleUrls: ['./view-dayli-user.component.css']
})
export class ViewDayliUserComponent implements OnInit {

  model:any = {};
  id: string | null = '' ;
  splited: string[] = [];
  numberOfMonths: number = 0;
  mjeseci: number[] = [0.5,1,3,6,12];
  form!: FormGroup;
  submitted: boolean = false;
  loading: boolean = false;

  constructor(private route: ActivatedRoute, private dayliService: DayliTrainingService, private formBuilder: FormBuilder, private router: Router, private alertservice: AlertService) { 
    this.form = this.formBuilder.group({
      title: this.formBuilder.control('initial value', Validators.required)
    });
  }

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');
    this.getUser(this.id ?? '');

    this.form = this.formBuilder.group({
      firstname: ['', Validators.required],
      lastname: ['', Validators.required],
      mberOfArrivalsCurrentMonth: ['', Validators.required],
      mberOfArrivalsLastMonth: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      lastCheckIn: ['', Validators.required]
    });
  }

  get f() { return this.form.controls; }

  getUser(id : string)
  {
    this.dayliService.getTraining(id).subscribe((response:any) =>{
      this.model = response;

      let birthDate: string = this.model.dateOfBirth.toString();
      this.splited = birthDate.split("T",2);
      this.model.dateOfBirth = this.splited[0];

      let newDate = new Date(this.model.lastCheckIn);
      const timeZoneOffsetMs = newDate.getTimezoneOffset() * 60 * 1000; // Convert minutes to milliseconds
      const adjustedDate = new Date(newDate.getTime() - timeZoneOffsetMs);
      let isoDateString = adjustedDate.toLocaleString();

      this.model.lastCheckIn = isoDateString;

      console.log(this.model);
    })
  }

  
  update(){

    if(this.f['firstname'].value != ''){
      this.model.firstName = this.f['firstname'].value;
    }
    if(this.f['lastname'].value != ''){
    this.model.lastName=this.f['lastname'].value;
    }
    
    this.dayliService.update(this.model.id, this.model).subscribe((response:any) =>{
      const returnUrl ='/dayli-training/view-all-dayli';
      this.router.navigateByUrl(returnUrl);
      this.alertservice.success("Profil korisnika promenjen!");
    });
  }

  addArrival(){

    this.dayliService.addArrival(this.model.id)
    .pipe(first())
      .subscribe({
        next: (response: any) => {
          const returnUrl ='/checkIn-history/view-checkins-by-date';
          this.router.navigateByUrl(returnUrl);
          this.alertservice.success('Evidentiran dolazak za korisnika:' + this.model.firstname +'!');

        },
        error: (error : HttpErrorResponse) => {
          this.alertservice.error(error.error.message);
          this.loading = false;
        }
      })
  }
}
