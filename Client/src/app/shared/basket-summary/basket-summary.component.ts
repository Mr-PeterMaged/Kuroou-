import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IBasketItem } from '../models/basket'
import { BasketService } from 'src/app/basket/basket.service';

@Component({
  selector: 'app-basket-summary',
  templateUrl: './basket-summary.component.html',
  styleUrls: ['./basket-summary.component.scss']
})
export class BasketSummaryComponent implements OnInit {
  @Output() addItem = new EventEmitter<IBasketItem>();
  @Output() removeItem = new EventEmitter<IBasketItem>();
  @Output() decrementBasket = new EventEmitter<IBasketItem>();
  @Input() isBasket = true;
  constructor(public BasketService:BasketService) { }

  ngOnInit(): void {
  }

  addBasketItem(item:IBasketItem){
    this.addItem.emit(item)
  }

  decrementBasketItem(item:IBasketItem){
    this.decrementBasket.emit(item)
  }

  removeBasketItem(item:IBasketItem){
    this.removeItem.emit(item)
  }

}
