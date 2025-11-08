# Copilot Instructions for NAZDAR! The Game

## Project Overview
NAZDAR! The Game is a MonoGame-based platform/strategic game written in C#. The game features a village management system, survival mechanics, and various game screens including map exploration, village building, and combat.

## Technology Stack
- **Framework**: MonoGame 3.8.3 with MonoGame.Extended 3.7.0
- **Language**: C# 8 (required for UWP/Xbox compatibility)
- **.NET**: .NET 8.0 (net8.0-windows10.0.22000.0 for Windows, net8.0 for Linux)
- **Key Dependencies**:
  - MonoGame.Framework.DesktopGL
  - MonoGame.Extended & MonoGame.Extended.Particles
  - Newtonsoft.Json for serialization
  - TiledCS for tile map handling
  - Xamarin.Essentials

## Project Structure
The solution uses a shared project pattern to support multiple platforms:

- **Nazdar.Shared/**: Core game logic shared across all platforms
  - `Game1.cs`: Main game class
  - `Screens/`: Game screens (Menu, Village, Map, Survival, etc.)
  - `Objects/`: Game objects and entities
  - `Controls/`: Input handling
  - `Content/`: Game assets (sprites, sounds, maps)
  - `Translation/`: Localization files
  
- **Nazdar.GL/**: Desktop OpenGL version (Windows/Linux)
- **Nazdar.UWP/**: Universal Windows Platform version (Xbox)
- **Nazdar.Android/**: Android version
- **Nazdar.GL.Packaging/**: Windows Store packaging

## Building and Publishing

### Building
```bash
# Restore tools first
dotnet tool restore

# Build for current platform
dotnet build -c Release

# For Linux development: Install freetype and freetype-devel (Fedora) or libfreetype6 (Debian)
```

### Publishing
```bash
# Windows
dotnet publish -c Release -r win-x64 /p:PublishReadyToRun=false /p:TieredCompilation=false /p:PublishSingleFile=true --self-contained

# Linux
dotnet publish -c Release -r linux-x64 /p:PublishReadyToRun=false /p:TieredCompilation=false /p:PublishSingleFile=true --self-contained
```

### Content Pipeline
```bash
# Edit game content (must be run from project folder, not solution folder)
cd Nazdar.GL  # or appropriate platform project
dotnet mgcb-editor
```

## Code Style and Conventions

### EditorConfig
- Insert final newline in all files

### C# Guidelines
- Use C# 8 syntax (do not use newer features due to UWP compatibility)
- Follow standard C# naming conventions
- Keep shared code in `Nazdar.Shared/` project
- Platform-specific code goes in respective platform projects

### Game Architecture
- Screen-based architecture: Each major game mode is a separate screen class
- Large screen classes are split into partial classes (e.g., `VillageScreen.cs`, `VillageScreenDraw.cs`, `VillageScreenUpdate.cs`)
- Assets are loaded through the `Assets` static class
- Audio managed through the `Audio` static class
- Use MonoGame.Extended for sprite rendering and particle effects

## Platform Considerations

### Windows (Nazdar.GL)
- Target: net8.0-windows10.0.22000.0
- Self-contained with embedded debug symbols
- Requires SDL2.dll (copied to output)

### Linux (Nazdar.GL)
- Target: net8.0
- Requires freetype libraries installed on the system
- Single-file self-contained executable
- Includes install/uninstall scripts

### UWP/Xbox (Nazdar.UWP)
- Requires C# 8 compatibility
- Special packaging for Microsoft Store
- x64 build for Xbox

### Android (Nazdar.Android)
- Version managed in AndroidManifest
- Archive for publishing

## Assets and Credits
The game uses various open-source and Creative Commons licensed assets:
- Graphics: CC-29 color palette, pixel art from various itch.io artists
- Music: Lofi and metal tracks from OpenGameArt
- Sounds: Various CC0 sound effects from OpenGameArt
- Fonts: Dogika font family

Always respect licenses when adding new assets and update credits in README.md.

## Development Workflow

### Testing
- Currently no automated test suite
- Manual testing required for game functionality
- Test on multiple platforms when making core changes

### Version Control
- Solution file includes .editorconfig, .gitignore in solution items
- Use .gitignore patterns for Visual Studio and build artifacts

## Important Notes
- MonoGame.Extended version locked to 3.7 (3.8 has known bugs)
- Must use C# 8 (not newer) for UWP/Xbox compatibility
- Run `dotnet tool restore` before building to ensure MGCB tools are available
- Content files (.mgcb) must be edited from within the project directory
- Visual Studio may require restart when working with Store packaging

## Common Tasks

### Adding New Game Content
1. Add raw assets to appropriate `Content/` subfolder
2. Open MGCB editor: `dotnet mgcb-editor` (from project folder)
3. Add content files and configure processors
4. Load in code through `Assets` class

### Adding New Game Screen
1. Create new class in `Nazdar.Shared/Screens/`
2. Implement draw and update logic
3. Add screen transitions in existing screens
4. Consider splitting into partial classes if screen becomes large

### Modifying Game Objects
- Game objects located in `Nazdar.Shared/Objects/`
- Ensure serialization compatibility for save/load functionality
- Update collision detection if changing object bounds
