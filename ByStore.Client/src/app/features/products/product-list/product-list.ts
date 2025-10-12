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
  imports: [ProductCard, CommonModule, FormsModule, NgbPagination],
  templateUrl:'./product-list.html',
  styleUrl: './product-list.css',
})
export class ProductList implements OnInit {

  products: Product[] = [];
  filteredProducts: Product[] = [];

  // brands: string[] = [];
  brands: Brand[] = [];
  categories: Category[] = [];
  // selectedCategoryId?:number;
  // selectedBrandId?:number;
  // selectedMinPrice?:number;
  // selectedMaxPrice?:number;
  searchTerm?: string;

  pageSize!: number;
  currentPage!: number;
  metaData!: MetaData;

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
    console.log(this.filteredProducts)
    console.log(this.categories)
    console.log(this.brands)
  }


  loadProducts():void{
      this.productService.getProducts(this.productParams).subscribe((response: HttpResponse<any>) => {
      this.products = response.body ?? [];
      this.metaData = JSON.parse(response.headers.get('X-Pagination')!);
      this.filteredProducts = this.products;
      this.pageSize = this.metaData.PageSize;
      this.currentPage = this.metaData.CurrentPage;
    });
  }
  loadCategories(): void {
    this.categoryService.getAllCategories().subscribe((categories) => {
    this.categories = categories;
    });
  }
  loadBrands(): void {
    this.brandService.getAllBrands().subscribe((brands) => {
    // this.brands = brands.map((b) => b.name);
    this.brands = brands;
    });
  }

  applyFilters() {
    this.loadProducts();
    }

  resetFilters() {
    this.productParams= new ProductParameters();
    this.loadProducts();
  }

  onPageChange($event: number) {
    throw new Error('Method not implemented.');
  }
  onSortChanged($event: Event) {
    throw new Error('Method not implemented.');
  }

searchProduct() {
throw new Error('Method not implemented.');
}
}
