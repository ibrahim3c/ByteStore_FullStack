import { Routes } from '@angular/router';
import { ProductList } from './features/products/product-list/product-list';
import { ProductDetails } from './features/products/product-details/product-details';

export const routes: Routes = [
  // {path:'/home',component:Home},
  {path:'products',component:ProductList},
  {path:'product/:id',component:ProductDetails},
  // {path:'about',component:About},
  // {path:'contact',component:Contact},
  // {path:'**',component:NotFound},
  {path:'',redirectTo:'/products',pathMatch:'full'} // later we will change it to home
];
