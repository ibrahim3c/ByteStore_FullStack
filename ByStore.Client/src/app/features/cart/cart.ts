import { Component, OnInit } from '@angular/core';
import { CartService } from '../../core/services/cart.service';
import { ShoppingCart } from '../../core/models/cart/shoppingCart';
import { CartItem } from '../../core/models/cart/CartItem';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-cart',
  imports: [CommonModule,RouterLink],
  templateUrl: './cart.html',
  styleUrl: './cart.css'
})
export class Cart implements OnInit {
  cart$!: Observable<ShoppingCart | null>;
   constructor(private cartService: CartService) {}
  ngOnInit() {
  this.cart$ = this.cartService.cart$;
}

  removeItem(productId: number): void {
    this.cartService.removeItem(productId);
  }

  private updateQuantity(item: CartItem, event: any): void {
    const quantity = Number(event.target.value);
    if (quantity > 0) {
      this.cartService.updateItemQuantity(item.productId, quantity);
    }
  }
  decreaseQuantity(item: CartItem): void {
    if (item.quantity === 1) {
      this.cartService.removeItem(item.productId);
    } else {
      this.cartService.updateItemQuantity(item.productId, item.quantity - 1);
    }
  }
  increaseQuantity(item: CartItem): void {
    this.cartService.updateItemQuantity(item.productId, item.quantity + 1);
  }

  clearCart(): void {
    this.cartService.clearCart();
  }

  getTotalPrice(): number {
    return this.cartService.getTotalPrice();
  }

}
