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
using System.Collections.Generic;
using System.IO;
using MassImageConverter.Core;
using NUnit.Framework;

namespace MassImageConverter.Test
{
    [TestFixture]
    public class ImageConverterTester
    {
        [Test]
        public void filter_png()
        {
            string folderPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "assets");
            var converter = new ImageConverter();
            converter.Extensions = new Dictionary<string, string>();
            converter.Extensions.Add(".png", ".png");
            converter.IsDebugOnly = true;
            converter.IsVerbose = true;
            converter.Process(folderPath);
            Assert.AreEqual(5, converter.Completed.Count);
            Assert.AreEqual(0, converter.Failed.Count);
        }

        [Test]
        public void filter_bmp()
        {
            string folderPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "assets");
            var converter = new ImageConverter();
            converter.Extensions = new Dictionary<string, string>();
            converter.Extensions.Add(".bmp", ".bmp");
            converter.IsDebugOnly = true;
            converter.IsVerbose = true;
            converter.Process(folderPath);
            Assert.AreEqual(2, converter.Completed.Count);
            Assert.AreEqual(0, converter.Failed.Count);
        }

        [Test]
        public void filter_multiple_extensions()
        {
            string folderPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "assets");
            var converter = new ImageConverter();
            converter.Extensions = new Dictionary<string, string>();
            converter.Extensions.Add(".png", ".png");
            converter.Extensions.Add(".bmp", ".bmp");
            converter.IsDebugOnly = true;
            converter.IsVerbose = true;
            converter.Process(folderPath);
            Assert.AreEqual(7, converter.Completed.Count);
            Assert.AreEqual(0, converter.Failed.Count);
        }

        [Test]
        public void file_conversion()
        {
            string folderPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "assets");
            var converter = new ImageConverter();
            converter.Extensions = new Dictionary<string, string>();
            converter.Extensions.Add(".bmp", ".bmp");
            converter.IsDebugOnly = false;
            converter.IsKeeper = true;
            converter.IsVerbose = true;
            converter.Process(folderPath);
            Assert.AreEqual(2, converter.Completed.Count);
            Assert.AreEqual(0, converter.Failed.Count);
            foreach (var f in converter.Completed)
            {
                Assert.That(File.Exists(f.Converted));
                File.Delete(f.Converted);
            }
        }
    }
}