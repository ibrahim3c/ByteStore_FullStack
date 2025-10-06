import { Component, Inject, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {MatToolbarModule} from '@angular/material/toolbar';
import { NavBar } from './shared/component/nav-bar/nav-bar';
import { HttpClient } from '@angular/common/http';
import { ProductService } from './core/services/product.service';
import { Product } from './core/models/Product';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet,MatToolbarModule,NavBar],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('ByStore.Client');
  products:Product[]=[];
  productService=Inject(ProductService)
  ngOnInit(){
   const subscription= this.productService.subscribe((data:Product[])=>{
      this.products=data
    })
  }
}
