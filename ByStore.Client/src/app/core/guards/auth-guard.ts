import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { map } from 'rxjs';

export const authGuard: CanActivateFn = (route, state) => {
  const authService=inject(AuthService)
  const router=inject(Router)
  return authService.$user.pipe(
    map(logged=>{
      console.log("is logged",logged)
        console.log("the user:",logged)
      if(logged)
        return true
       return router.createUrlTree(['/login'])
    }
  )
  );
};
