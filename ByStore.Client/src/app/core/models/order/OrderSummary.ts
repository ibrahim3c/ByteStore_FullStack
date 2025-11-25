import { OrderStatus } from "./order";

export interface OrderSummary {
  id: string;
  orderDate: string;
  totalAmount: number;
  status: OrderStatus;
  itemsCount: number;
}
