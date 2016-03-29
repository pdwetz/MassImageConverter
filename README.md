# Mass Image Converter
Console app for converting images with given extensions to JPEG. Utilizes [Command Line Parser](https://github.com/gsscoder/commandline) (see examples below for usage tips). It will generally send the original files to the recycling bin, but you can also do a "hard delete" (which is probably preferred for massive file counts), or you can tell it to keep the originals. Similarly, when run it will initially pause as a "are you sure" check, but that can be overridden so that you can utilize it in automated scripts.
# Options
All flag options are considered true if provided, false if not. They do not accept values. Extensions can include the leading period or not.
```
  -f, --folder        Required. Folder path to process
  -e, --extensions    Required. Extensions (comma separated) of image files types to convert
  -q, --quality       (Default: 90) JPEG quality (1.0-100)
  -k, --keep          Flag for keeping original files.
  -h, --harddelete    Flag for doing a hard delete instead of using recycling bin.
  -i, --immediate     Flag for skipping initial pause button prior to running. Useful for automated scripts.
  -v, --verbose       Flag for displaying all event messages
  -d, --debug         Flag for debug only mode. No files will be deleted.
  --help              Display this help screen.
```
# Example Usage
```
Usage: MassImageConverter -f c:\somepath\xyx -e png,bmp
```
## Restrictions
If a folder path includes spaces it must be enclosed in quotes and NOT have a trailing backslash (that is, do not use something like "c:\my test\"). This is due to how Windows/.NET parses command line arguments.
# License
GNU GPL v3. See license file for more information.