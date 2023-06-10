import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BASE_URL } from '../config/api-url.config';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseUrl = `${BASE_URL}/User`;

constructor(private http: HttpClient) { }

getUserData(id:number){
  return this.http.get(this.baseUrl + '/' + id);
}

update(id: string, model:any){
  return this.http.put(this.baseUrl + '/' +id, model);
}
}

