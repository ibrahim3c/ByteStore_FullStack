import {
  APP_INITIALIZER,
  ApplicationConfig,
  importProvidersFrom,
  provideBrowserGlobalErrorListeners,
  provideZonelessChangeDetection,
} from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideToastr } from 'ngx-toastr';
import { authInterceptor } from './core/interceptors/auth-interceptor-interceptor';
import { AuthService } from './core/services/auth.service';

export function appInit(authService: AuthService) {
  return () => authService.initUser();
}
export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(withInterceptors([
      // errorInterceptor,
      //  loaderInterceptor,
       authInterceptor])),
    provideAnimations(),
    provideToastr({
      positionClass: 'toast-bottom-right',
      timeOut: 3000,
      preventDuplicates: true,
    }),
     {
      provide: APP_INITIALIZER,
      useFactory: appInit,
      deps: [AuthService],
      multi: true
    }
  ],
};
