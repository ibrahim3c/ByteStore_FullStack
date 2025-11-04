import { Component, inject } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { UserRegister } from '../../../core/models/auth/userRegister';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { ToastrService } from 'ngx-toastr';


function equalValidator(control: AbstractControl): ValidationErrors | null {
  const password = control.get('Password');
  const confirm = control.get('ConfirmPassword');
  if (!password || !confirm) return null;
  return password?.value === confirm?.value ? null : { notEqual: true };
};
@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule,CommonModule,RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  private authService: AuthService=inject(AuthService);
  private router=inject(Router);
  private toastr = inject(ToastrService);

   registerForm: FormGroup;
  constructor(private fb: FormBuilder) {
    this.registerForm = this.fb.group({
      Fname: ['', [Validators.required, Validators.minLength(2)]],
      Lname: ['', [Validators.required, Validators.minLength(2)]],
      PhoneNumber: ['', [Validators.required, Validators.pattern('^[0-9]{10,15}$')]],
      BirthDate: ['', Validators.required],
      Address: ['', Validators.required],
      Email: ['', [Validators.required, Validators.email]],
      passwords:this.fb.group({
        Password: ['', [Validators.required, Validators.minLength(6)]],
        ConfirmPassword: ['', Validators.required]
      }, {validators: equalValidator})
    });
  }

  onSubmit() {
    if (this.registerForm.valid) {
      const formValue = this.registerForm.value;
      const user: UserRegister = {
        Fname: formValue.Fname,
        Lname: formValue.Lname,
        Email: formValue.Email,
        PhoneNumber: formValue.PhoneNumber,
        BirthDate: formValue.BirthDate,
      Address: formValue.Address,
      Password: formValue.passwords.Password,
      ConfirmPassword: formValue.passwords.ConfirmPassword
    };

      console.log('User Registered:', user);
      this.authService.register(user).subscribe({
        next: (response:any) => {
          console.log('Response', response);
          this.toastr.success(response.message ||'Please verify your email before logging in.', 'Registration Successful');
          this.router.navigate(['/login']);
        },
        error: (error) => {
          console.error('Registration failed', error);
        }
      });
    } else {
      this.registerForm.markAllAsTouched();
    }
}
}
