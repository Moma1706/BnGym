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
  extend: boolean = false;
  formExtend: FormGroup;


  constructor(private route: ActivatedRoute, private gymUserService: GymUserService, private formBuilder: FormBuilder,private formBuilder1: FormBuilder, private checkInService: CheckInService) {
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

      if(this.model.userType == 0){
        this.model.userType = 'Pola mjeseca'
      }else if(this.model.userType == 1){
        this.model.userType = 'Mjesec dana'
      }else if(this.model.userType == 2){
        this.model.userType = 'Tri mjeseca'
      }else if(this.model.userType == 3){
        this.model.userType = 'Pola godine'
      }else {
        this.model.userType = 'Godinu dana'
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
    this.extend = true;
  }

  ExtendMembersip(){
    
    let type: any = this.g['type'].value;
    
    if(type == 'HalfMonth'){
      type = 0
    }else if(type == 'Month'){
      type = 1
    }else if(type == 'ThreeMonths'){
      type = 2
    }else if(type == 'HalfYear'){
      type = 3
    }else {
      type = 4
    }

    this.gymUserService.Extend(this.model.id ?? '', {'Type':type}).subscribe((response:any) =>{
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

    if(this.model.userType == 'Pola mjeseca'){
      this.model.userType = 0
    }else if(this.model == 'mjesec dana'){
      this.model.userType = 1
    }else if(this.model.userType == 'Tri mjeseca'){
      this.model.userType = 2
    }else if(this.model.userType == 'Pola Godine'){
      this.model.userType = 3
    }else {
      this.model.userType = 4
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
