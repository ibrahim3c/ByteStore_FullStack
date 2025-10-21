import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-contact',
  imports: [FormsModule],
  templateUrl: './contact.html',
  styleUrl: './contact.css'
})
export class Contact {
    contact = {
    name: '',
    email: '',
    message: ''
  };
  constructor(private toastr: ToastrService) {}
  onSubmit() {
    this.toastr.success('Message sent successfully!', 'Success');
    this.contact = { name: '', email: '', message: '' };
  }
}
