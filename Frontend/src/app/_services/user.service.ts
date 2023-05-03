import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseUrl = 'https://localhost:7085/api/User';

constructor(private http: HttpClient) { }

getUserData(id:number){
  return this.http.get(this.baseUrl + '/' + id);
}

update(id: string, model:any){
  return this.http.put(this.baseUrl + '/' +id, model);
}
}

