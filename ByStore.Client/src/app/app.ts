import { Component, Inject, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavBar } from './core/components/nav-bar/nav-bar';
import { ProductList } from './features/products/product-list/product-list';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet,NavBar,ProductList],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('ByStore.Client');
}
