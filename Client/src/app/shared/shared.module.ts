import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PagingHeaderComponent } from './components/paging-header/paging-header.component';
import { PagerComponent } from './components/pager/pager.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { OrderTotalsComponent } from './components/order-totals/order-totals.component';
import {CdkStepperModule} from '@angular/cdk/stepper';
import { StepperComponent } from './components/stepper/stepper.component';
import { BasketSummaryComponent } from './basket-summary/basket-summary.component'
import { RouterModule } from '@angular/router';


@NgModule({
  declarations: [PagingHeaderComponent, PagerComponent, OrderTotalsComponent, StepperComponent, BasketSummaryComponent],
  imports: [
    CommonModule,
    PaginationModule.forRoot(),
    ReactiveFormsModule,
    FormsModule,
    FormsModule,
    CdkStepperModule,
    RouterModule
  ],
  exports:[PagingHeaderComponent, PagerComponent, OrderTotalsComponent,ReactiveFormsModule,CdkStepperModule,StepperComponent,BasketSummaryComponent,FormsModule]
})
export class SharedModule { }
