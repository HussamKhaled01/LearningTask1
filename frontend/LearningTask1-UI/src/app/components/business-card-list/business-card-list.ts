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

 businessCardsSignal = signal<BusinessCard[]>([]);

  ngOnInit(): void {
    this.loadAll();
  }

  loadAll(): void {
    this.cardsService.getAll().subscribe({
      next: (data) => {
        this.businessCardsSignal.set(data); 
      },
      error: (err) => {
        console.error('Load error:', err);
      }
    });
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
        this.businessCardsSignal.update(cards => cards.filter(c => c.id !== id));
        this.ngOnInit();
      },
      error: (err) => {
        console.error('Delete error:', err);
      }
    });
  }
}