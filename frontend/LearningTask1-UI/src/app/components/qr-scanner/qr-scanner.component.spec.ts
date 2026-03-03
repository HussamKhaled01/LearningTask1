import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CommonModule } from '@angular/common';
import { QrScannerComponent } from './qr-scanner.component';
import { ZXingScannerModule } from '@zxing/ngx-scanner';

describe('QrScannerComponent', () => {
    let component: QrScannerComponent;
    let fixture: ComponentFixture<QrScannerComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [QrScannerComponent, CommonModule, ZXingScannerModule]
        })
            .compileComponents();

        fixture = TestBed.createComponent(QrScannerComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    it('should emit scanSuccess event when onScanSuccess is called', () => {
        let emitted = false;
        component.scanSuccess.subscribe((res: any) => {
            if (res === 'test') emitted = true;
        });

        component.onScanSuccess('test');

        expect(emitted).toBe(true);
    });

    it('should emit scanCancel event when stopScanning is called', () => {
        let emitted = false;
        component.scanCancel.subscribe(() => {
            emitted = true;
        });
        component.isScanning = true;

        component.stopScanning();

        expect(component.isScanning).toBe(false);
        expect(emitted).toBe(true);
    });

    it('should update hasCameras when onCamerasFound is called', () => {
        const mockCameras: MediaDeviceInfo[] = [
            { deviceId: '1', kind: 'videoinput', label: 'Camera 1', toJSON: () => { } } as MediaDeviceInfo
        ];

        component.onCamerasFound(mockCameras);
        expect(component.hasCameras).toBe(true);

        component.onCamerasFound([]);
        expect(component.hasCameras).toBe(false);
    });

    it('should update hasPermission when onHasPermission is called', () => {
        component.onHasPermission(true);
        expect(component.hasPermission).toBe(true);

        component.onHasPermission(false);
        expect(component.hasPermission).toBe(false);
    });
});
