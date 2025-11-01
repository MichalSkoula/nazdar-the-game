# Flatpak Package for NAZDAR! The Game

This directory contains the Flatpak manifest and related files for building and distributing NAZDAR! The Game as a Flatpak package on Linux.

## What is Flatpak?

Flatpak is a universal package format for Linux that provides sandboxed applications with all their dependencies. This makes it easy to distribute and install applications across different Linux distributions.

## Files

- `cz.skoula.nazdar.yml` - The Flatpak manifest that defines how to build the application
- `cz.skoula.nazdar.desktop` - Desktop entry file for application launcher
- `cz.skoula.nazdar.metainfo.xml` - AppData metadata for application stores

## Prerequisites

To build the Flatpak package, you need:

1. Install Flatpak and Flatpak Builder:
   ```bash
   # On Debian/Ubuntu
   sudo apt install flatpak flatpak-builder
   
   # On Fedora
   sudo dnf install flatpak flatpak-builder
   
   # On Arch Linux
   sudo pacman -S flatpak flatpak-builder
   ```

2. Add Flathub repository (if not already added):
   ```bash
   flatpak remote-add --if-not-exists flathub https://flathub.org/repo/flathub.flatpakrepo
   ```

3. Install the required runtime and SDK:
   ```bash
   flatpak install flathub org.freedesktop.Platform//23.08
   flatpak install flathub org.freedesktop.Sdk//23.08
   flatpak install flathub org.freedesktop.Sdk.Extension.dotnet8
   ```

## Building the Flatpak

From the repository root directory, run:

```bash
cd flatpak
flatpak-builder --force-clean --repo=repo build-dir cz.skoula.nazdar.yml
```

This will:
- Create a local repository in the `repo` directory
- Build the application in the `build-dir` directory
- Package everything as a Flatpak

## Installing Locally

After building, you can install the Flatpak locally:

```bash
flatpak-builder --user --install --force-clean build-dir cz.skoula.nazdar.yml
```

Or from the repository:

```bash
flatpak --user remote-add --no-gpg-verify nazdar-repo repo
flatpak --user install nazdar-repo cz.skoula.nazdar
```

## Running the Application

After installation, you can run the application:

```bash
flatpak run cz.skoula.nazdar
```

Or launch it from your application menu.

## Creating a Flatpak Bundle

To create a single-file bundle for distribution:

```bash
flatpak build-bundle repo nazdar-the-game.flatpak cz.skoula.nazdar
```

Users can then install this bundle:

```bash
flatpak install nazdar-the-game.flatpak
```

## Updating the Manifest

When updating the manifest for a new release:

1. Update the `tag` and `commit` fields in the manifest to point to the new release
2. Update the version in `cz.skoula.nazdar.metainfo.xml`
3. Add release notes to the metainfo file

## Publishing to Flathub

To publish this application to Flathub (the main Flatpak repository):

1. Fork the [Flathub repository](https://github.com/flathub/flathub)
2. Follow the [Flathub submission guidelines](https://github.com/flathub/flathub/wiki/App-Submission)
3. Submit a pull request with the manifest files

## Troubleshooting

### Build fails with missing dependencies

Make sure you have installed the .NET 8 SDK extension:
```bash
flatpak install flathub org.freedesktop.Sdk.Extension.dotnet8
```

### Application doesn't start

Check the logs:
```bash
flatpak run --command=sh cz.skoula.nazdar
journalctl --user -xe | grep nazdar
```

### Permission issues

The application uses these permissions:
- `--socket=x11` and `--socket=wayland` - For display
- `--socket=pulseaudio` - For audio
- `--device=dri` - For GPU acceleration
- `--persist=.nazdar` - For saving game data

## License

This Flatpak packaging is part of NAZDAR! The Game and is licensed under AGPL-3.0-or-later.
