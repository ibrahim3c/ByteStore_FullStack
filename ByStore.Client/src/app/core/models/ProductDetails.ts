import { ProductImage } from "./ProductImage";
import { ProductReview } from "./ProductReview";

export interface MyProductDetails {
  id: number;
  name: string;
  description: string;
  price: number;
  stockQuantity: number;

  images: ProductImage[];
  reviews: ProductReview[];

  brandName: string;
  categoryName: string;
}
