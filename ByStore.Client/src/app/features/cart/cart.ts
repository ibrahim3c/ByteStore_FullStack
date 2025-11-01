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
    this.cartService.getCart().subscribe();
    console.log("onInit",this.cart$)
}

  removeItem(productId: number): void {
    this.cartService.removeItemFromCart(productId);
  }

  decreaseQuantity(item: CartItem): void {
    if (item.quantity === 1) {
      this.cartService.removeItemFromCart(item.productId);
    } else {
      this.cartService.updateItemQuantity(item.productId, item.quantity - 1);
    }
    console.log("Current cart in decreaseQuantity:", this.cart$);
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
