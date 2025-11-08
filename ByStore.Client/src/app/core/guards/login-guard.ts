import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { map } from 'rxjs';

export const loginGuard: CanActivateFn = (route, state) =>
  {
    const authService = inject(AuthService);
  const router = inject(Router);

  return authService.$user.pipe(
    map(isLoggedIn => {
      if (isLoggedIn) {
        return router.createUrlTree(['/home']);
      }
      return true;
    })
  );}
