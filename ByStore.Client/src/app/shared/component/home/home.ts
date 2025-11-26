import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CategoryService } from '../../../core/services/category.service';
import { Category } from '../../../core/models/Category';
import { ProductCard } from '../product-card/product-card';
import { Product } from '../../../core/models/Product';
import { ProductService } from '../../../core/services/product.service';
import { HttpResponse } from '@angular/common/http';



@Component({
  selector: 'app-home',
  imports: [CommonModule, // For *ngFor
    RouterLink,
  ProductCard],
  templateUrl: './home.html',
  styleUrl: './home.css'
})

export class Home implements OnInit {
  ngOnInit(): void {
    this.categoryService.getAllCategories().subscribe(cats=>{
      this.categories=cats;
    })

    this.productService.getProducts({
      PageSize:3,
      PageNumber:1
    }).subscribe((response: HttpResponse<any>) => {
      this.featuredProducts = response.body ?? [];
    })
  }

  private readonly categoryService=inject(CategoryService)
  private readonly productService=inject(ProductService)
  featuredProducts: Product[]=[];
  categories: Category[]=[]


}
