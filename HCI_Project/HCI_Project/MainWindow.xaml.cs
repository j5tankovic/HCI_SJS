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
        TreeView treeView = null;
        TreeViewItem treeViewItem = null;
        Point _DragStart = new Point();
        Point _CanvasDragStart = new Point();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            repoTipovi = new RepoTipovi();
            repoLokali = new RepoLokali();
            repoEtikete = new RepoEtikete();
            popuniLokalima();
            initializeMap();
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
            lokal.ShowDialog();
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

        #region Drag & Drop

        private void treeView1_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _DragStart = e.GetPosition(null);
            treeView = sender as TreeView;
            treeViewItem = FindAncestor<TreeViewItem>((DependencyObject)e.OriginalSource);

        }

        private void treeView1_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = _DragStart - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
             Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance &&
             Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                
                if (treeViewItem != null)
                {

                    object help = treeView.ItemContainerGenerator.ItemFromContainer(treeViewItem);
                        
                    //Find its parent
                    ItemsControl parent = FindParent<ItemsControl>(treeViewItem);
                    //Get the bound object.

                    object item = parent.ItemContainerGenerator.ItemFromContainer(treeViewItem);
                    if (item != null)
                    {
                        if (item.GetType()==typeof(Lokal))
                        {
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
            _CanvasDragStart = e.GetPosition(null);
            
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Lokal lokal = e.Data.GetData("myFormat") as Lokal;
                if (MapaVecImaLokal(lokal))
                    return;
                string slika;
                if (lokal.Slika != null)
                    slika = lokal.Slika;
                else
                    slika = lokal.Tip.Slika;
                Uri myUri = new Uri(slika, UriKind.RelativeOrAbsolute);
                BitmapDecoder decoder;
                if (slika.EndsWith(".png"))
                {
                    decoder = new PngBitmapDecoder(myUri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                }
                else
                {
                    decoder = new JpegBitmapDecoder(myUri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                }
                BitmapSource bitmapSource = decoder.Frames[0];

                drag_image = new Image();
                drag_image.Source = bitmapSource;
                drag_image.Height = 32;
                drag_image.Width = 32;
                drag_image.DataContext = lokal;
                drag_image.MouseDown += imageDoubleClickHandler;
                // Initialize the drag & drop operation
                ContextMenu ctx = new ContextMenu();
                MenuItem m1 = new MenuItem();
                m1.DataContext = lokal;
                m1.Header = "Ukloni lokal sa mape";
                m1.Click += UkloniLokalIzMape;
                MenuItem m2 = new MenuItem();
                m2.DataContext = lokal;
                m2.Header = "Prikazi lokal";
                m2.Click += PrikaziLokal;
                ctx.Items.Add(m1);
                ctx.Items.Add(m2);
                drag_image.ContextMenu = ctx;

                mapa.Children.Add(drag_image);
                Point point = new Point();
                point = e.GetPosition(mapa);
                Canvas.SetLeft(drag_image, point.X);
                Canvas.SetTop(drag_image, point.Y);

                lokal.PozicijaX = point.X;
                lokal.PozicijaY = point.Y;

                drag_image = null;
                
            }
            else if (e.Data.GetDataPresent("canvasDND"))
            {
                Point point = new Point();
                point = e.GetPosition(mapa);
                Canvas.SetLeft(drag_image, point.X);
                Canvas.SetTop(drag_image, point.Y);

                Lokal lokal = (Lokal) drag_image.DataContext;

                lokal.PozicijaX = point.X;
                lokal.PozicijaY = point.Y;

                drag_image = null;
            }
            
        }

        private void Canvas_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") && !e.Data.GetDataPresent("canvasDND"))
            {
                
                e.Effects = DragDropEffects.None;
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            
            Point mousePos = e.GetPosition(null);
            Vector diff = _CanvasDragStart - mousePos;
            

            if (e.LeftButton == MouseButtonState.Pressed &&
             (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
             Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                
                if (e.OriginalSource is Image)
                {
                    Image img = (Image)e.OriginalSource;
                    
                    if (img != null)
                    {
                        
                        Lokal lokal = (Lokal)img.DataContext;
                        if (lokal != null)
                        {
                            
                            drag_image = img;
                            DataObject dragData = new DataObject("canvasDND", lokal);
                            DragDrop.DoDragDrop(img, dragData, DragDropEffects.Move);
                        }
                    }
                }
            }
        }

        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            if (drag_image != null)
            {
                Point point = new Point();
                point = e.GetPosition(mapa);
                Canvas.SetLeft(drag_image, point.X);
                Canvas.SetTop(drag_image, point.Y);
            }
        }

        #endregion

        private void imageDoubleClickHandler(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                Image img = (Image)sender;
                Lokal l = (Lokal)img.DataContext;
                IzmenaLokala dialog = new IzmenaLokala(this, l);
                dialog.InitializeComponent();
                dialog.ShowDialog();
            }
        }


        private bool MapaVecImaLokal(Lokal lokal)
        {
            foreach (UIElement e in mapa.Children)
            {
                Image img = (Image)e;
                if (img.DataContext != null)
                {
                    Lokal l = (Lokal)img.DataContext;
                    if (l.Oznaka.Equals(lokal.Oznaka))
                        return true;
                }
            }
            return false;
        }

        public void removeLokalFromMap(Lokal lokal)
        {
            foreach (UIElement e in mapa.Children)
            {
                Image img = (Image)e;
                if (img.DataContext != null)
                {
                    Lokal l = (Lokal)img.DataContext;
                    if (l.Oznaka.Equals(lokal.Oznaka))
                    {
                        mapa.Children.Remove(e);
                        lokal.PozicijaX = -1;
                        lokal.PozicijaY = -1;
                        return;
                    }
                }
            }
        }

        private void UkloniLokalIzMape(object sender, RoutedEventArgs args)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Da li ste sigurni?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                MenuItem m = (MenuItem)sender;
                removeLokalFromMap((Lokal)m.DataContext);
            }
        }

        private void PrikaziLokal(object sender, RoutedEventArgs args)
        {
            MenuItem m = (MenuItem)sender;
            Lokal l = (Lokal)m.DataContext;
            IzmenaLokala dialog = new IzmenaLokala(this, l);
            dialog.InitializeComponent();
            dialog.ShowDialog();
        }


        private void initializeMap()
        {
            foreach (Lokal lokal in repoLokali.lokali)
            {
                if (lokal.PozicijaX != -1 && lokal.PozicijaY != -1)
                {
                    string slika;
                    if (lokal.Slika != null)
                        slika = lokal.Slika;
                    else
                        slika = lokal.Tip.Slika;
                    Uri myUri = new Uri(slika, UriKind.RelativeOrAbsolute);
                    BitmapDecoder decoder;
                    if (slika.EndsWith(".png"))
                    {
                        decoder = new PngBitmapDecoder(myUri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    }
                    else {
                        decoder = new JpegBitmapDecoder(myUri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    }
                    
                    BitmapSource bitmapSource = decoder.Frames[0];

                    Image img = new Image();
                    img.Source = bitmapSource;
                    img.Height = 32;
                    img.Width = 32;
                    img.DataContext = lokal;
                    img.MouseDown += imageDoubleClickHandler;

                    ContextMenu ctx = new ContextMenu();
                    MenuItem m1 = new MenuItem();
                    m1.DataContext = lokal;
                    m1.Header = "Ukloni lokal sa mape";
                    m1.Click += UkloniLokalIzMape;
                    MenuItem m2 = new MenuItem();
                    m2.DataContext = lokal;
                    m2.Header = "Prikazi lokal";
                    m2.Click += PrikaziLokal;
                    ctx.Items.Add(m1);
                    ctx.Items.Add(m2);
                    img.ContextMenu = ctx;

                    mapa.Children.Add(img);
                    Canvas.SetLeft(img, lokal.PozicijaX);
                    Canvas.SetTop(img, lokal.PozicijaY);

                    
                }
            }
        }

    }
}
