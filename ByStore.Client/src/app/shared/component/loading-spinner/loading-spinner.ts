import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { Observable } from 'rxjs';
import { SpinnerService } from '../../../core/services/loading.service';

@Component({
  selector: 'app-loading-spinner',
  imports: [CommonModule],
  templateUrl: './loading-spinner.html',
  styleUrl: './loading-spinner.css'
})
export class LoadingSpinner {
visible$: Observable<boolean>;
  constructor(private spinner: SpinnerService) {
    this.visible$ = this.spinner.visible$;
  }}
