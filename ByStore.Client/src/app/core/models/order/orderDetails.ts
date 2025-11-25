import { OrderStatus } from "./order";
import { OrderAddress } from "./orderAddress";
import { GetOrderItem } from "./orderItem";

export interface GetOrderDetails {
  id: string;
  orderDate: string;
  totalAmount: number;
  status: OrderStatus;

  customerId: string;
  customerName: string;

  shippingAddressId: number;
  shippingAddress: OrderAddress;

  billingAddressId: number;
  billingAddress: OrderAddress;

  orderItems: GetOrderItem[];
}
