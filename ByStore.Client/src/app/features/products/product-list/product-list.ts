import { Component, Inject, OnInit } from '@angular/core';
import { Product } from '../../../core/models/Product';
import { ProductService } from '../../../core/services/product.service';
import { ProductCard } from '../../../shared/component/product-card/product-card';
import { CommonModule } from '@angular/common';
import { Category } from '../../../core/models/Category';

@Component({
  selector: 'app-product-list',
  imports: [ProductCard,CommonModule],
  templateUrl: './product-list.html',
  styleUrl: './product-list.css',
})
export class ProductList implements OnInit {
brands= [
  'Apple',
  'Samsung',
  'Dell',
  'HP',
  'Sony',
  'Logitech',
  'Lenovo',
  'Asus',
  'Microsoft',
  'Razer'
];
onBrandSelected(_t11: any,$event: Event) {
throw new Error('Method not implemented.');
}
categories: Category[] = [
  {
    id: 1,
    name: 'Electronics',
    subCategories: [
      { id: 2, name: 'Laptops', parentCategoryId: 1 },
      { id: 3, name: 'Phones', parentCategoryId: 1 },
      { id: 4, name: 'Accessories', parentCategoryId: 1 }
    ]
  },
  {
    id: 5,
    name: 'Home Appliances',
    subCategories: [
      { id: 6, name: 'Refrigerators', parentCategoryId: 5 },
      { id: 7, name: 'Washing Machines', parentCategoryId: 5 }
    ]
  }
];


onCategorySelected(_t19: any,$event: Event) {
throw new Error('Method not implemented.');
}
onSortChanged($event: Event) {
throw new Error('Method not implemented.');
}
  // products: Product[] = [];
  products: Product[] = [
    {
      id: 1,
      name: 'iPhone 15 Pro',
      brandName: 'Apple',
      categoryName: 'Smartphones',
      price: 1199,
      thumbnailUrl: 'assets/images/iphone15pro.jpg'
    },
    {
      id: 2,
      name: 'Samsung Galaxy S24',
      brandName: 'Samsung',
      categoryName: 'Smartphones',
      price: 1099,
      thumbnailUrl: 'assets/images/galaxyS24.jpg'
    },
    {
      id: 3,
      name: 'Dell XPS 13',
      brandName: 'Dell',
      categoryName: 'Laptops',
      price: 1399,
      thumbnailUrl: 'assets/images/dellxps13.jpg'
    },
    {
      id: 4,
      name: 'HP Spectre x360',
      brandName: 'HP',
      categoryName: 'Laptops',
      price: 1299,
      thumbnailUrl: 'assets/images/hpspectre.jpg'
    },
    {
      id: 5,
      name: 'Sony WH-1000XM5',
      brandName: 'Sony',
      categoryName: 'Headphones',
      price: 399,
      thumbnailUrl: 'assets/images/sonyheadphones.jpg'
    },
    {
      id: 6,
      name: 'Apple MacBook Air M3',
      brandName: 'Apple',
      categoryName: 'Laptops',
      price: 1499,
      thumbnailUrl: 'assets/images/macbookairm3.jpg'
    },
    {
      id: 7,
      name: 'Samsung 4K Smart TV',
      brandName: 'Samsung',
      categoryName: 'TVs',
      price: 899,
      thumbnailUrl: 'assets/images/samsungtv.jpg'
    },
    {
      id: 8,
      name: 'Logitech MX Master 3S',
      brandName: 'Logitech',
      categoryName: 'Accessories',
      price: 129,
      thumbnailUrl: 'assets/images/logitechmx3.jpg'
    }
  ];
  productService = Inject(ProductService);
  filteredProducts:Product[]=[]


  ngOnInit() {
    //  const subscription = this.productService.subscribe((data: Product[]) => {
    //   this.products = data;
    // });
    this.filteredProducts=this.products
  }
}
