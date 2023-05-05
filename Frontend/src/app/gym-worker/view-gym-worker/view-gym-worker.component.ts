import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { GymWorkerService } from 'src/app/_services/gym-worker.service';

@Component({
  selector: 'app-view-gym-worker',
  templateUrl: './view-gym-worker.component.html',
  styleUrls: ['./view-gym-worker.component.css']
})
export class ViewGymWorkerComponent implements OnInit {

  model:any = {};
  id: string | null = '' ;
  splited: string[] = [];
  numberOfMonths: number = 0;
  mjeseci: number[] = [0.5,1,3,6,12];
  form: FormGroup;
  submitted: boolean = false;
  loading: boolean = false;

  constructor(private route: ActivatedRoute, private gymWorkerService: GymWorkerService, private formBuilder: FormBuilder) { 
    this.form = this.formBuilder.group({
      title: this.formBuilder.control('initial value', Validators.required)
    });
  }

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');
    this.getWorker(this.id ?? '');

    this.form = this.formBuilder.group({
      firstname: ['', Validators.required],
      lastname: ['', Validators.required],
      email: ['', Validators.required],
      address: ['', Validators.required],
      type: ['', Validators.required],
    });
  }

  get f() { return this.form.controls; }

  getWorker(id : string)
  {
    this.gymWorkerService.getWorker(id).subscribe((response:any) =>{

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
    if(this.f['email'].value != ''){
    this.model.email=this.f['email'].value;
    }
    
    this.gymWorkerService.update(this.model.userId, this.model).subscribe((response:any) =>{
      console.log(response);
    })
  }

}
