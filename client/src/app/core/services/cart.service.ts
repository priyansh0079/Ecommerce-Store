import { computed, inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Cart, CartItem } from '../../shared/models/cart';
import { Product } from '../../shared/models/product';
import { ApiBaseResponse } from '../../shared/models/apiBaseResponse';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  cart = signal<Cart | null>(null);
  itemCount = computed(() => {
    return this.cart()?.items.reduce((sub, item) => sub + item.quantity, 0)
  });

  totals = computed(() => {
    const cart = this.cart();
    if (!cart){
      return null;
    }
    const subtotal = cart.items.reduce((sum, item) => sum + item.price * item.quantity, 0);
    const shipping = 0;
    const discount = 0;
    return  {
      subtotal,
      shipping,
      discount,
      total: subtotal + shipping - discount
    }
  })

  getCart(id: string){
    return this.http.get<ApiBaseResponse<Cart>>(this.baseUrl + 'cart/get?id=' + id).pipe(
      map(response => {
        this.cart.set(response.content);
        return response.content;
      })
    )
  }

  setCart(cart: Cart){
    return this.http.post<ApiBaseResponse<Cart>>(this.baseUrl + 'cart/set', cart).subscribe({
      next: response => this.cart.set(response.content)
    })
  }

  addItemToCart(item: CartItem | Product, quantity = 1){
    const cart = this.cart() ?? this.createCart();
    if (this.isProduct(item)){
      item = this.mapProductToCartItem(item);
    }
    cart!.items = this.addOrUpdateItem(cart!.items, item, quantity);
    this.setCart(cart!);
  }

  removeItemFromCart(productId: number, quantity = 1){
    const cart = this.cart();
    if (!cart) return;
    const index = cart.items.findIndex(x => x.productId === productId);
    if (index !== -1){
      if (cart.items[index].quantity > quantity){
        cart.items[index].quantity -= quantity;
      }
      else{
        cart.items.splice(index, 1);
      }
      if (cart.items.length === 0){
        this.deleteCart();
      }
      else{
        this.setCart(cart);
      }
    }
  }

  deleteCart(){
    this.http.delete(this.baseUrl + 'cart/delete?id=' + this.cart()?.id).subscribe({
      next: () => {
        localStorage.removeItem('cart_id');
        this.cart.set(null);
      }
    })
  }

  addOrUpdateItem(items: CartItem[], item: CartItem, quantity: number): CartItem[] {
    const index = items.findIndex(i => i.productId === item.productId);
    if (index === -1){
      item.quantity = quantity;
      items.push(item);
    }
    else{
      items[index].quantity += quantity;
    }
    return items;
  }

  private mapProductToCartItem(item: Product): CartItem {
    return {
      productId: item.id,
      productName: item.name,
      price: item.price,
      quantity: 0,
      pictureUrl: item.pictureUrl,
      brand: item.brand,
      type: item.type
    }
  }

  private isProduct(item: CartItem | Product): item is Product {
    return (item as Product).id !== undefined;
  }

  createCart(): Cart | null {
    const cart = new Cart();
    localStorage.setItem('cart_id', cart.id);
    return cart;
  }
}
