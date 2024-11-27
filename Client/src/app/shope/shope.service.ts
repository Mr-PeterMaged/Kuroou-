import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IProduct } from '../shared/models/product';
import { Observable, of, pipe } from 'rxjs';
import { IBrand } from '../shared/models/brand';
import { IPagination, Pagination } from '../shared/models/pagination';
import { ShopParam } from '../shared/models/shopParams';
import { map } from 'rxjs/operators';
import { IType } from '../shared/models/productType';

@Injectable({
  providedIn: 'root'
})
export class ShopeService {

  baseUrl = 'https://localhost:7261/api/Product';
  products:IProduct[] = [];
  brands:IBrand[] = [];
  types:IType[] = [];
  pagination = new Pagination();
  shopParams = new ShopParam();
  constructor(private httpClient: HttpClient) { }

  getProducts(useCach:boolean) : Observable<IPagination>{
    console.log(this.shopParams)

    if(useCach === false){
      this.products = [];
    }

    if(this.products.length > 0 && useCach == true){
      const pagesReceived = Math.ceil(this.products.length / this.shopParams.pageSize)
      if(this.shopParams.pageNumber <= pagesReceived){
        this.pagination.data = this.products.slice((this.shopParams.pageNumber - 1) * this.shopParams.pageSize,
      this.shopParams.pageNumber * this.shopParams.pageSize)

      return of(this.pagination)
      }
    }

    let params = new HttpParams();

    if(this.shopParams.brandIdSelected){
      params = params.append('brandId',this.shopParams.brandIdSelected.toString());
    }

    if(this.shopParams.typeIdselected){
      params = params.append('typeId',this.shopParams.typeIdselected.toString());
    }

    if(this.shopParams.pageNumber){
      params = params.append('pageIndex',this.shopParams.pageNumber.toString())
    }

    if(this.shopParams.pageSize){
      params = params.append('pageSize',this.shopParams.pageSize.toString())
    }

    if(this.shopParams.search){
      params = params.append('search',this.shopParams.search)
    }

    if(this.shopParams.sortSelected){
      params = params.append('sort',this.shopParams.sortSelected)
    }

    return this.httpClient.get<IPagination>(this.baseUrl,{observe:'response',params}).pipe(
      map(response =>{
        this.products = [...this.products, ...response.body.data];
        this.pagination = response.body;
        return this.pagination;
      })
    );
  }

  getBrands() : Observable<IBrand[]>{
    if(this.brands.length > 0)
      return of(this.brands);
    return this.httpClient.get<IBrand[]>(this.baseUrl + '/get-brands').pipe(
      map(reponse =>{
        this.types = reponse;
        return reponse
      })
    );
  }

  setShopParams(params : ShopParam){
    this.shopParams = params;
  }

  getShopParams(){
    return this.shopParams;
  }

  getTypes() : Observable<IType[]>{
    if(this.types.length > 0)
      return of(this.types);
    return this.httpClient.get<IType[]>(this.baseUrl + '/get-types').pipe(
      map(reponse =>{
        this.types = reponse;
        return reponse
      })
    );
  }
  getProduct(id:number): Observable<IProduct>{
    const product = this.products.find(p => p.id == id);
    if(product)
      return of(product);

    return this.httpClient.get<IProduct>(this.baseUrl + '/'+id)
  }
}
