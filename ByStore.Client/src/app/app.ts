import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';;
import { NavBar } from './shared/component/nav-bar/nav-bar';
import { LoadingSpinner } from "./shared/component/loading-spinner/loading-spinner";
import { Footer } from "./shared/component/footer/footer";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NavBar, LoadingSpinner, Footer],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
}
