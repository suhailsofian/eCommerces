import { Component, Input, OnInit } from '@angular/core';
import { Cart, Payment, Product } from '../models/models';
import { UtilityService } from '../services/utility.service';
import { Router } from '@angular/router';
import { NavigationService } from '../services/navigation.service';
@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css'],
})
export class ProductComponent implements OnInit {
  @Input() view: 'grid' | 'list' | 'currcartitem' | 'prevcartitem' = 'grid';
  @Input() product: Product = {
    id: 0,
    title: '',
    description: '',
    price: 0,
    quantity: 0,
    productCategory: {
      id: 1,
      category: '',
      subCategory: '',
    },
    offer: {
      id: 1,
      title: '',
      discount: 0,
    },
    imageName: '',
  };

  constructor(public utilityService: UtilityService,public navigationService: NavigationService) {}

 
  refreshCartItems() {
    window.location.reload();
    // Function to refresh cart items
    // this.navigationService
    // .getActiveCartOfUser(this.utilityService.getUser().id)
    // .subscribe((res: any) => {
    //   this.usersCart = res;

    //   // Calculate Payment
    //   this.utilityService.calculatePayment(
    //     this.usersCart,
    //     this.usersPaymentInfo
    //   );
    // });
  }

  
  onButtonClick(product: Product) {
    // Wrapper function to call multiple functions
   
    this.utilityService.removeFromCart(product)
    this.refreshCartItems();
  }
  ngOnInit(): void {}
}
