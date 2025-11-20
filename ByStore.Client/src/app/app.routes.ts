import { Routes } from '@angular/router';
import { ProductList } from './features/products/product-list/product-list';
import { ProductDetails } from './features/products/product-details/product-details';
import { NotFound } from './features/errors/not-found/not-found';
import { ServerError } from './features/errors/sever-error/server-error';
import { About } from './shared/component/about/about';
import { Contact } from './shared/component/contact/contact';
import { Home } from './shared/component/home/home';
import { Cart } from './features/cart/cart';
import { Login } from './features/auth/login/login';
import { Register } from './features/auth/register/register';
import { VerifyEmail } from './features/auth/verify-email/verify-email';
import { ForgotPassword } from './features/auth/forgot-password/forgot-password';
import { ResetPassword } from './features/auth/reset-password/reset-password';
import { loginGuard } from './core/guards/login-guard';
import { authGuard } from './core/guards/auth-guard';
import { Profile } from './features/auth/profile/profile';
import { Checkout } from './features/checkout/checkout';
import { CheckoutStepper } from './features/checkout-stepper/checkout-stepper';

export const routes: Routes = [
  {path:'home',component:Home},
  {path:'products',component:ProductList},
  {path:'cart',component:Cart,canActivate:[authGuard]},
  {path:'profile',component:Profile,canActivate:[authGuard]},
  // {path:'checkout',component:Checkout
  //   // ,canActivate:[authGuard]
  // },
  {path:'stepper',component:CheckoutStepper},
  {path:'product/:id',component:ProductDetails},
  {path:'about',component:About},
  {path:'contact',component:Contact},

  {path:'login',component:Login,canActivate:[loginGuard]},
  {path:'register',component:Register,canActivate:[loginGuard]},
  { path: 'verify-email', component: VerifyEmail},
  { path: 'forgot-password', component: ForgotPassword,canActivate:[loginGuard]},
  { path: 'reset-password', component: ResetPassword,canActivate:[loginGuard]},

  {path:'not-found',component:NotFound},
  {path:'server-error',component:ServerError},
  {path:'',redirectTo:'/home',pathMatch:'full'}, // later we will change it to home
  {path:'**',component:NotFound}
];
