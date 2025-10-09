import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { Category } from "../models/Category";
import { catchError, Observable, throwError } from "rxjs";

@Injectable({
  providedIn:'root'
})
export class CategoryService{
  private httpClient=inject(HttpClient)
  private readonly apiUrl=`${environment.baseUrl}/categories`;
  getAllCategories(){
    return this.httpClient.get<Category[]>(`${this.apiUrl}/tree`).pipe(catchError(this.handleError));
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
