using HtmlRendererCore.Demo.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HtmlRendererCore.Demo.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            htmlLabel.Text = SampleHtmlLabelText;
            htmlLabel.AutoSizeHeightOnly = true;

            //htmlPanel.Text = SampleHtmlPanelText;
            button.Click += (sender, e) =>
            {
                DrawPagesTest();
            };
        }

        /// <summary>
        /// The HTML text used in sample form for HtmlLabel.
        /// </summary>
        public String SampleHtmlLabelText
        {
            get
            {
                return "This is an <b>HtmlLabel</b> on transparent background with <span style=\"color: red\">colors</span> and links: " +
                       "<a href=\"http://htmlrenderer.codeplex.com/\">HTML Renderer</a>";
            }
        }

        /// <summary>
        /// The HTML text used in sample form for HtmlPanel.
        /// </summary>
        public String SampleHtmlPanelText
        {
            get
            {
                /*return "This is an <b>HtmlPanel</b> with <span style=\"color: red\">colors</span> and links: <a href=\"http://htmlrenderer.codeplex.com/\">HTML Renderer</a>" +
                       "<div style=\"font-size: 1.2em; padding-top: 10px;\" >If there is more text than the size of the control scrollbars will appear.</div>" +
                       "<br/>Click me to change my <code>Text</code> property.";*/
                var text = File.ReadAllText("TestSamples/01.Header.htm");
                return text;
            }
        }

        void DrawPagesTest()
        {
            /*var html = "This is an <b>HtmlLabel</b> on transparent background with <span style=\"color: red\">colors</span> and links: " +
                               "<a href=\"http://htmlrenderer.codeplex.com/\">HTML Renderer</a>";*/
            SamplesLoader.Init("Windows", "11");
            Directory.CreateDirectory(@"result\TestSamples");
           /* foreach (var sample in SamplesLoader.TestSamples)
            {
                var html = sample.Html;
            }*/
            Directory.CreateDirectory(@"result\Samples");
            foreach (var sample in SamplesLoader.ShowcaseSamples)
            {
                var html = sample.Html;
                if (sample.Name.Contains("Text"))
                {
                    htmlPanel.Text = html;
                    break;
                }
            }
        }
    }
}
