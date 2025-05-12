# NOTES

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

You must install the ```freetype``` and ```freetype-devel``` packages (on Fedora) or the ```libfreetype6``` packages (on Debian) in order to build the Nazdar.GL project.

### Store 🦄

* Nazdar.GL.Packaging	 x86 (Desktop)	Publish -> Create App Package
* Nazdar.UWP			 x64 (Xbox)		Publish -> Create App Package
* Nazdar.Android						Change version in AndroidManifest, then Archive

(sometimes Visual Studio restart is required...)
(to update packages in partner.microsoft.com, first upload UWP package and then GL package (with different version strings), otherwise it will prefer the UWP version for every device)

## MGCB

run mgcb (must be inside project folder, not solution folder)

```
dotnet mgcb-editor
```

## Tech

* MonoGame.Extended 3.7 (3.8 is bugged)
* C# 8 because of UWP (Xbox)
