/*
    MassImageConverter - Will convert images with given extension to JPEG
    Copyright (C) 2018 Peter Wetzel

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using CommandLine;

namespace MassImageConverter
{
    public class MassImageConverterOptions
    {
        [Option('f', "folder", Required = true, HelpText = "Folder path to process")]
        public string FolderPath { get; set; }

        [Option('e', "extensions", Required = true, HelpText = "Extensions (comma separated) of image files types to convert")]
        public string Extensions { get; set; }

        [Option('q', "quality", Default = 90, HelpText = "JPEG quality (1.0-100)")]
        public long Quality { get; set; }

        [Option('k', "keep", HelpText = "Flag for keeping original files.")]
        public bool Keep { get; set; }

        [Option('h', "harddelete", HelpText = "Flag for doing a hard delete instead of using recycling bin.")]
        public bool HardDelete { get; set; }

        [Option('i', "immediate", HelpText = "Flag for skipping initial pause button prior to running. Useful for automated scripts.")]
        public bool Immediate { get; set; }

        [Option('v', "verbose", HelpText = "Flag for displaying all event messages")]
        public bool Verbose { get; set; }

        [Option('d', "debug", HelpText = "Flag for debug only mode. No files will be deleted.")]
        public bool Debug { get; set; }
    }
}