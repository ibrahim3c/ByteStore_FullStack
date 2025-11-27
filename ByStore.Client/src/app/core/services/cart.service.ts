import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, catchError, throwError, tap, of, switchMap } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { ShoppingCart } from '../models/cart/shoppingCart';
import { CartItem } from '../models/cart/CartItem';
import { Product } from '../models/Product';
import { v4 as uuidv4 } from 'uuid';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private baseUrl = `${environment.baseUrl}/ShoppingCarts`;
  private cartSource = new BehaviorSubject<ShoppingCart | null>(null);
  cart$ = this.cartSource.asObservable();

  constructor(private http: HttpClient) {
    this.getCart().subscribe();
  }

  private getOrCreateCartId(): string {
    let cartId = localStorage.getItem('cart_id');
    if (!cartId) {
      cartId = uuidv4();
      localStorage.setItem('cart_id', cartId);
    }
    return cartId;
  }

  private setCart(cart: ShoppingCart): void {
    this.cartSource.next(cart);
  }

  getCart(): Observable<ShoppingCart | null> {
    const cartId = this.getOrCreateCartId();
    return this.http.get<ShoppingCart>(`${this.baseUrl}/${cartId}`).pipe(
      tap(cart => this.setCart(cart)),
      catchError(err => {
        if (err.status === 404) {
          console.warn('Cart not found, will create a new one.');
          return of(null);
        }
        console.error('Error fetching cart:', err);
        return throwError(() => err);
      })
    );
  }

  saveCart(cart: ShoppingCart): Observable<ShoppingCart> {
    return this.http.post<ShoppingCart>(this.baseUrl, cart).pipe(
      tap((updatedCart) => { this.setCart(updatedCart)}),
      catchError(err => {
        console.log("errorrrr")
        console.error('Error saving cart:', err);
        return throwError(() => err);
      })
    );
  }

  deleteCart(): Observable<void> {
    const cartId = this.getOrCreateCartId();
    return this.http.delete<void>(`${this.baseUrl}/${cartId}`).pipe(
      tap(() => {
        localStorage.removeItem('cart_id');
        this.cartSource.next(null);
      }),
      catchError(err => {
        console.error('Error deleting cart:', err);
        return throwError(() => err);
      })
    );
  }

  addItemToCart(product: Product, quantity: number = 1): void {
    const cartId = this.getOrCreateCartId();

    const newItem: CartItem = {
      productId: product.id,
      name: product.name,
      price: product.price,
      imageUrl: product.thumbnailUrl??"",
      quantity,
      brandName: product.brandName,
      categoryName: product.categoryName
    };

    this.getCart()
      .pipe(
        switchMap(cart => {
          let updatedCart: ShoppingCart;

          if (!cart) {
            // ✅ الكارت مش موجودة → أنشئ واحدة جديدة
            updatedCart = { id: cartId, cartItems: [newItem] };
          } else {
            // ✅ الكارت موجودة → عدّلها
            const existingItem = cart.cartItems.find(i => i.productId === product.id);
            if (existingItem) {
              existingItem.quantity += quantity;
            } else {
              cart.cartItems.push(newItem);
            }
            updatedCart = cart;
          }

          // ✅ احفظ الكارت المحدثة
          return this.saveCart(updatedCart);
        })
      )
      .subscribe({
        next: (updatedCart) => this.setCart(updatedCart),
        error: (err) => console.error('Error adding item to cart:', err)
      });
  }

  removeItemFromCart(productId: number): void {
    const currentCart = this.cartSource.value;
    if (!currentCart) return;

    currentCart.cartItems = currentCart.cartItems.filter(i => i.productId !== productId);
    this.saveCart(currentCart).subscribe({
      next: (updatedCart) => this.setCart(updatedCart),
      error: (err) => console.error('Error removing item:', err)
    });
  }

  updateItemQuantity(productId: number, quantity: number): void {
    const currentCart = this.cartSource.value;
    if (!currentCart) return;

    const item = currentCart.cartItems.find(i => i.productId === productId);
    console.log(item)
    if (item) {
      item.quantity = quantity;
      console.log(item)
      this.saveCart(currentCart).subscribe({
        next: (updatedCart) => { console.log("Updated cart in updateItemQuantity:", updatedCart); this.setCart(updatedCart)},
        error: (err) => console.error('Error updating quantity:', err)
      });
    }
  }
  clearCart(): void {
    const cartId = this.getOrCreateCartId();
    this.http.delete<void>(`${this.baseUrl}/${cartId}`).subscribe({
      next: () => {
        const emptyCart: ShoppingCart = { id: cartId, cartItems: [] };
        this.setCart(emptyCart);
      },
      error: err => console.error('Error clearing cart:', err)
    });
  }

  getTotalPrice(): number {
    const cart = this.cartSource.value;
    if (!cart) return 0;
    return cart.cartItems.reduce((total, item) => total + item.price * item.quantity, 0);
  }
}
