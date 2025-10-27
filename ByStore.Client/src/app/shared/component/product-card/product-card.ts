import { Component, Input } from '@angular/core';
import { Product } from '../../../core/models/Product';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CartService } from '../../../core/services/cart.service';

@Component({
  selector: 'app-product-card',
  imports: [CommonModule],
  templateUrl: './product-card.html',
  styleUrl: './product-card.css'
})
export class ProductCard {

  // Online placeholder if no image
  placeholderImage = 'https://placehold.co/400x400?text=No+Image&font=roboto';

  constructor(private router: Router, private cartService: CartService) {}

  viewDetails(id: number) {
    this.router.navigate(['/product', id]);
  }

  addToCart(product: Product) {
    this.cartService.addItemToCart(product, 1);
  }

  onImageError(event: Event) {
    const target = event.target as HTMLImageElement;
    target.src = this.placeholderImage;
  }
@Input() product!: Product;
}
