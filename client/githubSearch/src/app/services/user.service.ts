import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, catchError, map, tap } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {


  data: any = localStorage.getItem('access_token');

  constructor(private http: HttpClient) {
    // Call the login method during service initialization
  }

  login() {
    // Simulate a login process and obtain a JWT token from the server
    this.http.get<any>(`${environment.apiEndpoint}/token`).subscribe((e) => {
      this.data = e;
      localStorage.setItem("access_token",e);
    });
  }

  isAuthorized() {
    // Just check if token exists
    // It not, user has never logged in current session
    return false;//Boolean(this.data);
  }
}
