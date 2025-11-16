# NAZDAR! The Game

Build your armored train and defend it against the bloody Bolsheviks.

**Homepage** https://skoula.cz/nazdar

## Linux Installation

### Prerequisites

Before launching the game, you must install the OpenAL library:

```bash
sudo apt install libopenal-dev
```

### Running the Game
To run the game, simple enable execution permissions and execute the binary:

```bash
chmod +x Nazdar.GL
./Nazdar.GL
```

### Installation (Optional)

The `install.sh` script is optional. To use it, make it executable and run:

```bash
chmod +x install.sh
./install.sh
```

It will copy the game binary and its data to `$HOME/.nazdar` and create a desktop entry for easy access.
