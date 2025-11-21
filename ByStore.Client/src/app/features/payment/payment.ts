import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { OrderService } from '../../core/services/order.service';
import { PlaceOrder } from '../../core/models/order/placeOrder';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-payment',
  imports: [CommonModule],
  templateUrl: './payment.html',
  styleUrl: './payment.css',
})
export class Payment {
  @Input() shippingAddressId!: number;
  @Input() billingAddressId!: number;

  @Output() back = new EventEmitter<void>();
  @Output() orderComplete = new EventEmitter<void>();

  isLoading = false;

  constructor(private orderService: OrderService, private toaster: ToastrService) {}

  placeOrder(): void {
    const cartId= localStorage.getItem('cart_id');
    if (!this.shippingAddressId || !this.billingAddressId || !cartId) {
      this.toaster.error('Missing required information');
      return;
    }

    this.isLoading = true;

    const placeOrder: PlaceOrder = {
      billingAddressId: this.billingAddressId,
      shippingAddressId: this.shippingAddressId,
      customerId: cartId,
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

  goBack() {
    this.back.emit();
  }
}

