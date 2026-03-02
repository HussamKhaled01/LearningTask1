import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { Observable, tap } from 'rxjs';



export interface BusinessCard {
  id: number;
  name: string;
  gender: string;
  dob: string;
  email: string;
  phoneNumber: string;
  address: string;
}

@Injectable({
  providedIn: 'root'
})
export class BusinessCardService {
  private apiUrl = 'https://localhost:7193/api/BusinessCard';

  private cardsSignal = signal<BusinessCard[]>([]);
  cards = this.cardsSignal.asReadonly();

  constructor(private http: HttpClient) { }

  getAll(): Observable<BusinessCard[]> {
    return this.http.get<BusinessCard[]>(this.apiUrl).pipe(
      tap(data => this.cardsSignal.set(data))
    );
  }


  getById(id: number): Observable<BusinessCard> {
    return this.http.get<BusinessCard>(`${this.apiUrl}/${id}`);
  }

  create(card: BusinessCard): Observable<BusinessCard> {
    return this.http.post<BusinessCard>(this.apiUrl, card);
  }

  update(id: number, card: BusinessCard): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, card);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}