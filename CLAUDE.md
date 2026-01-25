# Claude Context for Legend of the Greasepole

## What This Is

**Legend of the Greasepole** - a multimedia tribute game to Queen's University's Frosh Week Greasepole event. Originally created in 1999 by 50+ Queen's students, now being revived for web play.

## Repository Structure

```
Greasepole2025/
├── SilverLegend/        # Silverlight C# (2010) - CANONICAL REFERENCE for game logic
├── GodotLegend/         # Godot 4.5.1 C# port - desktop only (C# doesn't compile to WASM)
├── GodotLegendGD/       # Godot 4.5.1 GDScript port - WEB VERSION (actively maintained)
└── docs/migration/      # Porting rules, analysis, migration plans
```

**Primary focus**: `GodotLegendGD` is the web-playable version hosted at https://robburke.net/gptest/

## Key Commands

### Web Export (GDScript version)
```bash
"C:\Users\rob\Downloads\Godot_v4.5.1-stable_win64.exe" --headless --path "e:/projects/Greasepole2025/GodotLegendGD" --export-release "Web" "builds/web/index.html"
```

### Test Locally
```bash
cd e:/projects/Greasepole2025/GodotLegendGD/builds/web
python -m http.server 8080
# Open: http://localhost:8080
```

### Deploy to Production
Double-click `GodotLegendGD/deploy/deploy.bat` (uses WinSCP to upload to robburke.net)

## Important Notes

- **Must use non-Mono Godot** for web export (the one WITHOUT "mono" in the name)
- Export preset "Web" is already configured in `GodotLegendGD/export_presets.cfg`
- Output goes to `GodotLegendGD/builds/web/`

## Architecture (GDScript)

The C# version used partial static classes. In GDScript this becomes:
- **AIMethods.gd** (Autoload) - holds ALL shared state
- **ai*.gd files** - static classes with logic functions that access `AIMethods.variable_name`

See [porting_rules.md](docs/migration/porting_rules.md) for detailed conversion rules.

## Key Files

| Purpose | Location |
|---------|----------|
| **Bug backlog** | [docs/backlog.md](docs/backlog.md) |
| Export commands | [docs/migration/porting_rules.md](docs/migration/porting_rules.md) |
| Deployment | [GodotLegendGD/deploy/DEPLOY.md](GodotLegendGD/deploy/DEPLOY.md) |
| Game controls | [docs/legendweb/how-to-play.md](docs/legendweb/how-to-play.md) |
| Full credits | [docs/legendweb/credits.md](docs/legendweb/credits.md) |

## Web Hosting

- **URL**: https://robburke.net/gptest/
- **Server**: ftp.robburke.net (FTP with explicit TLS)
- **Remote path**: /public_html/gptest
- **Requires**: `.htaccess` with WASM MIME types and CORS headers (already in builds/web/)
