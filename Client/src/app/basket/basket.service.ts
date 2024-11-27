import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Basket, IBasket, IBasketItem, IBasketTotals } from '../shared/models/basket';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { IProduct } from '../shared/models/product';
import { DeliveryMethod } from '../shared/models/deliveryMethod';

@Injectable({
  providedIn: 'root'
})
export class BasketService {

  baseUrl = environment.appUrl;
  private basketSource = new BehaviorSubject<IBasket>(null);
  basket$ = this.basketSource.asObservable();
  private basketTotalSource = new BehaviorSubject<IBasketTotals>(null)
  basketTotal$ = this.basketTotalSource.asObservable();
  shipping = 0;

  constructor(private httpClient:HttpClient) { }

  setShippingPrice(deliveryMethod:DeliveryMethod){
    this.shipping = deliveryMethod.price;
    this.calculateTotals();
  }

  getBasket(id:string){
    return this.httpClient.get(this.baseUrl + 'basket?id='+id).pipe(

      map((basket:IBasket)=>{
        this.basketSource.next(basket)
        this.calculateTotals()
      })
    )
  }

  getCurrentBasketValue(){
    return this.basketSource.value
  }

  addItemToBasket(item:IProduct, quantity = 1){

    const itemToAdd : IBasketItem = this.mapProductItemToBasketItem(item, quantity);
    let baseket = this.getCurrentBasketValue();
    console.log(baseket)
    if(baseket === null){
      
      baseket = this.createBasket();
      console.log(baseket)
    }
    baseket.items = this.addOrUpdateItem(baseket.items, itemToAdd, quantity)
    this.setBasket(baseket);
  }

  setBasket(basket:IBasket){
    return this.httpClient.post(this.baseUrl + 'basket', basket).subscribe((response:IBasket) =>{
      this.basketSource.next(response)
      this.calculateTotals()
    }, error =>{
      console.log(error)
    })
  }


  private createBasket(): IBasket {
    const basket =  new Basket()
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }

  mapProductItemToBasketItem(item: IProduct, quantity: number): IBasketItem {
    console.log(item,'74')
    return {
      id: item.id,
      productName: item.name,
      brand:item.productBrand,
      type:item.productType,
      pictureUrl: item.pictureUrl,
      price: item.price,
      quantity: quantity
    }
  }

  incrementItemQuantity(item : IBasketItem){
    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id);
    basket.items[foundItemIndex].quantity++;
    this.setBasket(basket)
  }

  decrementItemQuantity(item : IBasketItem){
    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id);
    if(basket.items[foundItemIndex].quantity > 1){
      basket.items[foundItemIndex].quantity--;
      this.setBasket(basket);
    }
    else{
      this.removeItemFromBasket(item);
    }
    
  }
  removeItemFromBasket(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();
    if(basket.items.some(x => x.id === item.id)){
      basket.items = basket.items.filter(i => i.id !== item.id)
      if(basket.items.length>0){
        this.setBasket(basket);
      }else{
        this.deleteBasket(basket);
      }
    }
  }
  deleteBasket(basket: IBasket) {
    return this.httpClient.delete(this.baseUrl + 'basket?id=' + basket.id).subscribe(()=>{
      this.deleteLocalBasket();
    })
  }

  deleteLocalBasket(){
    this.basketSource.next(null);
    this.basketTotalSource.next(null);
    localStorage.removeItem('basket_id');
  }

  private calculateTotals(){
    const baseket = this.getCurrentBasketValue();
    const subtotal = baseket.items.reduce((a,b)=>(b.price * b.quantity) + a, 0);
    const total = subtotal + this.shipping;
    this.basketTotalSource.next({shipping:this.shipping, total, subtotal});
  }

  private addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    const index = items.findIndex(i => i.id === itemToAdd.id)

    if(index === -1){
      itemToAdd.quantity = quantity
      items.push(itemToAdd) 
    }
    else{
      items[index].quantity += quantity
    }
    return items;
  }
}
