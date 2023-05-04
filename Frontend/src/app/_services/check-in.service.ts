import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CheckInService {

  baseUrl = 'https://localhost:7085/api/CheckIn';

constructor(private http: HttpClient) { }

checkIn(id:string){
  return this.http.post(this.baseUrl, { "GymUserId": id});
}

getCheckInsByDate(Date: string, PageSize: number, Page: number, SearchString: string, sortDirect: number)  {
  const queryParams = `?PageSize=${PageSize}&Page=${Page}&DateTime=${Date}&SearchString=${SearchString}&SortOrder=${sortDirect}`;
  return this.http.get(this.baseUrl + queryParams);
}

}
