// See https://aka.ms/new-console-template for more information
using HtmlRendererCore.Skia;
using HtmlRendererCore.Skia.Adapters;
using SkiaSharp;

Console.WriteLine("Hello, World!");
DrawPagesTest();
void DrawPagesTest()
{
    var bitmap = new SKBitmap(800, 500);
    var canvas = new SKCanvas(bitmap);
    canvas.Clear(SKColors.White);
    /*var html = "This is an <b>HtmlLabel</b> on transparent background with <span style=\"color: red\">colors</span> and links: " +
                       "<a href=\"http://htmlrenderer.codeplex.com/\">HTML Renderer</a>";*/
    var html = File.ReadAllText("TestSamples/01.Header.htm");
    SKCanvasGenerator.DrawPages(canvas
        , new SKSize(400, 400),
        html, new SKCanvasGenerateConfig()
        {
            MarginLeft = 5,
            MarginTop = 5,
            MarginRight = 5,
            MarginBottom = 5,
        });
    canvas.Flush();
    using (var s = File.OpenWrite("html.png"))
    {
        var result = bitmap.Encode(s, SKEncodedImageFormat.Png, 600);
    }
}