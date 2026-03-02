import { Component, effect, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BusinessCard, BusinessCardService } from '../../services/business-card-service';

@Component({
  selector: 'app-business-card-details',
  imports: [],
  templateUrl: './business-card-details.html',
  styleUrl: './business-card-details.css',
})
export class BusinessCardDetails implements OnInit {
  cardsService = inject(BusinessCardService);
  route = inject(ActivatedRoute);
  router = inject(Router);

  cardId = signal<number | null>(null);
  card = signal<BusinessCard | null>(null);

  constructor() {
    effect(() => {
      const id = this.cardId();
      if (id) {
        this.cardsService.getById(id).subscribe({
          next: (data) => this.card.set(data),
          error: (err) => console.error('Load error:', err)
        });
      }
    });
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.cardId.set(Number(id));
    }
  }

  goBack(): void {
    this.router.navigate(['/']);
  }

  deleteCard(): void {
    const id = this.cardId();
    if (id) {
      this.cardsService.delete(id).subscribe({
        next: () => this.router.navigate(['/']),
        error: (err) => console.error('Delete error:', err)
      });
    }
  }
}
