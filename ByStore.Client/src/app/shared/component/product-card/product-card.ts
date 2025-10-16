import { Component, Input } from '@angular/core';
import { Product } from '../../../core/models/Product';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-product-card',
  imports: [CommonModule],
  templateUrl: './product-card.html',
  styleUrl: './product-card.css'
})
export class ProductCard {

  // Online placeholder if no image
  placeholderImage = 'https://via.placeholder.com/400x300?text=No+Image';

  constructor(private router: Router) {}

  viewDetails(id: number) {
    this.router.navigate(['/product', id]);
  }

  addToCart(product: Product) {
    console.log('Added to cart:', product);
    // TODO: Connect with Cart Service
  }

  onImageError(event: Event) {
    const target = event.target as HTMLImageElement;
    target.src = this.placeholderImage;
  }
@Input() product!: Product;
}
