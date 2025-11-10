import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { UserRegister } from '../models/auth/userRegister';
import { UserLogin } from '../models/auth/userLogin';
import { BehaviorSubject, catchError, map, Observable, of, tap } from 'rxjs';
import { Router } from '@angular/router';
import {ResetPasswordRequest } from '../models/auth/resetPassword';
import { ForgotPasswordRequest } from '../models/auth/ForgotPassword';
import { User } from '../models/auth/User';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private httpClient = inject(HttpClient);
  private router = inject(Router);
  private readonly apiUrl = `${environment.baseUrl}/accounts`;

  private currentUserSource=new BehaviorSubject<User | null>(null);
  $user=this.currentUserSource.asObservable()
  //  isLoggedIn$ = this.$user.pipe(
  //   map(user => !!user)
  // );


 constructor() {
    this.getCurrentUser().subscribe({
    });
  }


  getCurrentUser():Observable<User | null>{

    const token = localStorage.getItem('accessToken');
    if(!token)
    {
      this.currentUserSource.next(null)
      return of(null)
    }


    return this.httpClient.get<User>(`${this.apiUrl}/me`).pipe(
      tap(user=>{
        this.currentUserSource.next(user)}
      ),
    catchError((err) => {
      // localStorage.removeItem('accessToken')
      this.currentUserSource.next(null);
      return of(null);
    })
    )
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
        this.getCurrentUser().subscribe();
      }
    }))
  }

  logout() {
    this.revokeToken().subscribe({
    next: () => {
      localStorage.removeItem('accessToken');
      this.currentUserSource.next(null);
      this.router.navigateByUrl('/');
    },
    error: () => {
      localStorage.removeItem('accessToken');
      this.currentUserSource.next(null);
      this.router.navigateByUrl('/');
    }
  });
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
