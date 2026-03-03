import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ZXingScannerModule } from '@zxing/ngx-scanner';
import { BarcodeFormat } from '@zxing/library';

@Component({
    selector: 'app-qr-scanner',
    standalone: true,
    imports: [CommonModule, ZXingScannerModule],
    templateUrl: './qr-scanner.component.html',
    styleUrls: ['./qr-scanner.component.css']
})
export class QrScannerComponent {
    @Output() scanSuccess = new EventEmitter<string>();
    @Output() scanCancel = new EventEmitter<void>();

    allowedFormats = [BarcodeFormat.QR_CODE];
    hasPermission: boolean | undefined = undefined;
    hasCameras = false;
    isScanning = false;

    onCamerasFound(cameras: MediaDeviceInfo[]): void {
        this.hasCameras = cameras && cameras.length > 0;
    }

    onHasPermission(has: boolean): void {
        this.hasPermission = has;
    }

    onScanSuccess(result: string): void {
        this.scanSuccess.emit(result);
    }

    onScanError(error: Error): void {
        // Ignore frequent scan errors when no QR is yet in frame
    }

    startScanning(): void {
        this.isScanning = true;
    }

    stopScanning(): void {
        this.isScanning = false;
        this.scanCancel.emit();
    }
}
