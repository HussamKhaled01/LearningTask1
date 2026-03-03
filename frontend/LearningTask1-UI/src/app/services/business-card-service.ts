import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { Observable, tap } from 'rxjs';



export interface PaginatedResponse<T> {
  data: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

export interface BusinessCard {
  id: number;
  name: string;
  gender: string;
  dob: string;
  email: string;
  phoneNumber: string;
  address: string;
  imageUrl?: string;
}

export interface CardFilters {
  searchTerm: string;
  gender: string;
  dobFrom: string;
  dobTo: string;
  pageNumber: number;
  pageSize: number;
}

@Injectable({
  providedIn: 'root'
})
export class BusinessCardService {
  private apiUrl = 'https://localhost:7193/api/BusinessCard';

  private cardsSignal = signal<BusinessCard[]>([]);
  cards = this.cardsSignal.asReadonly();

  private paginationSignal = signal<Omit<PaginatedResponse<BusinessCard>, 'data'>>({
    pageNumber: 1,
    pageSize: 10,
    totalCount: 0,
    totalPages: 0
  });
  pagination = this.paginationSignal.asReadonly();

  private filterSignal = signal<CardFilters>({
    searchTerm: '',
    gender: '',
    dobFrom: '',
    dobTo: '',
    pageNumber: 1,
    pageSize: 10
  });
  filters = this.filterSignal.asReadonly();

  constructor(private http: HttpClient) { }

  updateFilters(newFilters: Partial<CardFilters>) {
    this.filterSignal.update(current => ({ ...current, ...newFilters }));
  }

  loadCards(): Observable<PaginatedResponse<BusinessCard>> {
    const f = this.filterSignal();
    let url = `${this.apiUrl}?pageNumber=${f.pageNumber}&pageSize=${f.pageSize}`;
    if (f.searchTerm) url += `&searchTerm=${encodeURIComponent(f.searchTerm)}`;
    if (f.gender) url += `&gender=${encodeURIComponent(f.gender)}`;
    if (f.dobFrom) url += `&dobFrom=${encodeURIComponent(f.dobFrom)}`;
    if (f.dobTo) url += `&dobTo=${encodeURIComponent(f.dobTo)}`;

    return this.http.get<PaginatedResponse<BusinessCard>>(url).pipe(
      tap(response => {
        this.cardsSignal.set(response.data);
        this.paginationSignal.set({
          pageNumber: response.pageNumber,
          pageSize: response.pageSize,
          totalCount: response.totalCount,
          totalPages: response.totalPages
        });
      })
    );
  }


  getById(id: number): Observable<BusinessCard> {
    return this.http.get<BusinessCard>(`${this.apiUrl}/${id}`);
  }

  create(card: BusinessCard): Observable<BusinessCard> {
    return this.http.post<BusinessCard>(this.apiUrl, card);
  }

  createWithFormData(formData: FormData): Observable<BusinessCard> {
    return this.http.post<BusinessCard>(this.apiUrl, formData);
  }

  update(id: number, card: BusinessCard): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, card);
  }

  updateWithFormData(id: number, formData: FormData): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, formData);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  exportData(format: 'csv' | 'xml'): Observable<Blob> {
    const f = this.filterSignal();
    let url = `${this.apiUrl}/export?format=${format}&pageNumber=1&pageSize=1000000`;
    if (f.searchTerm) url += `&searchTerm=${encodeURIComponent(f.searchTerm)}`;
    if (f.gender) url += `&gender=${encodeURIComponent(f.gender)}`;
    if (f.dobFrom) url += `&dobFrom=${encodeURIComponent(f.dobFrom)}`;
    if (f.dobTo) url += `&dobTo=${encodeURIComponent(f.dobTo)}`;

    return this.http.get(url, { responseType: 'blob' });
  }

  importData(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file, file.name);
    return this.http.post(`${this.apiUrl}/import`, formData);
  }
}