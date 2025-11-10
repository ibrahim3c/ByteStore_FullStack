import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CartService } from '../../../core/services/cart.service';
import {  Observable } from 'rxjs';
import { ShoppingCart } from '../../../core/models/cart/shoppingCart';
import { User } from '../../../core/models/auth/User';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-nav-bar',
  imports: [
    CommonModule,
    FormsModule,
    RouterLink,
    NgbModule
],
  templateUrl: './nav-bar.html',
  styleUrl: './nav-bar.css'

})
export class NavBar implements OnInit {
  private cartService=inject(CartService);
  private authService=inject(AuthService);
  cart$?:Observable<ShoppingCart | null>;
  user$?:Observable<User | null>;

  isCollapsed = true;
  searchQuery = '';

  ngOnInit(){
    this.cart$=this.cartService.cart$;
    this.user$=this.authService.$user
  }

  logout(): void {
    this.authService.logout();
  }
}
