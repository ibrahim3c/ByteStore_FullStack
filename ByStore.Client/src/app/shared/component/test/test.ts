import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-test',
  imports: [MatCardModule,MatButtonModule, MatDividerModule, MatIconModule],
  templateUrl: './test.html',
  styleUrl: './test.css'
})
export class Test {

}
