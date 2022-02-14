using NUnit.Framework;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using System.IO;

namespace TheArtOfDev.HtmlRenderer.PdfSharp.Tests
{
    [TestFixture()]
    public class PdfGeneratorTests
    {
        [Test()]
        public void DrawPagesTest()
        {
            var bitmap = new SKBitmap(400, 400);
            var canvas = new SKCanvas(bitmap);
            var html = "";
            PdfGenerator.DrawPages(canvas
                ,new SKSize(400,400),
                html,new PdfGenerateConfig()
                {
                    MarginLeft=5,
                    MarginTop=5,
                    MarginRight=5,
                    MarginBottom=5,
                });
            canvas.Flush();
            using (var s = File.OpenWrite("html.png"))
            {
               var result= bitmap.Encode(s, SKEncodedImageFormat.Png, 300);
                Assert.IsTrue(result);
            }
               
        }
    }
}