#!/bin/bash 

INSTALLATION_DIR=$(echo $HOME)/.nazdar
CURRENT_DIR=$(pwd)

if [ "$INSTALLATION_DIR" != "$CURRENT_DIR" ]; then
	echo "Run this from installation directory"
	exit 1
fi

echo "Removing Nazdar..."
rm -r $INSTALLATION_DIR
rm -r $(echo $HOME)/.local/share/applications/nazdar.desktop
update-desktop-database $(echo $HOME)/.local/share/applications/
echo "Done."
