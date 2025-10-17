import { Component, inject, Input } from '@angular/core';
import { MyProductDetails } from '../../../core/models/ProductDetails';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from '../../../core/services/product.service';
@Component({
  selector: 'app-product-details',
  imports: [CommonModule],
  templateUrl: './product-details.html',
  styleUrl: './product-details.css'
})
export class ProductDetails {

  private activateRoute = inject(ActivatedRoute);
  private productService = inject(ProductService);

  product!: MyProductDetails;

  primaryImageUrl: string = '';
  otherImages: string[] = [];

  ngOnInit() {
    const productId= this.activateRoute.snapshot.paramMap.get('id');
    this.productService.getProductDetails(+productId!).subscribe((data)=>{
      this.product=data;
      if(this.product && this.product.images?.length){
        const primary=this.product.images.find(img=>img.isPrimary)
        this.primaryImageUrl=primary ? primary.imageUrl :this.product.images[0].imageUrl;
        this.otherImages = this.product.images.filter(i => !i.isPrimary).map(i => i.imageUrl);
      }
    })
  }
  changeImage(imageUrl: string) {
    this.primaryImageUrl = imageUrl;
  }

  getStars(rating: number): number[] {
    return Array(Math.round(rating)).fill(0);
  }

  
}
