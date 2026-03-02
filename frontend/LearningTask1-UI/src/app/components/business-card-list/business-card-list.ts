import { Component, inject, OnInit, signal } from '@angular/core';
import { Router } from '@angular/router';
import { BusinessCard, BusinessCardService } from '../../services/business-card-service';

@Component({
  selector: 'app-business-card-list',
  standalone: true,
  imports: [],
  templateUrl: './business-card-list.html',
  styleUrls: ['./business-card-list.css']
})
export class BusinessCardList implements OnInit {

  cardsService = inject(BusinessCardService);
  router = inject(Router);

  pendingFilters = signal({
    searchTerm: '',
    gender: '',
    dobFrom: '',
    dobTo: ''
  });

  ngOnInit(): void {
    const currentFilters = this.cardsService.filters();
    this.pendingFilters.set({
      searchTerm: currentFilters.searchTerm,
      gender: currentFilters.gender,
      dobFrom: currentFilters.dobFrom,
      dobTo: currentFilters.dobTo
    });
    this.loadAll();
  }

  loadAll(): void {
    this.cardsService.loadCards().subscribe({
      next: () => {
      },
      error: (err: any) => {
        console.error('Load error:', err);
      }
    });
  }

  changePage(newPageNumber: number): void {
    const currentPagination = this.cardsService.pagination();
    if (newPageNumber >= 1 && newPageNumber <= currentPagination.totalPages) {
      this.cardsService.updateFilters({ pageNumber: newPageNumber });
      this.loadAll();
    }
  }

  onPageSizeChange(event: Event): void {
    const newSize = parseInt((event.target as HTMLSelectElement).value, 10);
    this.cardsService.updateFilters({ pageSize: newSize, pageNumber: 1 });
    this.loadAll();
  }

  onFilterInput(event: Event, filterType: 'searchTerm' | 'gender' | 'dobFrom' | 'dobTo'): void {
    const target = event.target as any;
    const value = target?.value || '';
    this.pendingFilters.update(current => ({ ...current, [filterType]: value }));
  }

  applyFilters(): void {
    this.cardsService.updateFilters({
      ...this.pendingFilters(),
      pageNumber: 1
    });
    this.loadAll();
  }

  createNew(): void {
    this.router.navigate(['/create']);
  }

  edit(id: number): void {
    this.router.navigate(['/edit', id]);
  }

  viewDetails(id: number): void {
    this.router.navigate(['/details', id]);
  }

  delete(id: number): void {
    if (!confirm('Are you sure you want to delete this business card?')) return;

    this.cardsService.delete(id).subscribe({
      next: () => {
        this.loadAll();
      },
      error: (err) => {
        console.error('Delete error:', err);
      }
    });
  }
}