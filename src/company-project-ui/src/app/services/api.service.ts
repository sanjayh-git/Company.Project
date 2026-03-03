import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Account } from '../models/account';
import { MeterReading } from '../models/meter-reading';

export interface UploadResult {
  successCount: number;
  failureCount: number;
}

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private readonly baseUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  getAccounts(): Observable<Account[]> {
    return this.http.get<Account[]>(`${this.baseUrl}/accounts`);
  }

  getMeterReadings(): Observable<MeterReading[]> {
    return this.http.get<MeterReading[]>(`${this.baseUrl}/meter-readings`);
  }

  uploadMeterReadings(file: File): Observable<UploadResult> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<UploadResult>(
      `${this.baseUrl}/meter-reading-uploads`,
      formData
    );
  }
}
