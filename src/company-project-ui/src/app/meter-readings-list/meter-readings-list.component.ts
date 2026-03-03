import { Component, OnInit } from '@angular/core';
import { ApiService } from '../services/api.service';
import { MeterReading } from '../models/meter-reading';

@Component({
  selector: 'app-meter-readings-list',
  templateUrl: './meter-readings-list.component.html',
  styleUrls: ['./meter-readings-list.component.scss']
})
export class MeterReadingsListComponent implements OnInit {
  readings: MeterReading[] = [];
  loading = false;
  error: string | null = null;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.loadReadings();
  }

  loadReadings(): void {
    this.loading = true;
    this.error = null;

    this.api.getMeterReadings().subscribe({
      next: readings => {
        this.readings = readings;
        this.loading = false;
      },
      error: err => {
        console.error(err);
        this.error = 'Failed to load meter readings';
        this.loading = false;
      }
    });
  }
}
