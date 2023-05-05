import { GymUserService } from './../../_services/gym-user.service';
import { ActivatedRoute } from '@angular/router';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CheckInService } from 'src/app/_services/check-in.service';

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


  constructor(private route: ActivatedRoute, private gymUserService: GymUserService, private formBuilder: FormBuilder, private checkInService: CheckInService) {
    this.form = this.formBuilder.group({
      title: this.formBuilder.control('initial value', Validators.required)
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
      type: ['', Validators.required],
    });

  }

  get f() { return this.form.controls; }

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

      if(this.model.lastCheckIn == "0001-01-01"){
        this.model.lastCheckIn = "Nema ni jedan dolazak..."
      }

      if(this.model.isFrozen == false){
        this.model.isFrozen = 'Ne';
        this.model.freezeDate = '';
      }
      else{
        this.model.isFrozen = 'Da';
        let freezDate: string = this.model.freezeDate.toString();
        this.splited = freezDate.split("T",2);
        this.model.freezeDate = this.splited[0];
      }
      
      if(this.model.isInactive){
        this.model.isInactive = 'Da';
      }
      else{
        this.model.isInactive = 'Ne';
      }

      if(this.model.type == 0){
        this.model.type = 'Pola mjeseca'
      }else if(this.model.type == 1){
        this.model.type = 'Mjesec dana'
      }else if(this.model.type == 2){
        this.model.type = 'Tri mjeseca'
      }else if(this.model.type == 3){
        this.model.type = 'Pola godine'
      }else {
        this.model.type = 'Godinu dana'
      }

      console.log(this.model);
    })
  }

  Freez(){
      this.gymUserService.freeze(this.model.id ?? '').subscribe((response:any) =>{
        console.log(response);
        window.location.reload();
      });
  }

  Activate(){
    this.gymUserService.Activate(this.model.id ?? '').subscribe((response:any) =>{
      console.log(response);
      window.location.reload();
    });;
  }

  Extend(){
    this.gymUserService.Extend(this.model.id ?? '', {Type : 1}).subscribe((response:any) =>{
      console.log(response);
      window.location.reload();
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

    if(this.model.type == 'Pola mjeseca'){
      this.model.type = 0
    }else if(this.model == 'mjesec dana'){
      this.model.type = 1
    }else if(this.model.type == 'Tri mjeseca'){
      this.model.type = 2
    }else if(this.model.type == 'Pola Godine'){
      this.model.type = 3
    }else {
      this.model.type = 4
    }

    this.gymUserService.Update(this.model.id?? '', this.model).subscribe((response:any)=>{
      console.log(response);
      window.location.reload();
    });
  }

  checkIn(){
  this.checkInService.checkIn(this.model.id).subscribe((response:any)=>{
    console.log(response);
    window.location.reload();
  });
    
  }
}
