import { AfterViewInit, Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { OrderService } from '../../core/services/order.service';
import { PlaceOrder } from '../../core/models/order/placeOrder';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { CustomerService } from '../../core/services/customer.service';
import { Customer } from '../../core/models/customer/customer';
import { AuthService } from '../../core/services/auth.service';
import { Observable } from 'rxjs';
import { User } from '../../core/models/auth/User';
import { loadStripe, Stripe, StripeCardElement } from '@stripe/stripe-js';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-payment',
  imports: [CommonModule],
  templateUrl: './payment.html',
  styleUrl: './payment.css',
})
export class Payment implements OnInit, AfterViewInit {
  @Input() shippingAddressId!: number;
  @Input() billingAddressId!: number;

  @Output() back = new EventEmitter<void>();
  @Output() orderComplete = new EventEmitter<void>();

  isLoading = false;

  customer?: Customer;
  private customerService = inject(CustomerService);
  private userService = inject(AuthService);
  constructor(private orderService: OrderService, private toaster: ToastrService) {}

  user$?: Observable<User | null>;

    // Stripe variables
  stripe!: Stripe | null;
  cardElement!: StripeCardElement;

  ngOnInit(): void {
    this.user$ = this.userService.$user;

    this.user$?.subscribe((user) => {
      if (user) {
        this.customerService.getCustomerByUserId(user.userId).subscribe((customer) => {
          this.customer = customer;
        });
      }
    });
  }

    async ngAfterViewInit() {
  this.stripe = await loadStripe(environment.stripePublishableKey);
    if (!this.stripe) {
      this.toaster.error('Stripe failed to load');
      return;
    }

    const elements = this.stripe.elements();
    this.cardElement = elements.create('card');
    this.cardElement.mount('#card-element'); // اربط بالـ div في HTML
  }


  placeOrder(): void {
    const cartId = localStorage.getItem('cart_id');
    const customerId = this.customer?.id;
    if (!this.shippingAddressId || !this.billingAddressId || !cartId || !customerId) {
      this.toaster.error('Missing required information');
      return;
    }

    this.isLoading = true;

    const placeOrder: PlaceOrder = {
      cartId: cartId,
      billingAddressId: this.billingAddressId,
      shippingAddressId: this.shippingAddressId,
      customerId: customerId,
    };

    this.orderService.placeCustomerOrder(placeOrder).subscribe({
      next: (value) => {
        this.isLoading = false;
        this.toaster.success('Order Created Successfully');
        this.orderComplete.emit();
      },
      error: (err) => {
        this.isLoading = false;
        this.toaster.error('Something Went Wrong');
      },
    });
  }

  createPaymentIntent(): void {
    const cartId = localStorage.getItem('cart_id');
    if (!cartId) {
      this.toaster.error('Cart ID not found');
      return;
    }

    this.isLoading = true;

    this.orderService.createOrUpdatePaymentIntent(cartId).subscribe({
      next: (res) => {
        this.isLoading = false;
        this.openStripePayment(res.clientSecret);
      },
      error: (err) => {
        this.isLoading = false;
        this.toaster.error('PaymentIntent creation failed');
      }
    });
  }

  async openStripePayment(clientSecret: string) {
    if (!this.stripe || !this.cardElement) {
      this.toaster.error('Stripe not initialized properly');
      return;
    }

    const result = await this.stripe.confirmCardPayment(clientSecret, {
      payment_method: {
        card: this.cardElement,
      },
    });

    if (result.paymentIntent?.status === 'succeeded') {
      this.placeOrder();
    } else {
      this.toaster.error(result.error?.message || 'Payment failed');
    }
  }

  goBack() {
    this.back.emit();
  }
}
