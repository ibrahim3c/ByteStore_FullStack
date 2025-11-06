import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';
import { ForgotPasswordRequest } from '../../../core/models/auth/ForgotPassword';

@Component({
  selector: 'app-forgot-password',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './forgot-password.html',
  styleUrl: './forgot-password.css'
})
export class ForgotPassword {
forgotForm: FormGroup;
  submitted = false;
  message = '';
  error = '';

  constructor(private fb: FormBuilder, private authService: AuthService) {
    this.forgotForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  get f() {
    return this.forgotForm.controls;
  }

  onSubmit() {
    console.log('Forgot Password form submitted');
    this.submitted = true;
    this.message = '';
    this.error = '';

    if (this.forgotForm.invalid) return;

    const forgotPassword = new ForgotPasswordRequest();
    forgotPassword.email = this.forgotForm.value.email;

    this.authService.forgotPassword(forgotPassword).subscribe({
      next: (res: any) => {
        console.log(res);
        this.message = res.message || 'Please check your email to reset your password.';
        this.forgotForm.reset();
        this.submitted = false;
      },
      error: (err) => {
        console.error(err);
        this.error = err.error?.message || 'Something went wrong. Please try again.';
      }
    });
  }}
