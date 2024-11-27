import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { TestErrorComponent } from './core/test-error/test-error.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { AuthGuard } from './core/guards/auth.guard';



const routes: Routes = [
  {path:'',component:HomeComponent},
  {path:'shop',loadChildren:()=> import('./shope/shope.module').then(mod => mod.ShopeModule)},
  {path:'basket',loadChildren:()=> import('./basket/basket.module').then(mod => mod.BasketModule)},
  {
    path:'checkout',
    canActivate : [AuthGuard],
    loadChildren:()=> import('./checkout/checkout.module').then(mod => mod.CheckoutModule)
  },
  {path:'account',loadChildren:()=> import('./account/account.module').then(mod => mod.AccountModule)},
  {path:'test-error',component:TestErrorComponent},
  {path:'server-error',component:ServerErrorComponent},
  {path:'not-found',component:NotFoundComponent},
  {path:'**',redirectTo:'',pathMatch:'full'},
  

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
