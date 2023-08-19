import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AccountService } from './account.service';
import { BASE_URL } from '../config/api-url.config';

@Injectable({
  providedIn: 'root'
})
export class GymUserService {

  baseUrl = `${BASE_URL}/GymUser`;

constructor(private http: HttpClient, private auth: AccountService) { }
  
addGymUser(model: any)  {
    return this.http.post(this.baseUrl, model);
  }

  getAllUsers(PageSize: number, Page: number, SearchString: string, sortDirect: number, sortParam: string)  {
    const queryParams = `?PageSize=${PageSize}&Page=${Page}&SearchString=${SearchString}&SortOrder=${sortDirect}&?SortParam=${sortParam}`;
    return this.http.get(this.baseUrl + queryParams);
  }

  getUser(id:string)  {
    return this.http.get(this.baseUrl + '/' + id);
  }

  freeze(id:string){
    return this.http.put(this.baseUrl + '/freez/' + id, undefined);
  }

  Activate(id: string){
    return this.http.put(this.baseUrl + '/activate/' + id, undefined);
  }

  Extend(id: string, numberOfMonths: any){
    return this.http.put(this.baseUrl + '/extend/' + id, numberOfMonths);
  }

  Update(id: string, model:any){
    return this.http.put(this.baseUrl + '/' + id, model);
  }

  freezAll(){
    return this.http.put(this.baseUrl + '/' + 'freez-all', undefined);
  }

  activateAll(){
    return this.http.put(this.baseUrl + '/' + 'activate-all', undefined);
  }
}
