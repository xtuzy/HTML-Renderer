using HtmlRendererCore.Skia.Maui;

namespace HtmlRenderer.Demo.Skia.Maui
{
    public partial class MainPage : ContentPage
    {
        //HtmlLabel htmlLabel = new HtmlLabel();
        //HtmlLabel htmlPanel = new HtmlLabel();
        string htmlString = "<body style=\"font-size:1em; font-family:Microsoft YaHei;text-align:right;\">" +
            "<span style=\"color: red;font-family: FangSong\">这里是</span> an <b>HtmlLabel</b> on transparent background with <span style=\"color: red\">colors</span> and links: " +
                       "<a href=\"http://htmlrenderer.codeplex.com/\">HTML Renderer</a>" +
            "</body>";
        public MainPage()
        {
            InitializeComponent();
            htmlLabel.WidthRequest = 200;
            //htmlLabel.HeightRequest = 50;
            //htmlLabel.AutoSize = true;
            htmlLabel.AutoSizeHeightOnly = true;
            htmlLabel.Opacity = 0.5;
            htmlLabel.Text = "<span style=\"color: red;font-family: FangSong\">这里是</span> an <b>HtmlLabel</b> on transparent background with <span style=\"color: red\">colors</span> and links: " +
                       "<a href=\"http://htmlrenderer.codeplex.com/\">HTML Renderer</a>";
            htmlPanel.AutoSizeHeightOnly = true;
            //htmlPanel.WidthRequest = 300;
            LoadMauiAsset().ContinueWith(async (html) =>
            {
                htmlText = await html;
            });

            var density = DeviceDisplay.MainDisplayInfo.Density;
        }

        private void ManyLabelButton_Clicked(object sender, EventArgs e)
        {
            for (int i = 1; i <= 50; i++)
            {
                stackLayout.Add(new HtmlLabel()
                {
                    Text = htmlString,
                    WidthRequest = 200,
                    AutoSizeHeightOnly = true,
                    BackgroundColor = Colors.LightPink
                });
            }
            this.InvalidateMeasure();
        }

        async Task<string> LoadMauiAsset()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("02.Text.htm");
            using var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }

        string htmlText;
        private void OnCounterClicked(object sender, EventArgs e)
        {
            this.Dispatcher.Dispatch(() =>
            {
                htmlPanel.Text = htmlText;
                this.InvalidateMeasure();
            });
        }
    }
}