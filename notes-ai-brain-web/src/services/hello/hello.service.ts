import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root',
})
export class HelloService {
  private http = inject(HttpClient);
  constructor() {}

  getHello() {
    return this.http.get('https://localhost:7112/api/hello', {
      responseType: 'text',
    });
  }
}
