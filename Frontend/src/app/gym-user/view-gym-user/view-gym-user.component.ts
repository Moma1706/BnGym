import { GymUserService } from './../../_services/gym-user.service';
import { ActivatedRoute } from '@angular/router';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

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
  Meseci: number[] = [0.5,1,3,6,12];
  form: FormGroup;
  submitted: boolean = false;
  loading: boolean = false;


  constructor(private route: ActivatedRoute, private gymUserService: GymUserService, private formBuilder: FormBuilder) {
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
        this.model.type = 'Pola Meseca'
      }else if(this.model.type == 1){
        this.model.type = 'Mesec dana'
      }else if(this.model.type == 2){
        this.model.type = 'Tri meseca'
      }else if(this.model.type == 3){
        this.model.type = 'Pola Godine'
      }else {
        this.model.type = 'Godinu Dana'
      }

      console.log(this.model);
    })
  }

  Freez(){
      this.gymUserService.freeze(this.id ?? '').subscribe((response:any) =>{
        console.log(response);
        window.location.reload();
      });
  }

  Activate(){
    this.gymUserService.Activate(this.id ?? '').subscribe((response:any) =>{
      console.log(response);
      window.location.reload();
    });;
  }

  Extend(){
    this.gymUserService.Extend(this.id ?? '', 1).subscribe((response:any) =>{
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

    this.gymUserService.Update(this.id?? '', this.model).subscribe((response:any)=>{
      console.log(response);
      window.location.reload();
    });
  }
}
