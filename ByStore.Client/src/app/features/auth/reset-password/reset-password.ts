import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { ResetPasswordRequest } from '../../../core/models/auth/resetPassword';

@Component({
  selector: 'app-reset-password',
  imports: [CommonModule ,ReactiveFormsModule],
  templateUrl: './reset-password.html',
  styleUrl: './reset-password.css'
})
export class ResetPassword {
    resetForm: FormGroup;
  submitted = false;
  userId = '';
  code = '';
  message = '';
  error = '';

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private authService: AuthService
  ) {
    this.resetForm = this.fb.group({
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]]
    }, { validators: this.passwordMatchValidator });
  }

  ngOnInit(): void {
    this.userId = this.route.snapshot.queryParamMap.get('userId') || '';
    this.code = this.route.snapshot.queryParamMap.get('code') || '';
  }

  get f() {
    return this.resetForm.controls;
  }

  passwordMatchValidator(form: FormGroup) {
    return form.get('newPassword')?.value === form.get('confirmPassword')?.value
      ? null : { mismatch: true };
  }

  onSubmit() {
    this.submitted = true;
    this.message = '';
    this.error = '';

    if (this.resetForm.invalid) return;

    const data = new ResetPasswordRequest();
    data.userId = this.userId;
    data.code = this.code;
    data.newPassword = this.f['newPassword'].value;

    this.authService.resetPassword(data).subscribe({
      next: (res: any) => {
        this.message = res.message || 'Password reset successfully.';
        this.resetForm.reset();
      },
      error: (err) => {
        console.error(err);
        this.error = err.error?.message || 'Something went wrong. Please try again.';
      }
    });
  }
}
