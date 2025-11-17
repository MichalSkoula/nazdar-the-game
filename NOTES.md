# DEVELOPER NOTES

## Packaging 🤖

https://docs.monogame.net/articles/getting_started/packaging_games.html

Go to project (not solution) folder (for example Nazdar.GL for OpenGL version) and run publish command:

Short (depends on project settings):

```
dotnet publish -c Release -r win-x64
```
```
dotnet publish -c Release -r linux-x64
```

Full:

```
dotnet publish -c Release -r win-x64 /p:PublishReadyToRun=false /p:TieredCompilation=false /p:PublishSingleFile=true --self-contained
```

```
dotnet publish -c Release -r linux-x64 /p:PublishReadyToRun=false /p:TieredCompilation=false /p:PublishSingleFile=true --self-contained
```

### Linux enthusiasts ❤️

You must install the ```freetype``` and ```freetype-devel``` packages (on Fedora) or the ```libfreetype6``` packages (on Debian) in order to **build the Nazdar.GL project**.

### Store 🦄

* Nazdar.DX.Packaging	 x86 (Windows)	Publish -> Create App Package; it will package DX version for 
* Nazdar.UWP			 x64 (Xbox)		Publish -> Create App Package; it will package UWP version for Windows Store - Xbox
* Nazdar.GL				 x64 (Linux)	dotnet publish -c Release -r linux-x64
* Nazdar.Android						Change version in AndroidManifest; Switch to Release; then Archive

(sometimes Visual Studio restart is required...)
(to update packages in partner.microsoft.com, first upload UWP x64 package and then DX x86 package (with different version strings), otherwise it will prefer the UWP version for every device)

## MGCB

run mgcb (must be inside project folder, not solution folder)

```
dotnet mgcb-editor
```

## Tech

* .NET 9
* MonoGame.Extended 3.7 + MonoGame.Extended.Particles (3.8 is bugged and newer versions does not support UWP)
* C# 8 because of UWP (Xbox)
