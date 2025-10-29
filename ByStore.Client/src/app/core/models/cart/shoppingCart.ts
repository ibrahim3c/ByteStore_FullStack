import { CartItem } from "./CartItem";

export interface ShoppingCart {
  id: string;
  cartItems: CartItem[];
}
