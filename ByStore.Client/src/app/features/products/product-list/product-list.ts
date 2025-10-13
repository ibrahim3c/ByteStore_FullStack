import { Component, OnInit } from '@angular/core';
import { Product } from '../../../core/models/Product';
import { ProductService } from '../../../core/services/product.service';
import { ProductCard } from '../../../shared/component/product-card/product-card';
import { CommonModule } from '@angular/common';
import { Category } from '../../../core/models/Category';
import { HttpResponse } from '@angular/common/http';
import { MetaData } from '../../../core/models/MetaData';
import { BrandService } from '../../../core/services/brand.service';
import { CategoryService } from '../../../core/services/category.service';
import { FormsModule } from '@angular/forms';
import { NgbPagination } from '@ng-bootstrap/ng-bootstrap';
import { ProductParameters } from '../../../core/models/ProductParameters';
import { Brand } from '../../../core/models/Brand';

@Component({
  selector: 'app-product-list',
  imports: [ProductCard, CommonModule, FormsModule,
    // NgbPagination
  ],
  templateUrl:'./product-list.html',
  styleUrl: './product-list.css',
})
export class ProductList implements OnInit {
  products: Product[] = [];
  filteredProducts: Product[] = [];

  brands: Brand[] = [];
  categories: Category[] = [];

  metaData?:MetaData;

  productParams:ProductParameters=new ProductParameters();

  constructor(
    private productService: ProductService,
    private brandService: BrandService,
    private categoryService: CategoryService
  ) {}

  ngOnInit() {
    this.loadProducts();
    this.loadCategories();
    this.loadBrands();
  }


  loadProducts():void{
      this.productService.getProducts(this.productParams).subscribe((response: HttpResponse<any>) => {
      this.products = response.body ?? [];
      this.metaData = JSON.parse(response.headers.get('X-Pagination')!);
      console.log(  this.metaData);
      this.filteredProducts = this.products;

      this.productParams.PageSize = this.metaData?.PageSize ?? 10;
      this.productParams.PageNumber = this.metaData?.CurrentPage ?? 1;
    });
  }
  loadCategories(): void {
    this.categoryService.getAllCategories().subscribe((categories) => {
    this.categories = categories;
    });
  }
  loadBrands(): void {
    this.brandService.getAllBrands().subscribe((brands) => {
    this.brands = brands;
    });
  }

  applyFilters() {
    this.productParams.PageNumber=1;
    this.loadProducts();
    }

  resetFilters() {
    this.productParams= new ProductParameters();
    this.loadProducts();
  }
  onSortChanged() {
    this.productParams.PageNumber = 1;
    this.loadProducts();
  }

searchProduct() {
  this.productParams.PageNumber = 1;
  this.loadProducts();
}

onPageChange(PageNumber: number) {
  console.log(PageNumber)
  this.productParams.PageNumber = PageNumber;
  this.loadProducts();
  // console.log(this.metaData)
}
}
