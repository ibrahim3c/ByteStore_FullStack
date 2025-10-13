import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Product } from '../models/Product';
import * as environment from '../../../environments/environment';
import { catchError, Observable, throwError } from 'rxjs';
import { ProductParameters } from '../models/ProductParameters';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private httpClient = inject(HttpClient);
  private readonly apiUrl = `${environment.environment.baseUrl}/products`;

  getProducts(productParams:ProductParameters) {
let params = new HttpParams()
  .set('pageNumber', productParams.PageNumber ?? 1)
  .set('pageSize', productParams.PageSize ?? 10);

if (productParams.MinPrice != null)
  params = params.set('minPrice', productParams.MinPrice);

if (productParams.MaxPrice != null)
  params = params.set('maxPrice', productParams.MaxPrice);

if (productParams.CategoryId)
  params = params.set('categoryId', productParams.CategoryId);

if (productParams.BrandId)
  params = params.set('brandId', productParams.BrandId);

if (productParams.SearchTerm)
  params = params.set('searchTerm', productParams.SearchTerm);

if (productParams.OrderBy)
  params = params.set('orderBy', productParams.OrderBy);

    return this.httpClient.get(`${this.apiUrl}`, { params ,observe:'response'})
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
