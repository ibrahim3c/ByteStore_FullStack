import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormsModule, NgModel } from '@angular/forms';
import { NgbDropdown, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { Observable, switchMap } from 'rxjs';
import { Customer } from '../../../core/models/customer/customer';
import { CustomTransformer } from 'typescript';
import { CustomerService } from '../../../core/services/customer.service';
import { AuthService } from '../../../core/services/auth.service';
import { User } from '../../../core/models/auth/User';
import { ToastrService } from 'ngx-toastr';

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

  addresses = [
    {
      id: 1,
      street: '123 Tech Avenue',
      city: 'San Francisco',
      state: 'CA',
      postalCode: '94105',
      country: 'USA',
      isPrimary: true,
      addressType: 'Shipping',
    },
    {
      id: 2,
      street: '456 Innovation Street',
      city: 'San Jose',
      state: 'CA',
      postalCode: '94088',
      country: 'USA',
      isPrimary: false,
      addressType: 'Billing',
    },
  ];

  ngOnInit(): void {
    this.user$ = this.userService.$user;

    this.user$?.subscribe((user) => {
      if (user) {
        this.customerService.getCustomerByUserId(user.userId).subscribe((customer) => {
          console.log('the customer ', customer);
          this.customer = customer;
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
}
