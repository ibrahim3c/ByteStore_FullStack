import { RequestParameters } from "./RequestParameters";

export class ProductParameters extends RequestParameters {
  MinPrice?: number;
  MaxPrice?: number;
  CategoryId?: number;
  BrandId?: number;
  SearchTerm?: string;
}

