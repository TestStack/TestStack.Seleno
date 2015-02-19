using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.Configuration.Screenshots
{
    /// <summary>
    /// Camera that saves screenshots to a file with additional contextual information. e.g. 
    /// - Page Title and URL
    /// - Timestamp
    /// - Exception (if available)
    /// - Current Call stack
    /// </summary>
    public class ContextFileCamera : ICamera
    {
        private const string FontFamilyName = "Arial";
        private const int FontPointSize = 10;
        private const int LineSpacing = 5;
        private const int LeftMargin = 10;

        private readonly string _screenShotPath;

        /// <summary>
        /// Constructs a ContextFileCamera.
        /// </summary>
        /// <param name="screenshotPath">The file system directory to save the screenshots in</param>
        public ContextFileCamera(string screenshotPath)
        {
            _screenShotPath = screenshotPath;

            if (!Directory.Exists(_screenShotPath))
                Directory.CreateDirectory(_screenShotPath);
        }

        public void TakeScreenshot(string filename = null, Exception exception = null)
        {
            var frames = new StackTrace(1, true).GetFrames();
            var testFrame = GetTestFrame(frames);
            var relevantFrames = new List<StackFrame>(frames.TakeWhile(x => x != testFrame)) { testFrame };
            var firstFrame = relevantFrames.FirstOrDefault(x => x != testFrame);

            var screenshotFilename = string.Format("{0}{1}", testFrame.GetMethod().Name, firstFrame == null ? string.Empty : string.Concat("_", firstFrame.GetMethod().Name));
            var screenshotPath = (filename == null) ? CreateValidPath(_screenShotPath, screenshotFilename) : Path.Combine(_screenShotPath, filename);

            var callstack = relevantFrames.Select(x => string.Format("{0}.{1}(), Line {2}", x.GetMethod().DeclaringType, x.GetMethod().Name, x.GetFileLineNumber()));

            var contextMetaData = new Dictionary<string, string>
            {
                {"Page Title", string.IsNullOrWhiteSpace(Browser.Title) ? Browser.Url : Browser.Title },
                {"URL", Browser.Url},
                {"Time", string.Format("{0} at {1}", SystemTime.Now().ToShortDateString(), SystemTime.Now().ToLongTimeString())},
                {"Callstack", string.Join(Environment.NewLine, callstack) },
            };

            if (exception != null)
                contextMetaData.Add("Exception", exception.Message);

            var lineCount = string.Join(Environment.NewLine, contextMetaData.Values)
                .Split(Environment.NewLine.ToCharArray())
                .Length;

            var estimatedHeightRequired = lineCount * (FontPointSize + (LineSpacing * 2));
            var screenshot = ScreenshotTaker.GetScreenshot();

            using (var ms = new MemoryStream(screenshot.AsByteArray))
            using (var screenshotBitmap = new Bitmap(ms))
            using (var contextBitmap = new Bitmap(screenshotBitmap.Width, estimatedHeightRequired))
            {
                int actualHeightRequired;
                using (var graphics = Graphics.FromImage(contextBitmap))
                {
                    graphics.FillRectangle(Brushes.Black, new Rectangle(Point.Empty, contextBitmap.Size));

                    using (var arialFontNormal = new Font(FontFamilyName, FontPointSize))
                    using (var arialFontBold = new Font(FontFamilyName, FontPointSize, FontStyle.Bold))
                    {
                        var yOffset = LineSpacing;

                        foreach (var pair in contextMetaData)
                        {
                            var size = graphics.MeasureString(pair.Key, arialFontBold).ToSize();
                            graphics.DrawString(pair.Key, arialFontBold, Brushes.White, new Point(LeftMargin, yOffset));
                            var lines = pair.Value.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            foreach (var line in lines)
                            {
                                graphics.DrawString(line, arialFontNormal, Brushes.White, new Point(LeftMargin + size.Width + 3, yOffset));
                                yOffset += (size.Height + LineSpacing);
                            }
                        }

                        actualHeightRequired = yOffset;
                    }
                }

                var finalSize = new Size(screenshotBitmap.Width, screenshotBitmap.Height + actualHeightRequired);
                using (var finalBitmap = new Bitmap(finalSize.Width, finalSize.Height))
                using (var graphics = Graphics.FromImage(finalBitmap))
                {
                    graphics.DrawImageUnscaledAndClipped(contextBitmap, new Rectangle(Point.Empty, new Size(screenshotBitmap.Width, actualHeightRequired)));
                    graphics.DrawImage(screenshotBitmap, new Point(0, actualHeightRequired));

                    finalBitmap.Save(screenshotPath, ImageFormat.Png);
                }
            }
        }

        private static StackFrame GetTestFrame(StackFrame[] frames)
        {
            foreach (var frame in frames)
            {
                var attributes = frame.GetMethod().GetCustomAttributes(false);
                foreach (var attribute in attributes)
                {
                    switch (attribute.ToString())
                    {
                        case "xUnit.FactAttribute":
                        case "xUnit.TheoryAttribute":
                        case "NUnit.Framework.TestAttribute":
                        case "NUnit.Framework.TestCaseAttribute":
                        case "Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute":
                            return frame;
                    }
                }
            }

            return frames.First();
        }

        private static string CreateValidPath(string path, string filename)
        {
            var validFilename = new StringBuilder(filename);
            validFilename.Replace(' ', '_');
            foreach (var invalidChar in Path.GetInvalidFileNameChars())
                validFilename.Replace(invalidChar, '.');

            var validPath = Path.Combine(path, string.Format("{0}_{1}.png", validFilename, SystemTime.Now().ToFileTimeUtc()));
            return validPath;
        }

        public ITakesScreenshot ScreenshotTaker { get; set; }
        public IWebDriver Browser { get; set; }
    }

}