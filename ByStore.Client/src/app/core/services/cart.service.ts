import { inject, Injectable, OnInit } from "@angular/core";
import { BehaviorSubject, catchError, map, Observable, tap, throwError } from "rxjs";
import {ShoppingCart } from "../models/cart/shoppingCart";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../environments/environment";
import { CartItem } from "../models/cart/CartItem";
import { Product } from "../models/Product";
import { v4 as uuidv4 } from 'uuid';


@Injectable({
  providedIn: 'root'
})
export class CartService{
  constructor() {
    this.loadCart()
    console.log(this.cart.getValue());
  }
  private cart = new BehaviorSubject<ShoppingCart | null>(null);
  cart$ = this.cart.asObservable();

  private http=inject(HttpClient)
  private readonly apiUrl=`${environment.baseUrl}/ShoppingCarts`;

  setCart(cart: ShoppingCart | null){
    this.cart.next(cart);
  }
  getCurrentCartValue(): ShoppingCart | null {
    return this.cart.getValue();
  }

  // getCart(): Observable<ShoppingCart> {
  //   const id = this.getOrCreateCartId();
  //   return this.http.get<ShoppingCart>(`${this.apiUrl}/${id}`).pipe(
  //     map((cart) => {
  //       this.setCart(cart);
  //       return cart;
  //     }),
  //     catchError(this.handleError)
  //   );
  // }

  loadCart(): void {
  const id = this.getOrCreateCartId();
  this.http.get<ShoppingCart>(`${this.apiUrl}/${id}`).pipe(
    catchError(this.handleError)
  ).subscribe(cart=> this.setCart(cart));
  }

  saveCart(cart: ShoppingCart): void {
  this.http.post<ShoppingCart>(this.apiUrl, cart).pipe(
    catchError(this.handleError)
  ).subscribe(cart => this.setCart(cart));
  }

  addItemToCart(product: Product, quantity = 1): void
  {
    // convert Product to CartItem
    const item: CartItem={
      productId:product.id,
      name: product.name,
      price: product.price,
      imageUrl: product.thumbnailUrl,
      quantity:quantity,
      brandName: product.brandName,
      categoryName: product.categoryName
    }
    let cart = this.getCurrentCartValue();
    console.log('Current cart before adding item:', cart);
    // if no cart → create new one
    if (!cart) {
      cart = {
        id: this.getOrCreateCartId(),
        cartItems: [],
      };
    }
    // check if item already exists
    const index = cart.cartItems.findIndex(i => i.productId === item.productId);
    console.log('Item index in cart:', index);
    console.log('productId to add:', item.productId);

    if (index === -1) {
      cart.cartItems.push(item);
    } else {
      cart.cartItems[index].quantity += quantity;
    }
    // update observable
    this.cart.next(cart);
    // save to backend
    this.saveCart(cart);
    console.log('Cart after adding item:', cart);
  }

clearCart(): void
  {
  const id = this.getOrCreateCartId();
  this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
    tap(() => {
      localStorage.removeItem('cart_id');
      this.setCart(null);
    }),
    catchError(this.handleError)
  ).subscribe();
}
removeItem(productId: number): void
  {
  const cart = this.getCurrentCartValue();
  if (!cart) return;
  cart.cartItems = cart.cartItems.filter(i => i.productId !== productId);
  this.setCart(cart);
  this.saveCart(cart);
}

updateItemQuantity(productId: number, quantity: number): void {
  const cart = this.getCurrentCartValue();
  if (!cart) return;

  const item = cart.cartItems.find(i => i.productId === productId);
  if (item) {
    item.quantity = quantity;
    this.setCart(cart);
    this.saveCart(cart);
  }
}

getTotalPrice(): number {
  const cart = this.getCurrentCartValue();
  if (!cart) return 0;
  return cart.cartItems.reduce((sum, item) => sum + item.price * item.quantity, 0);
}



  private getOrCreateCartId(): string {
    let cartId = localStorage.getItem('cart_id');
    if (!cartId) {
      cartId = uuidv4();
      localStorage.setItem('cart_id', cartId);
    }
    return cartId;
  }
  // Error handling
  private handleError(error: any): Observable<never> {
    let errorMessage = 'An unknown error occurred!';

    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side error
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }

    console.error(errorMessage);
    return throwError(() => new Error(errorMessage));
  }

}
