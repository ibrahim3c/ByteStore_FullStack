import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Address } from '../../../core/models/customer/address';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-address-selector',
  imports: [CommonModule],
  templateUrl: './address-selector.html',
  styleUrl: './address-selector.css',
})
export class AddressSelector {
  @Input() addresses: Address[] = [];

  shippingAddresses: Address[] = [];
  billingAddresses: Address[] = [];

  ngOnChanges() {
    if (this.addresses) {
      this.shippingAddresses = this.addresses.filter(
        (a) => a.addressType.toLowerCase() === 'shipping'
      );
      this.billingAddresses = this.addresses.filter(
        (a) => a.addressType.toLowerCase() === 'billing'
      );
    }
  }

  @Output() next = new EventEmitter<{ shipping: number; billing: number }>();
  @Output() back = new EventEmitter<void>();

  shippingAddressId: number | null = null;
  billingAddressId: number | null = null;

  shippingError = false;
  billingError = false;

  goNext() {
    this.shippingError = !this.shippingAddressId;
    this.billingError = !this.billingAddressId;

    if (this.shippingError || this.billingError) return;

    this.next.emit({
      shipping: this.shippingAddressId!,
      billing: this.billingAddressId!,
    });
  }

  goBack() {
    this.back.emit();
  }

  goManageAddresses() {
    window.location.href = '/profile'; // change if your route is different
  }

  selectShipping(id: number) {
    this.shippingAddressId = id;
    this.shippingError = false;
  }

  selectBilling(id: number) {
    this.billingAddressId = id;
    this.billingError = false;
  }

}
