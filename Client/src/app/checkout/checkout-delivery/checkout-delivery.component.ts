import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { DeliveryMethod } from 'src/app/shared/models/deliveryMethod';
import { CheckoutService } from '../checkout.service';
import { BasketService } from 'src/app/basket/basket.service';

@Component({
  selector: 'app-checkout-delivery',
  templateUrl: './checkout-delivery.component.html',
  styleUrls: ['./checkout-delivery.component.scss']
})
export class CheckoutDeliveryComponent implements OnInit {
  @Input() checkoutForm?:FormGroup;
  deliveryMethods:DeliveryMethod[]=[];
  constructor(private checkoutSerive:CheckoutService, private basketService:BasketService) { }

  ngOnInit(): void {
    this.checkoutSerive.getDeliveryMethods().subscribe({
      next: dm => {
        console.log(dm)
        this.deliveryMethods = dm
      }
    })
  }

  setShippingPrice(deliveryMethod:DeliveryMethod){
    this.basketService.setShippingPrice(deliveryMethod);
  }

}
