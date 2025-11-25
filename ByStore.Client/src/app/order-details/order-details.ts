import { Component } from '@angular/core';
import { GetOrderItem } from '../core/models/order/orderItem';
import { ActivatedRoute } from '@angular/router';
import { OrderService } from '../core/services/order.service';
import { OrderStatus } from '../core/models/order/order';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { GetOrderDetails } from '../core/models/order/orderDetails';

@Component({
  selector: 'app-order-details',
  imports: [CurrencyPipe,CommonModule,DatePipe],
  templateUrl: './order-details.html',
  styleUrl: './order-details.css'
})
export class OrderDetails {
  order!: GetOrderDetails;
  loading = true;

  constructor(
    private route: ActivatedRoute,
    private orderService: OrderService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get("id");

    if (id)
      this.getOrder(id);
  }

  getOrder(id: string) {
    this.orderService.getOrderById(id).subscribe({
      next: (res) => {
        this.order = res;
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      }
    });
  }

  getTotalItemPrice(item: GetOrderItem) {
    return item.quantity * item.unitPrice;
  }
  formatStatus(status: OrderStatus) {
    switch(status){
      case OrderStatus.Pending: return 'Pending';
      case OrderStatus.PaymentReceived: return 'PaymentReceived';
      case OrderStatus.PaymentFailed: return 'PaymentFailed';
    }
  }

}
