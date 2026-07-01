```markdown
# DECISIONS.md — Music Distribution Full Stack Task

## 1. What did AI generate for you, and what did you write or modify yourself?

### AI Generated:
- Initial project structure and Clean Architecture layers (Domain, Application, Infrastructure, API)
- Basic Entity definitions (Artist, Track, DSP, TrackDistribution)
- Repository pattern implementation with EF Core
- JWT authentication setup and configuration
- Angular components basic structure
- Swagger/OpenAPI integration suggestions

### What I Wrote/Modified:

#### Backend Changes:
1. **Fixed OpenAPI Package Conflict**: AI suggested using both `Microsoft.AspNetCore.OpenApi` and `Swashbuckle.AspNetCore`, but they use incompatible versions of `Microsoft.OpenApi` (v1.x vs v2.x). I removed `Microsoft.AspNetCore.OpenApi` and kept Swashbuckle only.

2. **Fixed CORS Placement**: AI put `builder.Services.AddCors()` after `builder.Build()`, which throws `InvalidOperationException: 'The service collection cannot be read-only.'` I moved it to the configuration phase before `Build()`.

3. **Added DTOs to Prevent JSON Cycles**: AI returned Entities directly from controllers, causing `JsonException: A possible object cycle was detected` (Artist → Tracks → Artist → Tracks...). I created DTOs (TrackDto, ArtistDto, DistributionDto) to break the cycle.

4. **Added Input Validation**: Added meaningful error responses for:
   - Missing/invalid artist name
   - Invalid email format
   - Missing ISRC
   - Artist not found when creating track

5. **Created Seed Data**: Added realistic Arabic music industry data:
   - Artists: Amr Diab, Nancy Ajram, Hussain Al Jassmi
   - Tracks: 8 tracks across Pop, Khaliji, Dance genres
   - DSPs: Spotify, Apple Music, YouTube Music

#### Frontend Changes:
6. **Fixed Zone.js Import**: Angular 18+ requires explicit `import 'zone.js'` in `main.ts`. AI didn't mention this, causing `NG0908: In this configuration Angular requires Zone.js`.

7. **Fixed HTTPS/HTTP Issue**: Angular couldn't connect to API due to self-signed certificate. Changed API URL from `https://localhost:7195` to `http://localhost:5001`.

8. **Fixed Change Detection**: Added `ChangeDetectorRef` to force UI updates after API calls, preventing "must refresh to see data" issue.

9. **Fixed Dropdown Filter**: Changed from `(change)` with `[(ngModel)]` to `[ngModel]` with `(change)` using `$any($event.target).value` for proper two-way binding.

## 2. What security issues did you find (or introduce) in the AI-generated code? How did you handle them?

### Issues Found:

1. **JWT Key Hardcoded in appsettings.json**
   - **Risk**: Sensitive key committed to source control
   - **Fix**: For production, move to environment variables or Azure Key Vault. Kept in appsettings for demo purposes with a comment warning.

2. **CORS Too Permissive**
   - **Risk**: `AllowAnyOrigin()` allows any website to call the API
   - **Fix**: Restricted to `http://localhost:4200` only:
   ```csharp
   policy.WithOrigins("http://localhost:4200")
         .AllowAnyHeader()
         .AllowAnyMethod();