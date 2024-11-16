import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { ApiBaseResponse } from '../../shared/models/apiBaseResponse';
import { Pagination } from '../../shared/models/pagination';
import { Product } from '../../shared/models/product';
import { ShopParams } from '../../shared/models/shopParams';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = 'http://localhost:5183/api/'
  private http = inject(HttpClient);

  types: string[] = [];
  brands: string[] = [];

  getProducts(shopParams: ShopParams){
    let params = new HttpParams();

    if (shopParams.brands.length > 0){
      params = params.append('brands', shopParams.brands.join(','));
    }

    if (shopParams.types.length > 0){
      params = params.append('brands', shopParams.types.join(','));
    }

    if (shopParams.sort){
      params = params.append('sort', shopParams.sort);
    }

    if (shopParams.search){
      params = params.append('search', shopParams.search);
    }

    params = params.append('pageSize', shopParams.pageSize);
    params = params.append('pageIndex', shopParams.pageNumber);

    return this.http.get<ApiBaseResponse<Pagination<Product>>>(this.baseUrl + 'product/list', {params});
  }

  getBrands(){
    if (this.brands.length > 0) return;
    return this.http.get<ApiBaseResponse<string[]>>(this.baseUrl + 'product/brands').subscribe({
      next: response => this.brands = response.content
    });
  }

  getTypes(){
    if (this.types.length > 0) return;
    return this.http.get<ApiBaseResponse<string[]>>(this.baseUrl + 'product/types').subscribe({
      next: response => this.types = response.content
    });
  }
}
