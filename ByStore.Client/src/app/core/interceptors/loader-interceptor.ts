import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { LoadingService } from '../services/loading.service';
import { finalize } from 'rxjs';

let totalRequests = 0;
export const loaderInterceptor: HttpInterceptorFn = (req, next) => {
  const loadingService=inject(LoadingService);

  console.log('Loader Interceptor Invoked');
  if (totalRequests === 0){
    loadingService.show();
    console.log('Loading started');
  }
  totalRequests++;

  return next(req).pipe(
    finalize(()=>{
      totalRequests--;
      if (totalRequests === 0){
        loadingService.hide();
        console.log('Loading ended');
      }
    })
  );
};
