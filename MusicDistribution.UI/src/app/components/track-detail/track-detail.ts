import { Component, OnInit, ChangeDetectorRef } from '@angular/core';  // ← أضف ChangeDetectorRef
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { TrackService } from '../../services/track.service';
import { Track } from '../../models/track.model';

@Component({
  selector: 'app-track-detail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="container" *ngIf="track">
      <a routerLink="/tracks" class="back-link">← Back to Tracks</a>
      
      <h1>{{ track.title }}</h1>
      
      <div class="detail-card">
        <div class="info-section">
          <h3>Track Information</h3>
          <p><strong>Artist:</strong> {{ track.artistName }}</p>
          <p><strong>Genre:</strong> {{ track.genre }}</p>
          <p><strong>ISRC:</strong> {{ track.isrc }}</p>
          <p><strong>Release Date:</strong> {{ track.releaseDate | date }}</p>
          <p><strong>Status:</strong> 
            <span class="status" [class]="'status-' + track.status.toLowerCase()">
              {{ track.status }}
            </span>
          </p>
        </div>

        <div class="distribution-section">
          <h3>DSP Distribution</h3>
          <div *ngIf="track.distributions && track.distributions.length > 0">
            <div class="dsp-item" *ngFor="let dist of track.distributions">
              <span class="dsp-name">{{ dist.dspName }}</span>
              <span class="dsp-status" [class]="'dsp-status-' + dist.status.toLowerCase()">
                {{ dist.status }}
              </span>
              <span class="dsp-date">{{ dist.submittedAt | date:'medium' }}</span>
            </div>
          </div>
          <div *ngIf="!track.distributions || track.distributions.length === 0" class="no-distribution">
            Not distributed to any DSP yet.
          </div>
        </div>
      </div>
    </div>

    <div *ngIf="!track && !error" class="loading">
      Loading...
    </div>

    <div *ngIf="error" class="error">
      {{ error }}
    </div>
  `,
  styles: [`
    .container { padding: 20px; max-width: 800px; margin: 0 auto; }
    .back-link { display: inline-block; margin-bottom: 20px; color: #007bff; text-decoration: none; font-size: 16px; }
    .back-link:hover { text-decoration: underline; }
    h1 { color: #333; margin-bottom: 20px; }
    .detail-card { background: #f8f9fa; border-radius: 8px; padding: 30px; }
    .info-section, .distribution-section { margin-bottom: 30px; }
    .info-section h3, .distribution-section h3 { color: #2c3e50; border-bottom: 2px solid #3498db; padding-bottom: 10px; margin-bottom: 20px; }
    .info-section p { margin: 10px 0; font-size: 16px; }
    .status { padding: 4px 12px; border-radius: 12px; font-size: 12px; font-weight: bold; margin-left: 10px; }
    .status-draft { background: #fff3cd; color: #856404; }
    .status-submitted { background: #d1ecf1; color: #0c5460; }
    .status-distributed { background: #d4edda; color: #155724; }
    .dsp-item { display: flex; justify-content: space-between; align-items: center; padding: 15px; background: white; border-radius: 6px; margin-bottom: 10px; box-shadow: 0 1px 3px rgba(0,0,0,0.1); }
    .dsp-name { font-weight: bold; color: #2c3e50; font-size: 16px; }
    .dsp-status { padding: 4px 12px; border-radius: 12px; font-size: 12px; font-weight: bold; }
    .dsp-status-pending { background: #fff3cd; color: #856404; }
    .dsp-status-live { background: #d4edda; color: #155724; }
    .dsp-status-rejected { background: #f8d7da; color: #721c24; }
    .dsp-date { color: #6c757d; font-size: 12px; }
    .no-distribution { color: #6c757d; font-style: italic; padding: 20px; text-align: center; }
    .loading { text-align: center; padding: 50px; font-size: 18px; color: #6c757d; }
    .error { text-align: center; padding: 50px; font-size: 18px; color: #dc3545; }
  `]
})
export class TrackDetailComponent implements OnInit {
  track: Track | null = null;
  error: string = '';

  constructor(
    private route: ActivatedRoute,
    private trackService: TrackService,
    private cdr: ChangeDetectorRef  // ← أضف ده
  ) {}

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    console.log('Track ID from URL:', id);
    
    if (id) {
      console.log('Fetching track with ID:', id);
      
      this.trackService.getTrack(id).subscribe({
        next: (data) => {
          console.log('Track data received:', data);
          this.track = data;
          console.log('Track assigned:', this.track);
          this.cdr.detectChanges();  // ← FORCE UPDATE HERE
        },
        error: (err) => {
          console.error('Error loading track:', err);
          this.error = 'Failed to load track details.';
        }
      });
    } else {
      this.error = 'No track ID provided.';
    }
  }
}