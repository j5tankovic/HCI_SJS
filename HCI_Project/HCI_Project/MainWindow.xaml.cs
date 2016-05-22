using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HCI_Project.Dijalozi;
using HCI_Project.NotBeans;
using System.Collections.ObjectModel;
using HCI_Project.Repos;

namespace HCI_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public RepoTipovi repoTipovi { get; set; }
        public RepoLokali repoLokali { get; set; }
        public RepoEtikete repoEtikete { get; set; }

        Image drag_image = null;
        Point _DragStart = new Point();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            repoTipovi = new RepoTipovi();
            repoLokali = new RepoLokali();
            repoEtikete = new RepoEtikete();
            popuniLokalima();
            //ObservableCollection<TipLokala> popunjeniTipovi = popuniLokalima();
            /*
            TipLokala s = new TipLokala() { Naziv = "Tip Lokala 1" };
            s.Lokali.Add(new Lokal() { Naziv = "Lokal 1" });
            s.Lokali.Add(new Lokal() { Naziv = "Lokal 2" });
            Tipovi.Add(s);
            s = new TipLokala() { Naziv = "Tip Lokala 2" };
            s.Lokali.Add(new Lokal() { Naziv = "Lokal 3" });
            s.Lokali.Add(new Lokal() { Naziv = "Lokal 4" });
            Tipovi.Add(s);
             * */


            Uri myUri = new Uri("../../map.jpg", UriKind.RelativeOrAbsolute);
            JpegBitmapDecoder decoder = new JpegBitmapDecoder(myUri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource = decoder.Frames[0];

            // Draw the Image
            myImage.Source = bitmapSource;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void Dodaj_Lokal(object sender, RoutedEventArgs e)
        {
            LokalDialog lokal = new LokalDialog(this);
            lokal.Closed += dialogLokalClosed;
            lokal.InitializeComponent();
            lokal.Show();
        }

        private void Dodaj_Tip(object sender, RoutedEventArgs e)
        {
            TipDialog tip = new TipDialog(this);
            //tip.Closed += dialogTipClosed;
            tip.InitializeComponent();
            tip.Show();
        }

        private void Dodaj_Etiketa(object sender, RoutedEventArgs e)
        {
            EtiketaDialog etiketa = new EtiketaDialog(this);
            etiketa.InitializeComponent();
            etiketa.Show();
        }

        private void Prikazi_Tabela(object sender, RoutedEventArgs e)
        {
            TabelaLokala tabela = new TabelaLokala(this);
            tabela.InitializeComponent();
            tabela.Show();
        }

        private void Prikazi_Tabelu_Tipova(object sender, RoutedEventArgs e)
        {
            TabelaTipova tabela = new TabelaTipova(this);
            tabela.Closed += tabelaTipovaClosed;
            tabela.InitializeComponent();
            tabela.Show();
        }

        private void Prikazi_Tabelu_Etiketa(object sender, RoutedEventArgs e)
        {
            TabelaEtiketa tabela = new TabelaEtiketa(this);
            tabela.InitializeComponent();
            tabela.Show();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs args)
        {
            myImage.Width = theScrollViewer.ViewportWidth;
            myImage.Height = theScrollViewer.ViewportHeight;
        }


        private ObservableCollection<TipLokala> popuniLokalima()
        {
            ObservableCollection<TipLokala> tipoviLokala = repoTipovi.sviTipovi();
            ObservableCollection<Lokal> lokali = repoLokali.sviLokali();
            foreach(var lokal in lokali)
            {
                foreach (TipLokala t in tipoviLokala)
                {
                    if (t.Oznaka.Equals(lokal.Tip.Oznaka))
                        t.Lokali.Add(lokal);
                }
            }

            return tipoviLokala;
        }

        private void dopuniTip(Lokal lokal)
        {
            try
            {
                (repoTipovi.tipovi.Single(x => x.Oznaka == lokal.Tip.Oznaka)).Lokali.Add(lokal);
            }
            catch (InvalidOperationException) { MessageBox.Show("Neuspelo, aaa!"); }
        }


        private void dialogTipClosed(object sender, EventArgs e)
        {
            ((TipDialog)sender).Closed -= dialogTipClosed;
            if (((TipDialog)sender).TipLokala != null)
                 repoTipovi.dodaj(((TipDialog)sender).TipLokala);

        }

        private void tabelaTipovaClosed(object sender, EventArgs e)
        {
            ((TabelaTipova)sender).Closed -= tabelaTipovaClosed;
            /*
            var itemsSource = ((TabelaTipova)sender).dgrMain.ItemsSource;
            List<TipLokala> tt = new List<TipLokala>();
            foreach (var i in itemsSource)
                tt.Add((TipLokala)i);
            //repoTipovi.tipovi.RemoveAll(item => !tt.Contains(item));
            //repoTipovi.tipovi =  repoTipovi.tipovi.Intersect(tt).ToList();
            repoTipovi.tipovi.Clear();
            foreach (TipLokala t in tt)
                repoTipovi.dodaj(t);
            foreach (TipLokala t in repoTipovi.tipovi)
            {
                if (!tt.Contains(t))
                    izbaciTip(t);
            }
            */
        }

        private void dialogLokalClosed(object sender, EventArgs e)
        {
            ((LokalDialog)sender).Closed -= dialogLokalClosed;
            if (((LokalDialog)sender).Lokal != null)
                 dopuniTip(((LokalDialog)sender).Lokal);

        }

        void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            repoEtikete.memorisi();
            repoLokali.memorisi();
            repoTipovi.memorisi();
        }

        private void izbaciTip(TipLokala t)
        {
            foreach (TipLokala tip in repoTipovi.tipovi)
            {
                if (tip.Oznaka.Equals(t.Oznaka))
                {
                    repoTipovi.izbaci(tip);
                    break;
                }
            }
        }

        public TipLokala nadjiTipLokala(Lokal l)
        {
            foreach (TipLokala tip in repoTipovi.tipovi)
            {
                if (tip.Oznaka.Equals(l.Tip.Oznaka))
                    return tip;
            }
            return null;
        }

        private void treeView1_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _DragStart = e.GetPosition(null);
        }

        private void treeView1_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = _DragStart - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
             Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance &&
             Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                TreeView treeView = sender as TreeView;
                TreeViewItem treeViewItem = FindAncestor<TreeViewItem>((DependencyObject)e.OriginalSource);
                if (treeViewItem != null)
                {
                    //Find its parent
                    ItemsControl parent = FindParent<ItemsControl>(treeViewItem);
                    //Get the bound object.
                    object item = parent.ItemContainerGenerator.ItemFromContainer(treeViewItem);
                    if (item != null)
                    {
                        if (item.GetType() == typeof(Lokal)) {
                            DataObject dragData = new DataObject("myFormat", (Lokal)item);
                            DragDrop.DoDragDrop(treeViewItem, dragData, DragDropEffects.Move);
                        }
                    }
                }
            }
        }

        //Get the parent of an item.
        private static T FindParent<T>(FrameworkElement current)
          where T : FrameworkElement
        {
            do
            {
                current = VisualTreeHelper.GetParent(current) as FrameworkElement;
                if (current is T)
                {
                    return (T)current;
                }
            }
            while (current != null);
            return null;
        }

        // Helper to search up the VisualTree
        private static T FindAncestor<T>(DependencyObject current)
          where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        private void Canvas_StartDrag(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Image)
            {
                mapa.CaptureMouse();


                drag_image = e.OriginalSource as Image;
                DragDrop.DoDragDrop(this, drag_image, DragDropEffects.Move);

            }
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Console.WriteLine("AAAAAAAA");
                Lokal lokal = e.Data.GetData("myFormat") as Lokal;

                Uri myUri = new Uri(lokal.Slika, UriKind.RelativeOrAbsolute);
                JpegBitmapDecoder decoder = new JpegBitmapDecoder(myUri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                BitmapSource bitmapSource = decoder.Frames[0];

                drag_image = new Image();
                drag_image.Source = bitmapSource;
                drag_image.Height = 32;
                drag_image.Width = 32;
                // Initialize the drag & drop operation
                mapa.Children.Add(drag_image);
                Point point = new Point();
                point = e.GetPosition(mapa);
                Canvas.SetLeft(drag_image, point.X);
                Canvas.SetTop(drag_image, point.Y);

                drag_image = null;
            }
            else {
                Point point = new Point();
                point = e.GetPosition(mapa);
                Canvas.SetLeft(drag_image, point.X);
                Canvas.SetTop(drag_image, point.Y);

                drag_image = null;
            }
            
        }

        private void Canvas_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }
    }
}
