import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { DeliveryMethod } from '../shared/models/deliveryMethod';
import { map } from 'rxjs/operators';
import { Order, OrderToCreate } from '../shared/models/order';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  baseUrl = environment.appUrl; 

  constructor(private httpClient:HttpClient) { }

  createOrder(order:OrderToCreate){
    return this.httpClient.post<Order>(this.baseUrl+ 'order', order)
  }

  getDeliveryMethods(){
    return this.httpClient.get<DeliveryMethod[]>(this.baseUrl + 'order/delivery-methods').pipe(
      map(dm=>{
        return dm.sort((a,b)=>b.price - a.price)
      })
    )
  }
}
