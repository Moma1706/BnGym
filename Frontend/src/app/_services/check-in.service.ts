import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BASE_URL } from '../config/api-url.config';

@Injectable({
  providedIn: 'root'
})
export class CheckInService {

  baseUrl = `${BASE_URL}/CheckIn`;

constructor(private http: HttpClient) { }

checkIn(id:string){
  return this.http.post(this.baseUrl, { "GymUserId": id});
}

getCheckInsByDate(Date: string, PageSize: number, Page: number, SearchString: string, sortDirect: number)  {
  const queryParams = `?PageSize=${PageSize}&Page=${Page}&DateTime=${Date}&SearchString=${SearchString}&SortOrder=${sortDirect}`;
  return this.http.get(this.baseUrl + queryParams);
}

}
