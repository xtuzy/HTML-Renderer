// See https://aka.ms/new-console-template for more information
using HtmlRendererCore.Demo.Console;
using HtmlRendererCore.Skia;
using HtmlRendererCore.Skia.Adapters;
using SkiaSharp;

Console.WriteLine("Hello, World!");
DrawPagesTest();
void DrawPagesTest()
{
    /*var html = "This is an <b>HtmlLabel</b> on transparent background with <span style=\"color: red\">colors</span> and links: " +
                       "<a href=\"http://htmlrenderer.codeplex.com/\">HTML Renderer</a>";*/
    SamplesLoader.Init("Windows","11");
     Directory.CreateDirectory(@"result\TestSamples");
    foreach(var sample in SamplesLoader.TestSamples)
    {
        if (sample.Name.Contains("image") || sample.Name.Contains("Image"))
            continue;
        var html = sample.Html;
        try
        {
            SKPdfGenerator.GeneratePdf(
                @$"result\TestSamples\{sample.FullName}.pdf", html, new SKPdfGenerateConfig()
                {
                    MarginLeft = 5,
                    MarginTop = 5,
                    MarginRight = 5,
                    MarginBottom = 5,
                });
        }catch(Exception e)
        {
            Console.WriteLine(e.Message+sample.FullName);
        }
    }
    Directory.CreateDirectory(@"result\Samples");
    foreach (var sample in SamplesLoader.ShowcaseSamples)
    {
        if (sample.Name.Contains("image") || sample.Name.Contains("Image"))
            continue;
        var html = sample.Html;
        try
        {
            SKPdfGenerator.GeneratePdf(
                @$"result\Samples\{sample.FullName}.pdf", html, new SKPdfGenerateConfig()
                {
                    MarginLeft = 5,
                    MarginTop = 5,
                    MarginRight = 5,
                    MarginBottom = 5,
                });
        }catch(Exception e)
        {
            Console.WriteLine(e.Message + sample.FullName);
        }
    }
}
   
    