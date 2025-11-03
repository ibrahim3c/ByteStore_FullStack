import { inject, Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpParams } from "@angular/common/http";
import { UserRegister } from "../models/auth/userRegister";
import { UserLogin } from "../models/auth/userLogin";
import { map, tap } from "rxjs";
import { Router } from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class AuthService{
  private httpClient = inject(HttpClient)
  private router=inject(Router)
  private readonly apiUrl = `${environment.baseUrl}/account`;
  isLoggedIn(): boolean {
    return !!localStorage.getItem("token");
  }

  register(userRegister: UserRegister) {
    return this.httpClient.post(`${this.apiUrl}/register`, userRegister);
  }
  login(userLogin: UserLogin) {
        tap((result: any) => {
      const token = result.token;
      if (token) {
        localStorage.setItem("token", token);
      }
    });
  }

  logout() {
    localStorage.removeItem("token");
    this.router.navigateByUrl('/')
  }
  verifyEmail(userId:string, code:string){
    return this.httpClient.post(`${this.apiUrl}/verify-email`,{
      HttpParams: new HttpParams()
        .set('userId', userId)
        .set('code', code)
    });
  }

}
