import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BasketService } from 'src/app/basket/basket.service';
import { CheckoutService } from '../checkout.service';
import { IBasket } from 'src/app/shared/models/basket';
import { Address } from 'src/app/shared/models/user';
import { NavigationExtras, Router } from '@angular/router';

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss']
})
export class CheckoutPaymentComponent implements OnInit {

  @Input() checkoutForm?:FormGroup;
  constructor(private basketService:BasketService, private checkoutService:CheckoutService, private router:Router) { }

  ngOnInit(): void {

  }

  submitOrder(){
    const basket = this.basketService.getCurrentBasketValue();
    if(!basket) return;

    const orderToCreate = this.orderToCreate(basket)
    if(!orderToCreate) return;
    this.checkoutService.createOrder(orderToCreate).subscribe({
      next:order =>{
        this.basketService.deleteLocalBasket();
        const navigationExras:NavigationExtras =  {state:order}
        this.router.navigate(['checkout/success'],navigationExras)
        console.log(order)
      }
    })
  }

  private orderToCreate(basket: IBasket) {
    const deliveryMethodId = this.checkoutForm?.get('deliveryForm')?.get('deliveryMethod')?.value;
    const shipToAddress = this.checkoutForm?.get('addressForm')?.value as Address;
    if(!deliveryMethodId || !shipToAddress) return;

    return {
      basketId : basket.id,
      deliveryMethodId : deliveryMethodId,
      shipToAddress : shipToAddress
    }
  }

}
