import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class GymUserService {

  baseUrl = 'https://localhost:7085/api/GymUser';

constructor(private http: HttpClient) { }

  addGymUser(model: any)
  {
    return this.http.post(this.baseUrl, model);
  }

}
