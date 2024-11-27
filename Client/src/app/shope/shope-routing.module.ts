import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ShopeComponent } from './shope.component';
import { ProductDetailsComponent } from './product-details/product-details.component';


var routes:Routes = [
  {path:'',component:ShopeComponent},
  {path:':id',component:ProductDetailsComponent},
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports:[RouterModule]
})
export class ShopeRoutingModule { }
