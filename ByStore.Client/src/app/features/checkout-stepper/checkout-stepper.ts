import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { AddressSelector } from '../../shared/component/address-selector/address-selector';
import { Address } from '../../core/models/customer/address';
import { CustomerService } from '../../core/services/customer.service';
import { Customer } from '../../core/models/customer/customer';
import { AuthService } from '../../core/services/auth.service';
import { Observable } from 'rxjs';
import { User } from '../../core/models/auth/User';
import { Payment } from '../payment/payment';

@Component({
  selector: 'app-stepper',
  imports: [CommonModule, AddressSelector, Payment],
  templateUrl: './checkout-stepper.html',
  styleUrl: './checkout-stepper.css',
})
export class CheckoutStepper {
  currentStep = 0;
  addresses: Address[] = [];
  shippingAddressId!: number;
  billingAddressId!: number;
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

          this.customerService.getCustomerAddresses(customer.id).subscribe((adds) => {
            this.addresses = adds;
          });
        });
      }
    });
  }

  steps = [{ label: 'Select Address' }, { label: 'Payment' }];

  goNextAddresses(event: { shipping: number; billing: number }) {
    this.shippingAddressId = event.shipping;
    this.billingAddressId = event.billing;

    this.goNext();
  }
  goNext() {
    if (this.currentStep < this.steps.length - 1) {
      this.currentStep++;
    }
  }

  goBack() {
    if (this.currentStep > 0) {
      this.currentStep--;
    }
  }
}
