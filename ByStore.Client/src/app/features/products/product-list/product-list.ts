import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
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

@Component({
  selector: 'app-product-list',
  imports: [ProductCard, CommonModule, FormsModule, NgbPagination],
  templateUrl: './product-list.html',
  styleUrl: './product-list.css',
})
export class ProductList implements OnInit {
  brands: string[] = [];
  searchTerm: any;
  pageSize!: number;
  currentPage!: number;

  constructor(
    private productService: ProductService,
    private brandService: BrandService,
    private categoryService: CategoryService
  ) {}
  products: Product[] = [];
  filteredProducts: Product[] = [];
  metaData!: MetaData;
  categories: Category[] = [];
  ngOnInit() {
    this.productService.getProducts().subscribe((response: HttpResponse<any>) => {
      this.products = response.body ?? [];
      this.metaData = JSON.parse(response.headers.get('X-Pagination')!);
      this.filteredProducts = this.products;

      this.pageSize = this.metaData.PageSize;
      this.currentPage = this.metaData.CurrentPage;
    });

    this.brandService.getAllBrands().subscribe((brands) => {
      this.brands = brands.map((b) => b.name);
    });

    this.categoryService.getAllCategories().subscribe((categories) => {
      this.categories = categories;
    });
  }

  clearFilters() {
    throw new Error('Method not implemented.');
  }
  applyFilters() {
    throw new Error('Method not implemented.');
  }
  onPageChange($event: number) {
    throw new Error('Method not implemented.');
  }
  onSortChanged($event: Event) {
    throw new Error('Method not implemented.');
  }
}
