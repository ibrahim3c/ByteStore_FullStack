import { ProductImage } from "./ProductImage";
import { ProductReview } from "./ProductReview";

export interface ProductDetails {
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
