import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';;
import { NavBar } from './shared/component/nav-bar/nav-bar';
import { LoadingSpinner } from "./shared/component/loading-spinner/loading-spinner";
import { LoadingService } from './core/services/loading.service';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NavBar, LoadingSpinner, AsyncPipe],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  private loadingService = inject(LoadingService);
  loading$ = this.loadingService.loading$;
}
