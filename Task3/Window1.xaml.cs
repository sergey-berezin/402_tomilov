using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Drawing;
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
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;

namespace Task3
{
    public partial class Window1 : Window
    {
        public ObservableCollection<myFace> myFaces1 { get; set; }
        public ObservableCollection<mydata> mydatas1 { get; set; }
        public ObservableCollection<System.Windows.Controls.Image> myFaces1Img { get; set; }
        public Window1()
        {
            myFaces1 = new ObservableCollection<myFace>();
            mydatas1 = new ObservableCollection<mydata>();
            myFaces1Img = new ObservableCollection<System.Windows.Controls.Image>();
            using var Lib = new LibraryContext();
            var a = Lib.myFaces;
            foreach (var b in a)
            {
                myFaces1.Add(b);
            }
            var c = Lib.mydatas;
            foreach (var b in c)
            {
                mydatas1.Add(b);
                var memoryStream = new MemoryStream(b.data);
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = memoryStream;
                 bitmap.EndInit();
                System.Windows.Controls.Image image = new()
                {
                    Source = bitmap
                };
                 myFaces1Img.Add(image);
            }
            InitializeComponent();
            this.DataContext = this;
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
                using var Lib = new LibraryContext();
                Lib.myFaces.Remove(myFaces1[List.SelectedIndex]);
                Lib.mydatas.Remove(mydatas1[List.SelectedIndex]);
                Lib.SaveChanges();
                myFaces1Img.RemoveAt(List.SelectedIndex);
        }
    }
}
