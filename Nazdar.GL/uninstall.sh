#!/bin/bash 

INSTALLATION_DIR=$(echo $HOME)/.nazdar
CURRENT_DIR=$(pwd)

echo "Removing Nazdar..."
rm -r $INSTALLATION_DIR
rm -r $(echo $HOME)/.local/share/applications/nazdar.desktop
update-desktop-database $(echo $HOME)/.local/share/applications/
echo "Done."
