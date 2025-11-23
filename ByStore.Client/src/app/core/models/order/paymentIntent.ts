export interface PaymentIntent {
    clientSecret: string;
    paymentIntentId: string;
    status: string;
    amount: number;
    currency: string;
}
