export interface Track {
  id: string;
  title: string;
  artistId: string;
  artistName: string;      // ← من الـ DTO
  isrc: string;
  releaseDate: string;
  genre: string;
  status: string;          // ← string مش enum
  distributions?: Distribution[];
}

export interface Distribution {
  id: string;
  dspName: string;         // ← من الـ DTO
  submittedAt: string;
  status: string;          // ← string مش number
}

export interface Artist {
  id: string;
  name: string;
  email: string;
  country: string;
}