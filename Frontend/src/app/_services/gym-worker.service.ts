import { GymWorkerModel } from './../_models/gym-worker';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BASE_URL } from '../config/api-url.config';

@Injectable({
  providedIn: 'root'
})
export class GymWorkerService {

  baseUrl = `${BASE_URL}/GymWorker`;

constructor(private http: HttpClient) { }

  addGymWorker(model: any){

    let gymWorkerModel = new GymWorkerModel();

    gymWorkerModel.email = model.email;
    gymWorkerModel.firstname = model.firstName;
    gymWorkerModel.lastname = model.lastName;

    console.log('Method addGymWorker from gymWorkerService worked!');

    return this.http.post(this.baseUrl, gymWorkerModel);
  }

  getAllWorkers(PageSize: number, Page: number, SearchString: string, sortDirect: number)  {
    const queryParams = `?PageSize=${PageSize}&Page=${Page}&SearchString=${SearchString}&SortOrder=${sortDirect}`;
    return this.http.get(this.baseUrl + queryParams);
  }

  getWorker(id:string){
    return this.http.get(this.baseUrl + '/' + id);
  }

  update(id: string, model:any){
    return this.http.put(this.baseUrl + '/' +id, model);
  }

  block(id:string){
    return this.http.delete(this.baseUrl + '/' +id);
  }

  activate(id:string){
    return this.http.put(this.baseUrl + '/activate/' +id, undefined);
  }

}
