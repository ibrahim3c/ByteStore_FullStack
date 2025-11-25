import { Component, inject } from '@angular/core';
import { OrderSummary } from '../../core/models/order/OrderSummary';
import { Router } from '@angular/router';
import { OrderStatus } from '../../core/models/order/order';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { Customer } from '../../core/models/customer/customer';
import { CustomerService } from '../../core/services/customer.service';
import { AuthService } from '../../core/services/auth.service';
import { User } from '../../core/models/auth/User';
import { Observable } from 'rxjs';
import { OrderService } from '../../core/services/order.service';

@Component({
  selector: 'app-order-list',
  imports: [CommonModule, CurrencyPipe, DatePipe],
  templateUrl: './order-list.html',
  styleUrl: './order-list.css',
})
export class OrderList {
  orders: OrderSummary[] = [];
  private orderService=inject(OrderService)

  customer?: Customer;
  private customerService = inject(CustomerService);
  private userService = inject(AuthService);
  user$?: Observable<User | null>;


  ngOnInit(): void {
    this.user$ = this.userService.$user;

    this.user$?.subscribe((user) => {
      if (user) {
        this.customerService.getCustomerByUserId(user.userId).subscribe((customer) => {
          this.customer = customer;

          this.orderService.getAllCustomerOrders(customer.id).subscribe(orders=>{
            this.orders=orders
          })

        });
      }
    });
  }

  constructor(private router: Router) {}

  goToDetails(id: string) {
    this.router.navigate(['/order-details', id]);
  }

  // Convert "Payment Received" → "PaymentReceived"
  formatStatus(status: OrderStatus): string {
    return status.replace(/ /g, '');
  }
}
