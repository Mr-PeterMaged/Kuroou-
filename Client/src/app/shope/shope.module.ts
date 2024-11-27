import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShopeComponent } from './shope.component';
import { ProductItemComponent } from './product-item/product-item.component';
import { SharedModule } from '../shared/shared.module';
import { ProductDetailsComponent } from './product-details/product-details.component';
import { ShopeRoutingModule } from './shope-routing.module';



@NgModule({
  declarations: [ShopeComponent, ProductItemComponent, ProductDetailsComponent],
  imports: [
    CommonModule,
    SharedModule,
    ShopeRoutingModule
  ],
})
export class ShopeModule { }
