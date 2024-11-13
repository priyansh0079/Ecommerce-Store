import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from "./layout/header/header.component";
import { HttpClient } from '@angular/common/http';
import { Product } from './shared/models/product';
import { Pagination } from './shared/models/pagination';
import { ApiBaseResponse } from './shared/models/apiBaseResponse';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit{

  baseUrl = 'http://localhost:5183/api/'
  private http = inject(HttpClient);
  title = 'E-Commerce';

  products: Product[] = [];

  ngOnInit(): void {
    this.http.get<ApiBaseResponse<Pagination<Product>>>(this.baseUrl + 'product/list').subscribe({
      next: response => this.products = response.content.data,
      error: error => console.error(error),
      complete: () => console.log('Request completed')
    })
  }

}