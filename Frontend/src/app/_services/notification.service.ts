import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BASE_URL } from '../config/api-url.config';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  baseUrl = `${BASE_URL}/Notification`;

constructor(private http: HttpClient) { }

getAll(){
  return this.http.get(this.baseUrl);
}

deleteAll(){
    return this.http.delete(this.baseUrl);
  }

deleteOne(id: string){
    return this.http.delete(this.baseUrl + `/${id}`);
  }
}
