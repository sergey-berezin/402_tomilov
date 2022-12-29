using System;
using System.Collections.Generic;
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
using System.Drawing;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Threading;
using ArcFaceNuget;

namespace Task2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Image<Rgb24>[] face;
        private int Imgamm;
        private CancellationTokenSource cts;
        private CancellationToken ct;
        private MyArcFace element_MyArcFace;
        public MainWindow()
        {
            InitializeComponent();
            element_MyArcFace = new();
            face = new Image<Rgb24>[100];
            Imgamm = 0;
            cts = new();
            ct = cts.Token;
        }
        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Picture";
            dialog.DefaultExt = ".png;.jpg";
            dialog.Filter = "Images (*.jpg, *.png)|*.jpg;*.png";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                string filename = dialog.FileName;
                face[Imgamm] = SixLabors.ImageSharp.Image.Load<Rgb24>(filename);
                System.Windows.Controls.Image image = new()
                {
                    Source = new BitmapImage(new Uri(filename)),
                };
                System.Windows.Controls.Image image2 = new()
                {
                    Source = new BitmapImage(new Uri(filename)),
                };
                List1.Items.Add(image);
                List2.Items.Add(image2);
                Imgamm++;
            }
        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            cts.Cancel();
        }
        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            List1.Items.Clear();
            List2.Items.Clear();
            Imgamm = 0;
        }

        private void ButtonCalc_Click(object sender, RoutedEventArgs e)
        {
            cts = new();
            ct = cts.Token;
            ButtonLoad.IsEnabled = false;
            ButtonCalc.IsEnabled = false;
            ButtonClear.IsEnabled = false;
            LoadText.Visibility = Visibility.Visible;
            AsyncButtonCalc_Click();
        }
        private async void AsyncButtonCalc_Click()
        {
            if ((List1.SelectedIndex==-1) || (List2.SelectedIndex == -1))
            {
                MessageBox.Show("Sellect images");
            }
            else
            {
                try
                {
                    var dist = await element_MyArcFace.Distance(face[List1.SelectedIndex], face[List2.SelectedIndex], ct);
                    var sim = await element_MyArcFace.Similarity(face[List1.SelectedIndex], face[List2.SelectedIndex], ct);
                    TextBlockDist.Text = "Dist=" + dist.ToString();
                    TextBlockSim.Text = "Sim=" + sim.ToString();
                }
                catch
                {
                    MessageBox.Show("Cancelled");
                }
            }
            ButtonLoad.IsEnabled = true;
            ButtonCalc.IsEnabled = true;
            ButtonClear.IsEnabled = true;
            LoadText.Visibility = Visibility.Hidden;
        }

    }
}
