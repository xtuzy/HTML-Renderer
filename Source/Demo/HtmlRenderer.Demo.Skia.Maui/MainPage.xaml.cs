namespace HtmlRenderer.Demo.Skia.Maui
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            htmlLabel.WidthRequest = 200;
            //htmlLabel.HeightRequest = 50;
            //htmlLabel.AutoSize = true;
            htmlLabel.AutoSizeHeightOnly = true;
            //htmlLabel.BackgroundColor = Colors.AliceBlue;
            htmlLabel.Opacity = 0.5;
            htmlLabel.Text = "<span style=\"color: red;font-family: FangSong\">这里是</span> an <b>HtmlLabel</b> on transparent background with <span style=\"color: red\">colors</span> and links: " +
                       "<a href=\"http://htmlrenderer.codeplex.com/\">HTML Renderer</a>";
            htmlPanel.AutoSizeHeightOnly = true;
            LoadMauiAsset().ContinueWith(async (html) =>
            {
                htmlPanel.Text = await html;
            });
        }

        async Task<string> LoadMauiAsset()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("02.Text.htm");
            using var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}