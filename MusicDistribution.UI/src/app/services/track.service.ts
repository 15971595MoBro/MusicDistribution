import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Track } from '../models/track.model';

@Injectable({
  providedIn: 'root'
})
export class TrackService {
  private apiUrl = 'http://localhost:5064/api/tracks';

  constructor(private http: HttpClient) {}

  getTracks(artistId?: string, genre?: string, status?: string): Observable<Track[]> {
    let params = new HttpParams();
    if (artistId) params = params.set('artistId', artistId);
    if (genre) params = params.set('genre', genre);
    if (status) params = params.set('status', status);
    
    return this.http.get<Track[]>(this.apiUrl, { params });
  }

  getTrack(id: string): Observable<Track> {
    return this.http.get<Track>(`${this.apiUrl}/${id}`);
  }

  createTrack(track: Track): Observable<Track> {
    return this.http.post<Track>(this.apiUrl, track);
  }

  distributeTrack(id: string, dspIds: string[]): Observable<any> {
    return this.http.post(`${this.apiUrl}/${id}/distribute`, dspIds);
  }

  updateStatus(id: string, status: string): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${id}/status`, JSON.stringify(status), {
      headers: { 'Content-Type': 'application/json' }
    });
  }
}