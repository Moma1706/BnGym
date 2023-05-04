import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class GymUserService {

  baseUrl = 'https://localhost:7085/api/GymUser';

constructor(private http: HttpClient, private auth: AccountService) { }
  addGymUser(model: any)  {
    return this.http.post(this.baseUrl, model);
  }

  getAllUsers(PageSize: number, Page: number, SearchString: string, sortDirect: number)  {
    const queryParams = `?PageSize=${PageSize}&Page=${Page}&SearchString=${SearchString}&SortOrder=${sortDirect}`;
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
}
