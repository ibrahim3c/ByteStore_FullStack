import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { SpinnerService } from '../services/loading.service';
import { finalize } from 'rxjs';

let totalRequests = 0;
export const loaderInterceptor: HttpInterceptorFn = (req, next) => {
  const spinner=inject(SpinnerService);

    spinner.show();
    return next(req).pipe(
      finalize(() =>spinner.hide())
    );

};
