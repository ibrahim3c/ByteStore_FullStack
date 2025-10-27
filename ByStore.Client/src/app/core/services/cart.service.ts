import { inject, Injectable } from "@angular/core";
import { BehaviorSubject, catchError, map, Observable, throwError } from "rxjs";
import { IShoppingCart, ShoppingCart } from "../models/cart/shoppingCart";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../environments/environment";
import { CartItem } from "../models/cart/CartItem";
import { Product } from "../models/Product";

@Injectable({
  providedIn: 'root'
})
export class CartService{
  private cart = new BehaviorSubject<IShoppingCart | null>(null);
  cart$ = this.cart.asObservable();

  private http=inject(HttpClient)
  private readonly apiUrl=`${environment.baseUrl}/ShoppingCarts`;


  setCart(cart: IShoppingCart | null){
    this.cart.next(cart);
  }
    getCurrentCartValue(): IShoppingCart | null {
    return this.cart.getValue();
  }

    getCart(id: string): Observable<IShoppingCart> {
    return this.http.get<IShoppingCart>(`${this.apiUrl}/${id}`).pipe(
      map((cart) => {
        this.setCart(cart);
        return cart;
      }),
      catchError(this.handleError)
    );
  }

  saveCart(cart: IShoppingCart): Observable<IShoppingCart> {
    return this.http.post<IShoppingCart>(this.apiUrl, cart).pipe(
      map((savedCart) => {
        this.setCart(savedCart);
        return savedCart;
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
      cart = new ShoppingCart();
      // store cartId in local storage
      localStorage.setItem('cartId', cart.id);
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


  clearCart(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
      map(() => this.setCart(null)),
      catchError(this.handleError)
    );
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
