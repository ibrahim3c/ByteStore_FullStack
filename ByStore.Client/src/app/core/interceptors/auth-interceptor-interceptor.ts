import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { inject, Injector } from '@angular/core'; // 1. استيراد Injector

// متغير خارجي للتحكم في عملية الريفريش عبر كل الطلبات
let isRefreshing = false;

export const authInterceptor: HttpInterceptorFn = (req, next) => {
    // const authService = inject(AuthService); // <-- 2. احذف هذا السطر المسبب للمشكلة
    const injector = inject(Injector); // 3. استخدم Injector بدلاً منه
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
        // نتأكد أن الخطأ 401 وأننا لا نحاول عمل ريفريش حالياً، وأيضاً نتجنب الدخول في لوب إذا كان طلب الريفريش نفسه هو اللي فشل
        if (error.status === 401 && !isRefreshing && !req.url.includes('refresh-token')) {
          isRefreshing = true;

          // 4. استدعاء AuthService هنا فقط عند الحاجة باستخدام Injector
          const authService = injector.get(AuthService);

          return authService.refreshToken().pipe(
            switchMap((res: any) => {
              isRefreshing = false;
              localStorage.setItem('accessToken', res.token); // تأكد أن الباك إند بيرجع "token" أو "accessToken" وعدلها هنا

              const cloned = req.clone({
                setHeaders: { Authorization: `Bearer ${res.token}` },
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
