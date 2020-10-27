/*
    MassImageConverter - Will convert images with given extension to JPEG
    Copyright (C) 2016 Peter Wetzel

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
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using WetzUtilities;

namespace MassImageConverter.Core
{
    public class ImageConverter
    {
        /// <summary>
        /// File extensions to process
        /// </summary>
        public List<string> Extensions { get; set; }
        /// <summary>
        /// JPEG quality (1.0-100)
        /// </summary>
        public long Quality { get; set; } = 90L;
        /// <summary>
        /// Flag for whether old file should be kept or deleted
        /// </summary>
        public bool IsKeeper { get; set; }
        // Settings
        /// <summary>
        /// Flag for whether we try to use the recycling bin or fo a hard delete (physically delete the file).
        /// </summary>
        public bool IsHardDelete { get; set; } = false;
        /// <summary>
        /// Flag for whether we write output for all steps or not.
        /// </summary>
        public bool IsVerbose { get; set; }
        /// <summary>
        /// Flag for whether we actually delete the files or just do a dry run.
        /// </summary>
        public bool IsDebugOnly { get; set; }

        // Tracking
        public long ElapsedMS { get; private set; }
        public List<ImageFile> Completed = new List<ImageFile>();
        public List<ImageFile> Failed = new List<ImageFile>();

        /// <summary>
        /// Zapper will recursively remove all small images in the given folder and its subfolders.
        /// </summary>
        /// <param name="path">Folder path to process</param>
        public void Process(string path)
        {
            if (path.IsEmpty())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Path required");
                return;
            }
            if (!Directory.Exists(path))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Folder does not exist: {path}");
                return;
            }
            if (Extensions == null || !Extensions.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("At least one extension is required");
                return;
            }
            var timer = new Stopwatch();
            timer.Start();
            var codec = ImageCodecInfo.GetImageDecoders().First(x => x.FormatID == ImageFormat.Jpeg.Guid);
            using (var encoders = new EncoderParameters(1))
            {
                encoders.Param[0] = new EncoderParameter(Encoder.Quality, Quality);
                string filters = Extensions.Count == 1 ? $"*{Extensions[0]}" : "*.*";
                var files = Directory.GetFiles(path, filters, System.IO.SearchOption.AllDirectories);
                foreach (var f in files)
                {
                    if (!Extensions.Contains(Path.GetExtension(f)))
                    {
                        continue;
                    }
                    var resultImage = new ImageFile { Source = f };
                    try
                    {
                        if (!IsDebugOnly)
                        {
                            string newFileName = Path.GetFileNameWithoutExtension(f);
                            resultImage.Converted = FileUtilities.GetNextName(Path.GetDirectoryName(f), $"{newFileName}.jpg");
                            using (var bitmap = new Bitmap(f))
                            {
                                bitmap.Save(resultImage.Converted, codec, encoders);
                            }
                            FileInfo oldFile = new FileInfo(f);
                            File.SetCreationTime(resultImage.Converted, oldFile.CreationTime);
                            File.SetLastWriteTime(resultImage.Converted, oldFile.LastWriteTime);
                            if (!IsKeeper)
                            {
                                if (IsVerbose)
                                {
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.WriteLine($"Removing original: {f}");
                                }
                                if (IsHardDelete)
                                {
                                    File.Delete(f);
                                }
                                else
                                {
                                    FileSystem.DeleteFile(f, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                                }
                            }
                        }
                        if (IsVerbose)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"Converted: {f}");
                        }
                        Completed.Add(resultImage);
                    }
                    catch (Exception ex)
                    {
                        resultImage.Error = ex.ToString();
                        Failed.Add(resultImage);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Error for {f}: {ex.Message}");
                    }
                }
            }
            timer.Stop();
            ElapsedMS = timer.ElapsedMilliseconds;
        }
    }
}