import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login  implements OnInit {
  loginForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loginForm = this.fb.group({
      Email: ['', [Validators.required, Validators.email]],
      Password: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      const userLogin = this.loginForm.value;
      this.authService.login(userLogin).subscribe({
        next: (response: any) => {
          this.toastr.success('Welcome back!', 'Login Successful');
          this.router.navigate(['/']);
        },
        error: (err) => {
          const message = err.error?.message || 'Invalid email or password';
          this.toastr.error(message, 'Login Failed');
        }
      });
    } else {
      this.loginForm.markAllAsTouched();
    }
  }
}
