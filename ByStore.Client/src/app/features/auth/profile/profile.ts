import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormsModule, NgModel } from '@angular/forms';
import { NgbDropdown, NgbDropdownModule, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Observable, switchMap } from 'rxjs';
import { Customer } from '../../../core/models/customer/customer';
import { CustomTransformer } from 'typescript';
import { CustomerService } from '../../../core/services/customer.service';
import { AuthService } from '../../../core/services/auth.service';
import { User } from '../../../core/models/auth/User';
import { ToastrService } from 'ngx-toastr';
import { Address } from '../../../core/models/customer/address';

@Component({
  selector: 'app-profile',
  imports: [CommonModule, FormsModule, NgbDropdownModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css',
})
export class Profile implements OnInit {
  customer?: Customer;
  private customerService = inject(CustomerService);
  private userService = inject(AuthService);
  private toastr = inject(ToastrService);
  user$?: Observable<User | null>;

  ngOnInit(): void {
    this.user$ = this.userService.$user;

    this.user$?.subscribe((user) => {
      if (user) {
        this.customerService.getCustomerByUserId(user.userId).subscribe((customer) => {
          console.log('the customer ', customer);
          this.customer = customer;

          this.customerService.getCustomerAddresses(customer.id).subscribe((adds) => {
            this.addresses = adds;
          });
        });
      }
    });
  }

  saveChanges() {
    console.log('save changes');
    if (this.customer?.id) {
      this.customerService.updateCustomer(this.customer.id, this.customer).subscribe({
        next: (response: any) => {
          this.toastr.success('Your changes have been saved successfully.', 'Success');
        },
      });
    } else {
      console.error('Customer ID is missing!');
    }
  }

  // address
  addresses: Address[] = [];
  currentAddress: Address = this.newEmptyAddress();
  isEditing: boolean = false; // to know if we add or edit null = add , number = edit

  @ViewChild('addressModal') addressModal!: TemplateRef<any>;
  private modalService = inject(NgbModal);
  openAddressModal(addr?: Address, index?: number) {
    if (addr) {
      this.currentAddress = { ...addr };
      this.isEditing = true;
    } else {
      this.currentAddress = this.newEmptyAddress();
      this.isEditing = false;
    }
    this.modalService.open(this.addressModal, { backdrop: 'static', centered: true });
  }

  saveAddress(modal: any) {
    console.log('saveAddress starts');
    console.log('modal', modal);
    console.log('customerId', this.customer?.id);
    console.log('isEditing', this.isEditing);

    if (!this.customer?.id) return; // safety check
    if (this.isEditing) {
      // edit
      this.customerService
        .updateCustomerAddress(this.customer?.id, this.currentAddress.id, this.currentAddress)
        .subscribe({
          next: (updated) => {
            this.customerService.getCustomerAddresses(this.customer?.id).subscribe((adds) => {
              this.addresses = adds;
            });
            this.toastr.success('Address updated successfully', 'Success');
          },
          error: (err) => this.toastr.error(err.message || 'Error updating address', 'Error'),
        });
    } else {
      // add
      this.customerService.addCustomerAddress(this.customer?.id, this.currentAddress).subscribe({
        next: (newAddr) => {
          this.customerService.getCustomerAddresses(this.customer?.id).subscribe((adds) => {
            this.addresses = adds;
          });
          this.toastr.success('Address added successfully', 'Success');
        },
        error: (err) => this.toastr.error(err.message || 'Error adding address', 'Error'),
      });
    }

    modal.close();
  }

  deleteAddress(addressId: number) {
    if (!this.customer?.id) return;
    console.log("addressId",addressId)
    this.customerService
      .deleteCustomerAddress(this.customer?.id,addressId)
      .subscribe({
        next: (newAddr) => {
          this.customerService.getCustomerAddresses(this.customer?.id).subscribe((adds) => {
            this.addresses = adds;
          });
          this.toastr.success('Address deleted successfully', 'Success');
        },
        error: (err) => this.toastr.error(err.message || 'Error adding address', 'Error'),
      });
  }

  private newEmptyAddress(): Address {
    return {
      id: 0,
      street: '',
      city: '',
      state: '',
      postalCode: '',
      country: '',
      isPrimary: false,
      addressType: 'Shipping',
      customerName: '',
    };
  }
}
