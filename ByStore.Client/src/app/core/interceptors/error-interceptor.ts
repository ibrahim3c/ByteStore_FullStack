// error.interceptor.ts
import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const toastr = inject(ToastrService);

  return next(req).pipe(
    catchError((error) => {
      if (!error) return throwError(() => error);

      switch (error.status) {
        case 400:
          if (error.error?.errors) {
            // Flatten multiple validation messages
            const messages = Object.values(error.error.errors)
              .flat()
              .join('<br>');
            toastr.error(messages, 'Validation Error', { enableHtml: true });
          } else {
            toastr.error(error.error?.detail || 'Bad Request', 'Error');
          }
          break;

        case 404:
          router.navigate(['/not-found']);
          break;

        case 500:
          router.navigate(['/server-error']);
          break;

        default:
          toastr.error('Something went wrong', 'Error');
          break;
      }

      return throwError(() => error);
    })
  );
};
