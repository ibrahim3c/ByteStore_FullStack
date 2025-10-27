import { CartItem } from "./CartItem";
import { v4 as uuidv4 } from 'uuid';

export interface IShoppingCart {
  id: string;
  cartItems: CartItem[];
}

export class ShoppingCart implements IShoppingCart {
  id=uuidv4();
  cartItems: CartItem[]=[];
}
