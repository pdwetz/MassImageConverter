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
using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using MassImageConverter.Core;

namespace MassImageConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            var initialColor = Console.ForegroundColor;
            Parser.Default.ParseArguments<MassImageConverterOptions>(args)
                .WithParsed<MassImageConverterOptions>(options => Process(options));
            Console.ForegroundColor = initialColor;
        }

        private static void Process(MassImageConverterOptions options)
        {
            var converter = new ImageConverter
            {
                IsKeeper = options.Keep,
                Quality = options.Quality,
                Extensions = ParseExtensions(options.Extensions)
            };
            if (!converter.Extensions.Any())
            {
                converter.Extensions.Add(".bmp");
                converter.Extensions.Add(".png");
            }
            converter.IsDebugOnly = options.Debug;
            converter.IsHardDelete = options.HardDelete;
            converter.IsVerbose = options.Verbose;
            Console.ForegroundColor = ConsoleColor.Gray;
            if (!options.Immediate || converter.IsVerbose)
            {
                Console.WriteLine($"Root path: {options.FolderPath}");
                if (converter.IsKeeper)
                {
                    Console.WriteLine("Originals will not be deleted");
                }
                else
                {
                    if (converter.IsHardDelete)
                    {
                        Console.WriteLine("Originals will be deleted permanently");
                    }
                    else
                    {
                        Console.WriteLine("Originals will be deleted to recycle bin");
                    }
                }
                Console.WriteLine($"Converting extensions {string.Join(", ", converter.Extensions)} to JPG");
            }
            if (converter.IsDebugOnly)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Debug mode enabled. No files will be deleted.");
            }
            Console.ForegroundColor = ConsoleColor.White;
            if (!options.Immediate)
            {
                Console.WriteLine("SmallImageZapper ready to begin. Press any key to continue.");
                Console.ReadLine();
            }
            Console.WriteLine("Working...");
            converter.Process(options.FolderPath);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Converted {converter.Completed.Count:#,##0} files in {converter.ElapsedMS:#,##0} ms. Failed on {converter.Failed.Count:#,##0} files.");
        }

        private static List<string> ParseExtensions(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                return new List<string>();
            }
            List<string> ext = raw.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            for (int i = 0; i < ext.Count; i++)
            {
                if (ext[i][0] != '.')
                {
                    ext[i] = $".{ext[i].Trim()}";
                }
                else
                {
                    ext[i] = ext[i].Trim();
                }
            }
            return ext;
        }
    }
}