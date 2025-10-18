import { Routes } from '@angular/router';
import { ProductList } from './features/products/product-list/product-list';
import { ProductDetails } from './features/products/product-details/product-details';
import { NotFound } from './features/errors/not-found/not-found';
import { ServerError } from './features/errors/sever-error/server-error';

export const routes: Routes = [
  // {path:'/home',component:Home},
  {path:'products',component:ProductList},
  {path:'product/:id',component:ProductDetails},
  // {path:'about',component:About},
  // {path:'contact',component:Contact},
  {path:'not-found',component:NotFound},
  {path:'server-error',component:ServerError},
  {path:'',redirectTo:'/products',pathMatch:'full'}, // later we will change it to home
  {path:'**',component:NotFound}
];
