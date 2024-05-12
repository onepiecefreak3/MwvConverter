# MwvConverter

## Description
A command line tool to de- and encode .mwv files from various PS2 games by Level5.

## Usage

Various options have to be set to properly use the command line tool.

| Option | Description |
| - | - |
| -h | Shows a help text explaining all the options listed here and examples on how to use use them. |
| -o | The operation to execute. Has to be followed by either:<br>`d` to decode a MWV<br>`e` to encode a PCM16 WAV to MWV |
| -f | The file or directory to execute the operation on. |

## Examples

To decode a MWV to PCM16 WAV:<br>
```MwvConverter.exe -o d -f Path/To/File.mwv```

To encode a PCM16 WAV to MWV:<br>
```MwvConverter.exe -o e -f Path/To/File.wav```
