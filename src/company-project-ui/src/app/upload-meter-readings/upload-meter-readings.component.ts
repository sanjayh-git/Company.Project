import { Component } from '@angular/core';
import { ApiService, UploadResult } from '../services/api.service';

@Component({
  selector: 'app-upload-meter-readings',
  templateUrl: './upload-meter-readings.component.html',
  styleUrls: ['./upload-meter-readings.component.scss']
})
export class UploadMeterReadingsComponent {
  selectedFile: File | null = null;
  uploading = false;
  result: UploadResult | null = null;
  error: string | null = null;

  constructor(private api: ApiService) {}

  onFileChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
      this.result = null;
      this.error = null;
    }
  }

  onUpload(): void {
    if (!this.selectedFile) {
      this.error = 'Please select a CSV file first.';
      return;
    }

    this.uploading = true;
    this.error = null;
    this.result = null;

    this.api.uploadMeterReadings(this.selectedFile).subscribe({
      next: res => {
        this.result = res;
        this.uploading = false;
      },
      error: err => {
        console.error(err);
        this.error = 'Upload failed.';
        this.uploading = false;
      }
    });
  }
}
