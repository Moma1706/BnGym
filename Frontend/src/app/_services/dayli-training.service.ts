import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DayliTrainingService {

  baseUrl = 'https://localhost:7085/api/DailyUser';

constructor(private http: HttpClient) { }

addTraining(model: any){
  return this.http.post(this.baseUrl, model);
}

getAllDayliTrainings(PageSize: number, Page: number, SearchString: string, sortDirect: number)
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
