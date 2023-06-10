import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BASE_URL } from '../config/api-url.config';

@Injectable({
  providedIn: 'root'
})
export class DailyTrainingService {

  baseUrl = `${BASE_URL}/DailyUser`;

constructor(private http: HttpClient) { }

addTraining(model: any){
  return this.http.post(this.baseUrl, model);
}

getAllDailyTrainings(PageSize: number, Page: number, SearchString: string, sortDirect: number)
{
  const queryParams = `?PageSize=${PageSize}&Page=${Page}&SearchString=${SearchString}&SortOrder=${sortDirect}`;
  return this.http.get(this.baseUrl + '/users/' + queryParams);
}

  getTraining(id:string){
    return this.http.get(this.baseUrl + '/' + id);
  }

  update(id:string, model: any){
    return this.http.put(this.baseUrl + '/' +id, model);
  }

  addArrival(id: string){
    return this.http.post(this.baseUrl + '/arrival/' + id, undefined);
  }

}
