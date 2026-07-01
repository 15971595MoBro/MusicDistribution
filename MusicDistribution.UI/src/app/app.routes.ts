import { Routes } from '@angular/router';
import { TrackListComponent } from './components/track-list/track-list';
import { TrackDetailComponent } from './components/track-detail/track-detail';


export const routes: Routes = [
  { path: '', redirectTo: 'tracks', pathMatch: 'full' },
  { path: 'tracks', component: TrackListComponent },
  { path: 'tracks/:id', component: TrackDetailComponent }
];
