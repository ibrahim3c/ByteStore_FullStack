import { inject, Injectable } from "@angular/core";
import { BehaviorSubject, catchError, map, Observable, throwError } from "rxjs";
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

  getCart(): Observable<ShoppingCart> {
    const id = this.getOrCreateCartId();
    return this.http.get<ShoppingCart>(`${this.apiUrl}/${id}`).pipe(
      map((cart) => {
        this.setCart(cart);
        return cart;
      }),
      catchError(this.handleError)
    );
  }

  saveCart(cart: ShoppingCart): Observable<void> {
    return this.http.post<ShoppingCart>(this.apiUrl, cart).pipe(
      map((savedCart) => {
        this.setCart(savedCart);
      }),
      catchError(this.handleError)
    );
  }

  addItemToCart(product: Product, quantity = 1): void {
    // convert Product to CartItem
    const item: CartItem={
      productId:product.id,
      name: product.name,
      price: product.price,
      imageUrl: product.thumbnailUrl,
      quantity:0, // will be set later,
      brandName: product.brandName,
      categoryName: product.categoryName
    }


    let cart = this.getCurrentCartValue();

    // if no cart → create new one
    if (!cart) {
      cart = {
        id: this.getOrCreateCartId(),
        cartItems: [],
      };
    }

    // check if item already exists
    const index = cart.cartItems.findIndex(i => i.productId === item.productId);

    if (index === -1) {
      // new item
      item.quantity = quantity;
      cart.cartItems.push(item);
    } else {
      // increase quantity
      cart.cartItems[index].quantity += quantity;
    }

    // update observable
    this.cart.next(cart);

    // save to backend
    this.saveCart(cart).subscribe();
  }

  clearCart(): Observable<void> {
     const id = this.getOrCreateCartId();
    return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
      map(() => this.setCart(null)),
      catchError(this.handleError)
    );
  }

  removeItem(productId: number): void {
  const cart = this.getCurrentCartValue();
  if (!cart) return;

  cart.cartItems = cart.cartItems.filter(i => i.productId !== productId);

  this.setCart(cart);
  this.saveCart(cart).subscribe();
}

updateItemQuantity(productId: number, quantity: number): void {
  const cart = this.getCurrentCartValue();
  if (!cart) return;

  const item = cart.cartItems.find(i => i.productId === productId);
  if (item) {
    item.quantity = quantity;
    this.setCart(cart);
    this.saveCart(cart).subscribe();
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
