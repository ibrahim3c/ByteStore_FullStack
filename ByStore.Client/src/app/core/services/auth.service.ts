import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { UserRegister } from '../models/auth/userRegister';
import { UserLogin } from '../models/auth/userLogin';
import { map, tap } from 'rxjs';
import { Router } from '@angular/router';
import {ResetPasswordRequest } from '../models/auth/resetPassword';
import { ForgotPasswordRequest } from '../models/auth/ForgotPassword';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private httpClient = inject(HttpClient);
  private router = inject(Router);
  private readonly apiUrl = `${environment.baseUrl}/accounts`;
  isLoggedIn(): boolean {
    return !!localStorage.getItem('accessToken');
  }

  register(userRegister: UserRegister) {
    return this.httpClient.post(`${this.apiUrl}/register`, userRegister);
  }
  login(userLogin: UserLogin) {
    return this.httpClient.post<{ token: string }>(`${this.apiUrl}/login`, userLogin,{withCredentials:true}).pipe(
    tap((result: any) => {
      const token = result.token;
      if (token) {
        localStorage.setItem('accessToken', token);
      }
    }))
  }

  logout() {
    localStorage.removeItem('accessToken');
    this.revokeToken().subscribe()
    this.router.navigateByUrl('/');
  }
  verifyEmail(userId: string, code: string) {
    return this.httpClient.get(`${this.apiUrl}/verify-email`, {
      params: { userId, code },
      responseType: 'text',
    });
  }
  refreshToken() {
    return this.httpClient.post(`${this.apiUrl}/refresh-token`,{},{withCredentials:true})
  }

  revokeToken() {
    return this.httpClient.post(`${this.apiUrl}/revoke-token`, {}, { withCredentials: true });
  }

  resetPassword(resetPassword: ResetPasswordRequest) {
    return this.httpClient.post(`${this.apiUrl}/reset-password`,resetPassword, {
      responseType: 'text'
    });
  }


  forgotPassword(forgotPassword: ForgotPasswordRequest) {
    return this.httpClient.post(`${this.apiUrl}/forgot-password`, forgotPassword, {
      responseType: 'text'
    });
  }
}
