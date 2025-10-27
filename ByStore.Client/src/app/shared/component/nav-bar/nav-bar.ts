import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { NgbCollapseModule } from '@ng-bootstrap/ng-bootstrap';
import { CartService } from '../../../core/services/cart.service';
import { Observable } from 'rxjs';
import { IShoppingCart } from '../../../core/models/cart/shoppingCart';

@Component({
  selector: 'app-nav-bar',
  imports: [
    CommonModule,
    FormsModule,
    NgbCollapseModule,
    RouterLink
],
  templateUrl: './nav-bar.html',
  styleUrl: './nav-bar.css'

})
export class NavBar implements OnInit {
  private cartService=inject(CartService);
  cart$?:Observable<IShoppingCart | null>;

  isCollapsed = true;
  searchQuery = '';

  ngOnInit(){
    this.cart$=this.cartService.cart$;
  }

}
