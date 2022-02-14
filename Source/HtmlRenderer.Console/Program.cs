// See https://aka.ms/new-console-template for more information
using SkiaSharp;
using TheArtOfDev.HtmlRenderer.PdfSharp;

Console.WriteLine("Hello, World!");
DrawPagesTest();
void DrawPagesTest()
{
    var bitmap = new SKBitmap(400, 400);
    var canvas = new SKCanvas(bitmap);
    canvas.DrawColor(SKColors.White);
    var html = "<html><head><title>Text</title></head></html> ";
    PdfGenerator.DrawPages(canvas
        , new SKSize(400, 400),
        html, new PdfGenerateConfig()
        {
            MarginLeft = 5,
            MarginTop = 5,
            MarginRight = 5,
            MarginBottom = 5,
        });
    canvas.Flush();
    using (var s = File.OpenWrite("html.png"))
    {
        var result = bitmap.Encode(s, SKEncodedImageFormat.Png, 300);
    }

}