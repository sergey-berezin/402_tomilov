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
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Task3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Image<Rgb24>[] face;
        private string[] facepath;
        private int Imgamm;
        private CancellationTokenSource cts;
        private CancellationToken ct;
        private MyArcFace element_MyArcFace;
        public MainWindow()
        {
            InitializeComponent();
            element_MyArcFace = new();
            face = new Image<Rgb24>[100];
            facepath = new string[100];
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
                facepath[Imgamm] = filename;
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
            if ((List1.SelectedIndex == -1) || (List2.SelectedIndex == -1))
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




                    bool mybl = true;
                    using var Lib = new LibraryContext();
                    var a = new BigInteger(System.IO.File.ReadAllBytes(facepath[List1.SelectedIndex]));
                    var myhash = a.GetHashCode();


                    var myFace1 = new myFace();
                    var mydata1 = new mydata();
                    mydata1.data = System.IO.File.ReadAllBytes(facepath[List1.SelectedIndex]);
                    myFace1.myImage = mydata1;
                    myFace1.Hash = myhash;
                    var floatArray = await element_MyArcFace.GetEmbeddings(face[List1.SelectedIndex], ct);
                    var byteArray = new byte[floatArray.Length * 4];
                    Buffer.BlockCopy(floatArray, 0, byteArray, 0, byteArray.Length);
                    myFace1.Embedding = byteArray;
                    var q = Lib.myFaces.Where(i => i.Hash == myhash).Include(x => x.myImage)
           .Where(i => Enumerable.SequenceEqual(i.myImage.data, mydata1.data))
     .FirstOrDefault();
                    if (q != null)
                    {
                        mybl = false;
                    }
                    if (mybl == true)
                    {
                        //Lib.mydatas.Add(mydata1);
                        Lib.myFaces.Add(myFace1);
                        var t = Lib.SaveChanges();
                    }
                    mybl = true;
                    var b = new BigInteger(System.IO.File.ReadAllBytes(facepath[List2.SelectedIndex]));
                    var myhash2 = b.GetHashCode();
                    var myFace2 = new myFace();
                    var mydata2 = new mydata();
                    mydata2.data = System.IO.File.ReadAllBytes(facepath[List2.SelectedIndex]);
                    myFace2.myImage = mydata2;
                    myFace2.Hash = myhash2;
                    var floatArray2 = await element_MyArcFace.GetEmbeddings(face[List2.SelectedIndex], ct);
                    var byteArray2 = new byte[floatArray2.Length * 4];
                    Buffer.BlockCopy(floatArray2, 0, byteArray2, 0, byteArray2.Length);
                    myFace2.Embedding = byteArray2;
                    var q1 = Lib.myFaces.Where(i => i.Hash == myhash2).Include(x => x.myImage)
           .Where(i => Enumerable.SequenceEqual(i.myImage.data, mydata2.data))
     .FirstOrDefault();
                    if (q1 != null)
                    {
                        mybl = false;
                    }
                    if (mybl == true)
                    {
                        //Lib.mydatas.Add(mydata1);
                        Lib.myFaces.Add(myFace2);
                        var t = Lib.SaveChanges();
                    }






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

        private void ButtonData_Click(object sender, RoutedEventArgs e)
        {
            Window1 W = new();
            W.ShowDialog();
        }
    }
}
