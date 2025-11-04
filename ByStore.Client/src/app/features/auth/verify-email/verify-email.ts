import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-verify-email',
  template: `
    <div class="container text-center mt-5">
      <h3>{{ message }}</h3>
    </div>
  `
})
export class VerifyEmail implements OnInit {
  message = 'Verifying your email...';

  constructor(
    private route: ActivatedRoute,
    private authService: AuthService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit() {
    const userId = this.route.snapshot.queryParamMap.get('userId');
    const code = this.route.snapshot.queryParamMap.get('code');

    if (userId && code) {
      this.authService.verifyEmail(userId, code).subscribe({
        next: (res: any) => {
          this.message = res.message;
          this.toastr.success("Email verified successfully!", 'Success');
          setTimeout(() => this.router.navigate(['/login']), 3000);
        },
        error: (err) => {
          this.message = err.error.message || 'Invalid or expired verification link.';
          this.toastr.error(this.message);
        }
      });
    } else {
      this.message = 'Invalid verification link.';
      this.toastr.error(this.message);
    }
  }
}
