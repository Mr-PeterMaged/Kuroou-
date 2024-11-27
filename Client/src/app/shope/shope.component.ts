import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { IProduct } from '../shared/models/product';
import { ShopeService } from './shope.service';
import { IBrand } from '../shared/models/brand';
import { IType } from '../shared/models/productType';
import { ShopParam } from '../shared/models/shopParams';

@Component({
  selector: 'app-shope',
  templateUrl: './shope.component.html',
  styleUrls: ['./shope.component.scss']
})
export class ShopeComponent implements OnInit {
  @ViewChild('search',{static:false}) searchTerm:ElementRef
  products:IProduct[];
  brands: IBrand[];
  types: IType[];
  sortOption = [
    {name : 'Alphabetical', value:'name'},
    {name : 'Price:Low To High', value:'priceAsc'},
    {name : 'Price:high To Low', value:'priceDesc'}
  ];
  shopParam:ShopParam;
  totalCount:number;

  constructor(private shopeService: ShopeService) 
  {
    this.shopParam = this.shopeService.getShopParams();
  }

  ngOnInit(): void {
    this.getProducts(true);
    this.getBrands();
    this.getTypes();
  }

  getProducts(useCash = false){
    this.shopeService.getProducts(useCash).subscribe(res => {
      this.products = res.data
      this.totalCount = res.count
    })
  }

  getBrands(){
    this.shopeService.getBrands().subscribe(res => {
      this.brands = [{ id:0, name:'all' }, ...res]
    })
  }

  getTypes(){
    this.shopeService.getTypes().subscribe(res => {
      this.types = [{ id:0, name:'all' }, ...res]
    })
  }
  onTypeSelected(id:number){
    const params = this.shopeService.getShopParams();
    params.typeIdselected = id
    params.pageNumber = 1
    this.shopeService.setShopParams(params);
    this.getProducts()
  }

  onBrandSelected(id:number){
    const params = this.shopeService.getShopParams();
    params.brandIdSelected = id
    params.pageNumber = 1
    this.shopeService.setShopParams(params);
    this.getProducts()
  }

  onPageChanged(event: any) {
    const params = this.shopeService.getShopParams();
    if(params.pageNumber !== event){
      params.pageNumber = event;
      this.shopeService.setShopParams(params);
      this.getProducts(true);
    }
  }
  onSearch(){
    const params = this.shopeService.getShopParams();
    params.search = this.searchTerm.nativeElement.value
    params.pageNumber = 1
    this.shopeService.setShopParams(params);
    this.getProducts()
  }
  
  onReset(){
    this.searchTerm.nativeElement.value = ''
    this.shopParam = new ShopParam();
    console.log(this.shopParam)
    this.shopeService.setShopParams(this.shopParam);
    this.getProducts();
  }

  onSortSelected(sort:string){
    const params = this.shopeService.getShopParams();
    params.sortSelected = sort;
    this.shopeService.setShopParams(params);
    this.getProducts();
  }
}
