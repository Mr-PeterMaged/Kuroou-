import { Component } from '@angular/core';
import { BasketService } from './basket/basket.service';
import { IProduct } from './shared/models/product';
import { AccountService } from './account/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  products: IProduct[]
  constructor(private basketService:BasketService, private accountService:AccountService){}

  ngOnInit(): void {
    this.loadBasket();
    this.loadCurrentUser();
  }

  loadBasket(){
    const basketId = localStorage.getItem('basket_id')
    if(basketId){
      this.basketService.getBasket(basketId).subscribe(() =>{
        console.log('intial')
      }, err =>{
        console.log(err)
      })
    }
  }



  loadCurrentUser(){
    const token = localStorage.getItem('token')
    console.log(token)

    this.accountService.loadCurrentUser(token).subscribe(()=>{
      console.log('user')
    },error=>{
      console.log(error)
    })
    
  }
  title = 'Client';
}
