import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { NgbCollapseModule } from '@ng-bootstrap/ng-bootstrap';

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
export class NavBar {
cartCount:number=3
  isCollapsed = true;
    searchQuery = '';

  onSearch() {
    console.log('Searching for:', this.searchQuery);
  }
}
