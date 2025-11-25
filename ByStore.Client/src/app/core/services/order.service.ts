import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Order } from '../models/order/order';
import { PlaceOrder } from '../models/order/placeOrder';
import { PaymentIntent } from '../models/order/paymentIntent';
import { OrderSummary } from '../models/order/OrderSummary';
import { OrderDetails } from '../../order-details/order-details';
import { GetOrderDetails } from '../models/order/orderDetails';

@Injectable({
  providedIn: 'any',
})
export class OrderService {
  private httpClient = inject(HttpClient);
  private readonly apiUrl = `${environment.baseUrl}/orders`;
  getCustomerOrders() {
    return this.httpClient.get<Order[]>(`${this.apiUrl}`).pipe(catchError(this.handleError));
  }

  placeCustomerOrder(placeOrder: PlaceOrder) {
    return this.httpClient.post(`${this.apiUrl}`, placeOrder).pipe(catchError(this.handleError));
  }
  createOrUpdatePaymentIntent(cartId: string): Observable<PaymentIntent> {
    return this.httpClient
      .post<PaymentIntent>(
        `${environment.baseUrl}/payments/payment-intent`,
        { cartId } // JSON body
      )
      .pipe(catchError(this.handleError));
  }
  getAllCustomerOrders(customerId: string) {
    return this.httpClient
      .get<OrderSummary[]>(`${this.apiUrl}/my-orders/${customerId}`)
      .pipe(catchError(this.handleError));
  }
  getOrderById(orderId:string){
        return this.httpClient
      .get<GetOrderDetails>(`${this.apiUrl}/${orderId}`)
      .pipe(catchError(this.handleError));

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
