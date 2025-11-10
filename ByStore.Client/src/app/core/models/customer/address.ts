export interface Address{
  id: number;
  street: string;
  city: string;
  state: string;
  postalCode: string;
  country: string;
  isPrimary: boolean;
  addressType: string; // Shipping or Billing
  customerName: string;
}
