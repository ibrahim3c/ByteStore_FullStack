import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { catchError, Observable, throwError } from 'rxjs';
import { Customer } from '../models/customer/customer';
import { Address } from '../models/customer/address';

@Injectable({
  providedIn: 'any',
})
export class CustomerService {
  private httpClient = inject(HttpClient);
  private readonly apiUrl = `${environment.baseUrl}/customers`;

  getCustomerByUserId(userId: string): Observable<Customer> {
    return this.httpClient
      .get<Customer>(`${this.apiUrl}/user/${userId}`)
      .pipe(catchError(this.handleError));
  }
  updateCustomer(customerId: string, customer: Customer) {
    const updatedCustomer = {
      firstName: customer.firstName,
      lastName: customer.lastName,
      dateOfBirth: customer.dateOfBirth,
      phoneNumber: customer.phoneNumber,
    };
    return this.httpClient
      .put<Customer>(`${this.apiUrl}/${customerId}`, updatedCustomer)
      .pipe(catchError(this.handleError));
  }

  getCustomerAddresses(customerId: string|undefined): Observable<Address[]> {
    return this.httpClient
      .get<Address[]>(`${this.apiUrl}/${customerId}/addresses`)
      .pipe(catchError(this.handleError));
  }
  addCustomerAddress(customerId: string|undefined, address: Address) {
    return this.httpClient
      .post<Address[]>(`${this.apiUrl}/${customerId}/addresses`, address)
      .pipe(catchError(this.handleError));
  }
  updateCustomerAddress(customerId: string |undefined, addressId: number, address: Address) {
    return this.httpClient
      .put<Address[]>(`${this.apiUrl}/${customerId}/addresses/${addressId}`, address)
      .pipe(catchError(this.handleError));
  }

  deleteCustomerAddress(customerId: string|undefined, addressId: number) {
    return this.httpClient
      .delete<Address[]>(`${this.apiUrl}/${customerId}/addresses/${addressId}`)
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
