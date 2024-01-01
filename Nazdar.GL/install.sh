#!/bin/bash 

INSTALLATION_DIR=$(echo $HOME)/.nazdar
CURRENT_DIR=$(pwd)

create_installation_directory() {
	echo "Creating instalation directory ${INSTALLATION_DIR}..."
	mkdir ${INSTALLATION_DIR} || exit 1
}

make_executable() {
	echo "Making $1 executable..."
	chmod +x $1 ||  exit 1
}

copy_to_installation_directory() {
	echo "Moving files to instalation directory '${INSTALLATION_DIR}'..."
	cp -r ${CURRENT_DIR}/* ${INSTALLATION_DIR}/ ||  exit 1
}

create_and_install_desktop_file() {
	echo "Creating desktop file"
	cat << EOF > $(echo $HOME)/.local/share/applications/nazdar.desktop
[Desktop Entry]
Version=1.0
Type=Application
Terminal=false
Exec=$INSTALLATION_DIR/Nazdar.GL
Name=Nazdar
Icon=$INSTALLATION_DIR/Icon.ico
EOF

	# update desktop file database
	update-desktop-database $(echo $HOME)/.local/share/applications/
}

create_installation_directory
make_executable "./Nazdar.GL"
make_executable "./uninstall.sh"
copy_to_installation_directory
create_and_install_desktop_file
echo "Done."
