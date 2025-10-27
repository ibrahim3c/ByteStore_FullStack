import { Routes } from '@angular/router';
import { ProductList } from './features/products/product-list/product-list';
import { ProductDetails } from './features/products/product-details/product-details';
import { NotFound } from './features/errors/not-found/not-found';
import { ServerError } from './features/errors/sever-error/server-error';
import { About } from './shared/component/about/about';
import { Contact } from './shared/component/contact/contact';
import { Home } from './shared/component/home/home';
import { Cart } from './features/cart/cart';

export const routes: Routes = [
  {path:'home',component:Home},
  {path:'products',component:ProductList},
  {path:'cart',component:Cart},
  {path:'product/:id',component:ProductDetails},
  {path:'about',component:About},
  {path:'contact',component:Contact},
  {path:'not-found',component:NotFound},
  {path:'server-error',component:ServerError},
  {path:'',redirectTo:'/home',pathMatch:'full'}, // later we will change it to home
  {path:'**',component:NotFound}
];
