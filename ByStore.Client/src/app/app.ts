import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {MatToolbarModule} from '@angular/material/toolbar';
import { NavBar } from './shared/component/nav-bar/nav-bar';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet,MatToolbarModule,NavBar],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('ByStore.Client');
}
