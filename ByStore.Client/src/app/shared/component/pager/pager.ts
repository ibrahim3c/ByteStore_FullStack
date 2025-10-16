import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MetadataOverride } from '@angular/core/testing';
import { MetaData } from '../../../core/models/MetaData';

@Component({
  selector: 'app-pager',
  imports: [CommonModule],
  templateUrl: './pager.html',
  styleUrl: './pager.css'
})
export class Pager {
pageNumber: number=1;
@Output() onPageChanges=new EventEmitter();

get totalPages(): number[] {
  return Array.from({ length: this.metaData?.TotalPages || 0 }, (_, i) => i + 1);
}

 lastClickTime=0;
onPageChange(pageNumber: number) {
     const now = Date.now();
  if (now - this.lastClickTime < 500) return; // ignore clicking if less than 0.5s
  this.lastClickTime = now;


  this.pageNumber=pageNumber;
  this.onPageChanges.emit(pageNumber);
}

@Input() metaData?: MetaData;

}
