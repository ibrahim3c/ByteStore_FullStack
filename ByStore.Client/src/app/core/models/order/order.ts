import { OrderAddress } from "./orderAddress";
import { GetOrderItem } from "./orderItem";

export interface Order {
  id: string;                  // Guid
  orderDate: string;        
  totalAmount: number;
  status: OrderStatus;         // Enum
  customerId: string;          // Guid
  customerName: string;

  shippingAddressId: number;
  shippingAddress: OrderAddress;

  billingAddressId: number;
  billingAddress: OrderAddress;

  orderItems: GetOrderItem[];
}


export enum OrderStatus {
  Pending = "Pending",
  PaymentReceived = "Payment Received",
  PaymentFailed = "Payment Failed"
}
