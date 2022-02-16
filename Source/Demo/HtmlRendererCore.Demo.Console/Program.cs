// See https://aka.ms/new-console-template for more information
using HtmlRendererCore.Skia;
using HtmlRendererCore.Skia.Adapters;
using SkiaSharp;

Console.WriteLine("Hello, World!");
DrawPagesTest();
void DrawPagesTest()
{
    /*var html = "This is an <b>HtmlLabel</b> on transparent background with <span style=\"color: red\">colors</span> and links: " +
                       "<a href=\"http://htmlrenderer.codeplex.com/\">HTML Renderer</a>";*/
    var filePath = "TestSamples/01.Header.htm";
    var html = File.ReadAllText(filePath);
    SKPdfGenerator.GeneratePdf(
        "html.pdf",html, new SKPdfGenerateConfig()
        {
            MarginLeft = 5,
            MarginTop = 5,
            MarginRight = 5,
            MarginBottom = 5,
        });
}