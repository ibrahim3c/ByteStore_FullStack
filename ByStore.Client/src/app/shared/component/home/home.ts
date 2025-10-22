import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';



interface Product {
  id: number;
  name: string;
  price: number;
  imageUrl: string;
  category: string;
}

interface Category {
  name: string;
  imageUrl: string;
  link: string;
}

@Component({
  selector: 'app-home',
  imports: [CommonModule, // For *ngFor
    RouterLink],
  templateUrl: './home.html',
  styleUrl: './home.css'
})

export class Home {
// Mock data for "Featured Products" section
  featuredProducts: Product[] = [
    {
      id: 1,
      name: 'Sleek Ultrabook Pro',
      price: 1399.99,
      imageUrl: '[Image of a sleek silver ultrabook laptop]',
      category: 'Laptops'
    },
    {
      id: 2,
      name: 'Galaxy-S Ultra Smartphone',
      price: 999.99,
      imageUrl: '[Image of a premium modern smartphone]',
      category: 'Smartphones'
    },
    {
      id: 3,
      name: 'Studio ANC Headphones',
      price: 349.99,
      imageUrl: '[Image of wireless noise-cancelling headphones]',
      category: 'Audio'
    }
  ];

  // Mock data for "Shop by Category" section
  categories: Category[] = [
    {
      name: 'Laptops',
      imageUrl: '[Image of a collection of modern laptops]',
      link: '/products?category=laptops'
    },
    {
      name: 'Smartphones',
      imageUrl: '[Image of the latest smartphones on display]',
      link: '/products?category=smartphones'
    },
    {
      name: 'Audio',
      imageUrl: '[Image of stylish wireless headphones and earbuds]',
      link: '/products?category=audio'
    },
    {
      name: 'Gaming',
      imageUrl: '[Image of high-tech gaming accessories keyboard mouse]',
      link: '/products?category=gaming'
    }
  ];
}
