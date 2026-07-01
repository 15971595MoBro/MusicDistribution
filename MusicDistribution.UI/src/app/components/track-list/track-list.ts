import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { TrackService } from '../../services/track.service';
import { Track } from '../../models/track.model';

@Component({
  selector: 'app-track-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  template: `
    <div class="container">
      <h1>🎵 Music Distribution - Tracks</h1>
      
      <div class="filters">
        <label>Filter by Status:</label>
        <select [ngModel]="selectedStatus" (change)="onStatusChange($any($event.target).value)">
          <option value="">All</option>
          <option value="Draft">Draft</option>
          <option value="Submitted">Submitted</option>
          <option value="Distributed">Distributed</option>
        </select>
      </div>

      <div class="tracks-grid" *ngIf="tracks.length > 0">
        <div class="track-card" *ngFor="let track of tracks" [routerLink]="['/tracks', track.id]">
          <h3>{{ track.title }}</h3>
          <p><strong>Artist:</strong> {{ track.artistName }}</p>
          <p><strong>Genre:</strong> {{ track.genre }}</p>
          <p><strong>ISRC:</strong> {{ track.isrc }}</p>
          <p><strong>Release:</strong> {{ track.releaseDate | date }}</p>
          <span class="status" [class]="'status-' + track.status.toLowerCase()">
            {{ track.status }}
          </span>
        </div>
      </div>

      <div *ngIf="tracks.length === 0" class="no-data">
        No tracks found.
      </div>
    </div>
  `,
  styles: [`
    .container { padding: 20px; max-width: 1200px; margin: 0 auto; }
    h1 { color: #333; margin-bottom: 20px; }
    .filters { margin-bottom: 20px; }
    .filters label { margin-right: 10px; }
    .tracks-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(300px, 1fr)); gap: 20px; }
    .track-card { background: #f8f9fa; border-radius: 8px; padding: 20px; cursor: pointer; transition: transform 0.2s; }
    .track-card:hover { transform: translateY(-5px); box-shadow: 0 4px 12px rgba(0,0,0,0.1); }
    .track-card h3 { margin-top: 0; color: #2c3e50; }
    .status { display: inline-block; padding: 4px 12px; border-radius: 12px; font-size: 12px; font-weight: bold; margin-top: 10px; }
    .status-draft { background: #fff3cd; color: #856404; }
    .status-submitted { background: #d1ecf1; color: #0c5460; }
    .status-distributed { background: #d4edda; color: #155724; }
    .no-data { text-align: center; padding: 40px; color: #6c757d; font-size: 18px; }
  `]
})
export class TrackListComponent implements OnInit {
  tracks: Track[] = [];
  selectedStatus: string = '';

  constructor(private trackService: TrackService,private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    this.loadTracks();
  }

  loadTracks() {
    this.trackService.getTracks().subscribe({
      next: (data) => {
        this.tracks = data;
      },
      error: (err) => console.error('Error loading tracks:', err)
    });
  }

  onStatusChange(status: string) {
    this.selectedStatus = status;
    
    if (!status || status === '') {
      this.loadTracks();
    } else {
      this.trackService.getTracks(undefined, undefined, status).subscribe({
        next: (data) => {
          this.tracks = data;
          this.cdr.detectChanges();  // ← force update
        },
        error: (err) => console.error('API Error:', err)
      });
    }
  }
}