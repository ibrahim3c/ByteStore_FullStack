import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
    const authService = inject(AuthService);
    let isRefreshing = false;
    const token = localStorage.getItem('accessToken');

    let authReq = req;
    if (token) {
      authReq = req.clone({
        setHeaders: { Authorization: `Bearer ${token}` },
        withCredentials: true
      });
    }
  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
        if (error.status === 401 && !isRefreshing) {
          isRefreshing = true;
          return authService.refreshToken().pipe(
            switchMap((res: any) => {
              isRefreshing = false;
              localStorage.setItem('accessToken', res.accessToken);

              const cloned = req.clone({
                setHeaders: { Authorization: `Bearer ${res.accessToken}` },
                withCredentials: true
              });
              return next(cloned);
            }),
            catchError(err => {
              isRefreshing = false;
              authService.logout();
              return throwError(() => err);
            })
          );
        }
        return throwError(() => error);
      })
    );
  };
