import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { catchError, Observable, throwError } from "rxjs";
import { environment } from "../../../environments/environment";
import { Brand } from "../models/Brand";

@Injectable({
  providedIn:'any'
})
export class BrandService{
  private httpClient=inject(HttpClient)
  private readonly apiUrl=`${environment.baseUrl}/brands`;
  getAllBrands(){
    return this.httpClient.get<Brand[]>(`${this.apiUrl}`).pipe(catchError(this.handleError));
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
