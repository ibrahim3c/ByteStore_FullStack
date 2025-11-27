export interface CartItem {
  productId: number;
  name: string;
  price: number; // snapshot of the price when added
  quantity: number;
  imageUrl: string;
  brandName?: string;     // optional
  categoryName?: string;  // optional
}
