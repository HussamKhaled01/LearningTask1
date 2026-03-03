import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { BusinessCardService } from '../../services/business-card-service';
import { CommonModule } from '@angular/common';
import { QrScannerComponent } from '../qr-scanner/qr-scanner.component';
import { parseVCard } from '../../utils/vcard-parser';

@Component({
  selector: 'app-business-card-form',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, QrScannerComponent],
  templateUrl: './business-card-form.html',
  styleUrl: './business-card-form.css',
})
export class BusinessCardForm implements OnInit {
  cardForm!: FormGroup;
  selectedFile: File | null = null;
  serverErrors: string[] = [];
  isSubmitting = false;

  isEditMode = false;
  editId: number | null = null;
  existingImageUrl: string | null = null;
  previewUrl: string | null = null;

  isConfirming = false;
  showScanner = false;

  openScanner(): void {
    this.showScanner = true;
  }

  closeScanner(): void {
    this.showScanner = false;
  }

  onScanSuccess(vCardString: string): void {
    this.showScanner = false;
    const data = parseVCard(vCardString);
    if (data) {
      if (data.name) this.cardForm.get('name')?.setValue(data.name);
      if (data.gender) this.cardForm.get('gender')?.setValue(data.gender);
      if (data.dob) this.cardForm.get('dob')?.setValue(data.dob);
      if (data.email) this.cardForm.get('email')?.setValue(data.email);
      if (data.phoneNumber) this.cardForm.get('phoneNumber')?.setValue(data.phoneNumber);
      if (data.address) this.cardForm.get('address')?.setValue(data.address);

      // Mark as touched so validation styles apply immediately
      this.cardForm.markAllAsTouched();
    } else {
      alert("Invalid QR Code format. Please scan a valid vCard.");
    }
  }

  private fb = inject(FormBuilder);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private cardsService = inject(BusinessCardService);

  ngOnInit(): void {
    this.initForm();

    this.route.paramMap.subscribe(params => {
      const idParam = params.get('id');
      if (idParam) {
        this.isEditMode = true;
        this.editId = +idParam;
        this.loadExistingCard(this.editId);
      }
    });
  }

  private initForm(): void {
    this.cardForm = this.fb.group({
      name: ['', Validators.required],
      gender: [''],
      dob: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required],
      address: ['']
    });
  }

  private loadExistingCard(id: number): void {
    this.cardsService.getById(id).subscribe({
      next: (card) => {
        this.cardForm.patchValue({
          name: card.name,
          gender: card.gender || '',
          dob: card.dob,
          email: card.email,
          phoneNumber: card.phoneNumber,
          address: card.address || ''
        });
        this.existingImageUrl = card.imageUrl || null;
      },
      error: (err) => {
        this.handleServerErrors(err);
      }
    });
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;

      const reader = new FileReader();
      reader.onload = (e) => {
        this.previewUrl = e.target?.result as string;
      };
      reader.readAsDataURL(file);
    }
  }

  onSubmit(): void {
    this.serverErrors = [];

    if (this.cardForm.invalid) {
      this.cardForm.markAllAsTouched();
      return;
    }

    this.isConfirming = true;
  }

  editCurrent(): void {
    this.isConfirming = false;
  }

  confirmSave(): void {
    this.isSubmitting = true;
    const formData = new FormData();

    if (this.isEditMode && this.editId) {
      formData.append('id', this.editId.toString());
    }

    Object.keys(this.cardForm.value).forEach(key => {
      const val = this.cardForm.value[key];
      if (val) {
        formData.append(key, val);
      }
    });

    if (this.selectedFile) {
      formData.append('file', this.selectedFile);
    }

    if (this.isEditMode && this.editId) {
      this.cardsService.updateWithFormData(this.editId, formData).subscribe({
        next: () => {
          this.isSubmitting = false;
          this.router.navigate(['/']);
        },
        error: (err) => {
          this.isSubmitting = false;
          this.isConfirming = false; // Go back to form on error so user can see/edit
          this.handleServerErrors(err);
        }
      });
    } else {
      this.cardsService.createWithFormData(formData).subscribe({
        next: () => {
          this.isSubmitting = false;
          this.router.navigate(['/']);
        },
        error: (err) => {
          this.isSubmitting = false;
          this.isConfirming = false; // Go back to form on error
          this.handleServerErrors(err);
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/']);
  }

  private handleServerErrors(error: any): void {
    if (error.status === 400 && error.error) {
      if (typeof error.error === 'string') {
        this.serverErrors.push(error.error);
        return;
      }

      if (error.error.errors) {
        const validationErrors = error.error.errors;
        for (const key in validationErrors) {
          if (validationErrors.hasOwnProperty(key)) {
            this.serverErrors.push(...validationErrors[key]);
          }
        }
      } else {
        this.serverErrors.push('A validation error occurred on the server.');
      }
    } else {
      this.serverErrors.push('An unexpected error occurred communicating with the server.');
    }
  }
}
