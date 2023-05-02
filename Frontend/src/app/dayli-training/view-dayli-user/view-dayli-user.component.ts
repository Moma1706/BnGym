import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { DayliTrainingService } from 'src/app/_services/dayli-training.service';

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
  Meseci: number[] = [0.5,1,3,6,12];
  form!: FormGroup;
  submitted: boolean = false;
  loading: boolean = false;

  constructor(private route: ActivatedRoute, private dayliService: DayliTrainingService, private formBuilder: FormBuilder) { 
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
      dateOfBirth: ['', Validators.required]
    });
  }

  get f() { return this.form.controls; }

  getUser(id : string)
  {
    this.dayliService.getTraining(id).subscribe((response:any) =>{

      this.model = response;
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
    
    this.dayliService.update(this.model.Id, this.model).subscribe((response:any) =>{
      console.log(response);
    })
  }


}
