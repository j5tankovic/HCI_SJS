﻿using System;
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
using HCI_Project.Komande;
using HCI_Project.Help;

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

        string active_map = "1";
        private static readonly int IMAGE_WIDTH = 32;
        private static readonly int IMAGE_HEIGHT = 32;
        private double DRAG_IMAGE_LEFT = -1;
        private double DRAG_IMAGE_TOP = -1;

        Image drag_image = null;
        TreeView treeView = null;
        TreeViewItem treeViewItem = null;
        Point _DragStart = new Point();
        Point _CanvasDragStart = new Point();

        private Lokal treeViewSelectedLokal;
        private Lokal working_lokal = new Lokal();
        private TipLokala treeViewSelectedTip;
        private TipLokala working_tip = new TipLokala();
        private int slc = 0; // 0 - nista; 1 - tip; 2 - lokal

        public MainWindow()
        {
            this.Resources.Add("working_lokal", working_lokal);
            this.Resources.Add("working_tip", working_tip);
            this.Resources.Add("working", null);
            InitializeComponent();
            this.DataContext = this;
            repoTipovi = new RepoTipovi();
            repoLokali = new RepoLokali();
            repoEtikete = new RepoEtikete();
            popuniLokalima();
            poveziEtikete();
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

            theScrollViewer2.Visibility = Visibility.Hidden;


            Uri myUri = new Uri("../../map.jpg", UriKind.RelativeOrAbsolute);
            JpegBitmapDecoder decoder = new JpegBitmapDecoder(myUri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource = decoder.Frames[0];

            // Draw the Image
            myImage.Source = bitmapSource;
            myImage14.Source = bitmapSource;

            Uri myUri2 = new Uri("../../map2.jpg", UriKind.RelativeOrAbsolute);
            JpegBitmapDecoder decoder2 = new JpegBitmapDecoder(myUri2, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource2 = decoder2.Frames[0];
            myImage24.Source = bitmapSource2;

            Uri myUri3 = new Uri("../../map3.jpg", UriKind.RelativeOrAbsolute);
            JpegBitmapDecoder decoder3 = new JpegBitmapDecoder(myUri3, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource3 = decoder3.Frames[0];
            myImage34.Source = bitmapSource3;

            Uri myUri4 = new Uri("../../map4.jpg", UriKind.RelativeOrAbsolute);
            JpegBitmapDecoder decoder4 = new JpegBitmapDecoder(myUri4, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource4 = decoder4.Frames[0];
            myImage44.Source = bitmapSource4;

            detaljiScroll.Visibility = Visibility.Hidden;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Dodaj_Lokal(object sender, RoutedEventArgs e)
        {
            LokalDialog lokal = new LokalDialog(this, null);
            lokal.Closed += dialogLokalClosed;
            lokal.InitializeComponent();
            lokal.ShowDialog();
        }

        private void Dodaj_Tip(object sender, RoutedEventArgs e)
        {
            TipDialog tip = new TipDialog(this, null);
            //tip.Closed += dialogTipClosed;
            tip.InitializeComponent();
            tip.ShowDialog();
        }

        private void Dodaj_Etiketa(object sender, RoutedEventArgs e)
        {
            EtiketaDialog etiketa = new EtiketaDialog(this);
            etiketa.InitializeComponent();
            etiketa.ShowDialog();
        }

        private void Prikazi_Tabela(object sender, RoutedEventArgs e)
        {
            TabelaLokala tabela = new TabelaLokala(this);
            tabela.InitializeComponent();
            tabela.ShowDialog();
        }

        private void Prikazi_Tabelu_Tipova(object sender, RoutedEventArgs e)
        {
            TabelaTipova tabela = new TabelaTipova(this);
            tabela.Closed += tabelaTipovaClosed;
            tabela.InitializeComponent();
            tabela.ShowDialog();
        }

        private void Prikazi_Tabelu_Etiketa(object sender, RoutedEventArgs e)
        {
            TabelaEtiketa tabela = new TabelaEtiketa(this);
            tabela.InitializeComponent();
            tabela.ShowDialog();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs args)
        {
            myImage.Width = theScrollViewer.ViewportWidth;
            myImage.Height = theScrollViewer.ViewportHeight;
        }


        private void popuniLokalima()
        {
            foreach (Lokal lokal in repoLokali.sviLokali())
            {
                foreach (TipLokala t in repoTipovi.sviTipovi())
                {
                    if (t.Oznaka.Equals(lokal.Tip.Oznaka))
                    {
                        t.ubaciLokal(lokal);
                        lokal.Tip = t;
                    }
                }
            }
        }

        private void poveziEtikete()
        {
            foreach (Etiketa e in repoEtikete.sveEtikete())
            {
                repoLokali.poveziEtikete(e);
            }
        }

        public void dopuniTip(Lokal lokal)
        {
            try
            {

                (repoTipovi.tipovi.Single(x => x.Oznaka == lokal.Tip.Oznaka)).ubaciLokal(lokal);
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
                if (MapaVecImaLokal(lokal, mapa))
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Lokal pod nazivom '"+lokal.Naziv+"' se vec nalazi na mapi!", "Oprez", System.Windows.MessageBoxButton.OK);
                    return;
                }
  
                drag_image = new Image();
                Binding bind = new Binding("Slika");
                bind.Source = lokal;
                bind.Mode = BindingMode.OneWay;
                drag_image.SetBinding(Image.SourceProperty, bind);
                drag_image.Height = IMAGE_HEIGHT;
                drag_image.Width = IMAGE_WIDTH;
                drag_image.DataContext = lokal;
                drag_image.MouseDown += imageDoubleClickHandler;
                ToolTip tt = new ToolTip();
                tt.Content = lokal.Oznaka + " " + lokal.Naziv;
                drag_image.ToolTip = tt;
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
                ctx.Items.Add(m2);
                for (int i = 1; i < 5; i++) {
                    if (i != Int32.Parse(active_map))
                    {
                        MenuItem m = new MenuItem();
                        m.DataContext = lokal;
                        m.Tag = i;
                        m.Header = "Premesti lokal na mapu " + i;
                        m.Click += PremestiLokal;
                        ctx.Items.Add(m);
                    }
                }
                ctx.Items.Add(m1);
                drag_image.ContextMenu = ctx;
                Point point = new Point();
                point = e.GetPosition(mapa);
                if (preklapaSe(point, mapa, null))
                    return;

                mapa.Children.Add(drag_image);
                
                Canvas.SetLeft(drag_image, point.X);
                Canvas.SetTop(drag_image, point.Y);

                lokal.PozicijaX = point.X;
                lokal.PozicijaY = point.Y;
                lokal.Mapa = active_map;

                drag_image = null;
                
            }
            else if (e.Data.GetDataPresent("canvasDND"))
            {
                Point point = new Point();
                point = e.GetPosition(mapa);
                if (preklapaSe(point, mapa, drag_image))
                {
                    Canvas.SetLeft(drag_image, ((Lokal)drag_image.DataContext).PozicijaX);
                    Canvas.SetTop(drag_image, ((Lokal)drag_image.DataContext).PozicijaY);
                    return;
                }
                Canvas.SetLeft(drag_image, point.X);
                Canvas.SetTop(drag_image, point.Y);

                Lokal lokal = (Lokal) drag_image.DataContext;

                lokal.PozicijaX = point.X;
                lokal.PozicijaY = point.Y;

                drag_image = null;
            }
            
        }

        private void Canvas_Drop14(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Lokal lokal = e.Data.GetData("myFormat") as Lokal;
                if (MapaVecImaLokal(lokal, mapa14))
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Lokal pod nazivom '" + lokal.Naziv + "' se vec nalazi na mapi!", "Oprez", System.Windows.MessageBoxButton.OK);
                    return;
                }

                DropFunction(lokal, e, 1, mapa14);
            }
            else if (e.Data.GetDataPresent("canvasDND"))
            {
                Lokal lokal = (Lokal)drag_image.DataContext;
                if (!MapaVecImaLokal(lokal, mapa14))
                {
                    DropFunction(lokal, e, 1, mapa14);
                    return;
                }
                Point point2 = new Point();
                point2 = e.GetPosition(mapa14);
                if (preklapaSe(point2, mapa14, drag_image))
                {
                    Canvas.SetLeft(drag_image, ((Lokal)drag_image.DataContext).PozicijaX);
                    Canvas.SetTop(drag_image, ((Lokal)drag_image.DataContext).PozicijaY);
                    return;
                }

                Canvas.SetLeft(drag_image, point2.X);
                Canvas.SetTop(drag_image, point2.Y);

                lokal.PozicijaX = point2.X;
                lokal.PozicijaY = point2.Y;

                drag_image = null;
            }

        }

        private void Canvas_Drop24(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Lokal lokal = e.Data.GetData("myFormat") as Lokal;
                if (MapaVecImaLokal(lokal, mapa24))
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Lokal pod nazivom '" + lokal.Naziv + "' se vec nalazi na mapi!", "Oprez", System.Windows.MessageBoxButton.OK);
                    return;
                }

                DropFunction(lokal, e, 2, mapa24);
            }
            else if (e.Data.GetDataPresent("canvasDND"))
            {
                Lokal lokal = (Lokal)drag_image.DataContext;
                if (!MapaVecImaLokal(lokal, mapa24)) {
                    DropFunction(lokal, e, 2, mapa24);
                    return;
                }
                Point point2 = new Point();
                point2 = e.GetPosition(mapa24);
                if (preklapaSe(point2, mapa24, drag_image))
                {
                    Canvas.SetLeft(drag_image, _CanvasDragStart.X);
                    Canvas.SetTop(drag_image, _CanvasDragStart.Y);
                    return;
                }

                Canvas.SetLeft(drag_image, point2.X);
                Canvas.SetTop(drag_image, point2.Y);

                lokal.PozicijaX = point2.X;
                lokal.PozicijaY = point2.Y;

                drag_image = null;
            }

        }

        private void Canvas_Drop34(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Lokal lokal = e.Data.GetData("myFormat") as Lokal;
                if (MapaVecImaLokal(lokal, mapa34))
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Lokal pod nazivom '" + lokal.Naziv + "' se vec nalazi na mapi!", "Oprez", System.Windows.MessageBoxButton.OK);
                    return;
                }

                DropFunction(lokal, e, 3, mapa34);
            }
            else if (e.Data.GetDataPresent("canvasDND"))
            {
                Lokal lokal = (Lokal)drag_image.DataContext;
                if (!MapaVecImaLokal(lokal, mapa34))
                {
                    DropFunction(lokal, e, 3, mapa34);
                    return;
                }
                Point point2 = new Point();
                point2 = e.GetPosition(mapa34);
                if (preklapaSe(point2, mapa34, drag_image))
                {
                    Canvas.SetLeft(drag_image, ((Lokal)drag_image.DataContext).PozicijaX);
                    Canvas.SetTop(drag_image, ((Lokal)drag_image.DataContext).PozicijaY);
                    return;
                }

                Canvas.SetLeft(drag_image, point2.X);
                Canvas.SetTop(drag_image, point2.Y);

                lokal.PozicijaX = point2.X;
                lokal.PozicijaY = point2.Y;

                drag_image = null;
            }

        }

        private void Canvas_Drop44(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Lokal lokal = e.Data.GetData("myFormat") as Lokal;
                if (MapaVecImaLokal(lokal, mapa44))
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Lokal pod nazivom '" + lokal.Naziv + "' se vec nalazi na mapi!", "Oprez", System.Windows.MessageBoxButton.OK);
                    return;
                }
                DropFunction(lokal, e, 4, mapa44);
                
            }
            else if (e.Data.GetDataPresent("canvasDND"))
            {
                Lokal lokal = (Lokal)drag_image.DataContext;
                if (!MapaVecImaLokal(lokal, mapa44))
                {
                    DropFunction(lokal, e, 4, mapa44);
                    return;
                }
                Point point2 = new Point();
                point2 = e.GetPosition(mapa44);
                if (preklapaSe(point2, mapa44, drag_image))
                {
                    Canvas.SetLeft(drag_image, ((Lokal)drag_image.DataContext).PozicijaX);
                    Canvas.SetTop(drag_image, ((Lokal)drag_image.DataContext).PozicijaY);
                    return;
                }

                Canvas.SetLeft(drag_image, point2.X);
                Canvas.SetTop(drag_image, point2.Y);

                lokal.PozicijaX = point2.X;
                lokal.PozicijaY = point2.Y;

                drag_image = null;
            }

        }

        private void DropFunction(Lokal lokal, DragEventArgs e, int map, Canvas mapa)
        {
            drag_image = new Image();
            Binding bind = new Binding("Slika");
            bind.Source = lokal;
            bind.Mode = BindingMode.OneWay;
            drag_image.SetBinding(Image.SourceProperty, bind);
            drag_image.Height = IMAGE_HEIGHT;
            drag_image.Width = IMAGE_WIDTH;
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
            ctx.Items.Add(m2);
            for (int i = 1; i < 5; i++)
            {
                if (i != map)
                {
                    MenuItem m = new MenuItem();
                    m.DataContext = lokal;
                    m.Tag = i;
                    m.Header = "Premesti lokal na mapu " + i;
                    m.Click += PremestiLokal;
                    ctx.Items.Add(m);
                }
            }
            ctx.Items.Add(m1);
            drag_image.ContextMenu = ctx;
            Point point = new Point();
            point = e.GetPosition(mapa);
            if (preklapaSe(point, mapa, null))
                return;

            mapa.Children.Add(drag_image);

            Canvas.SetLeft(drag_image, point.X);
            Canvas.SetTop(drag_image, point.Y);

            lokal.PozicijaX = point.X;
            lokal.PozicijaY = point.Y;
            lokal.Mapa = map.ToString();

            drag_image = null;
            ObrisiDecu();
            initializeMap();
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
                            DRAG_IMAGE_LEFT = Canvas.GetLeft(drag_image);
                            DRAG_IMAGE_TOP = Canvas.GetTop(drag_image);
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

        private void Canvas_DragOver14(object sender, DragEventArgs e)
        {
            if (drag_image != null)
            {
                Lokal lokal = (Lokal)drag_image.DataContext;
                if (MapaVecImaLokal(lokal, mapa14))
                {
                    Point point = new Point();
                    point = e.GetPosition(mapa14);
                    Canvas.SetLeft(drag_image, point.X);
                    Canvas.SetTop(drag_image, point.Y);
                }
            }
        }

        private void Canvas_DragOver24(object sender, DragEventArgs e)
        {
            if (drag_image != null)
            {
                Lokal lokal = (Lokal)drag_image.DataContext;
                if (MapaVecImaLokal(lokal, mapa24))
                {
                    Point point = new Point();
                    point = e.GetPosition(mapa24);
                    Canvas.SetLeft(drag_image, point.X);
                    Canvas.SetTop(drag_image, point.Y);
                }
            }
        }

        private void Canvas_DragOver34(object sender, DragEventArgs e)
        {
            if (drag_image != null)
            {
                Lokal lokal = (Lokal)drag_image.DataContext;
                if (MapaVecImaLokal(lokal, mapa34))
                {
                    Point point = new Point();
                    point = e.GetPosition(mapa34);
                    Canvas.SetLeft(drag_image, point.X);
                    Canvas.SetTop(drag_image, point.Y);
                }
            }
        }

        private void Canvas_DragOver44(object sender, DragEventArgs e)
        {
            if (drag_image != null)
            {
                Lokal lokal = (Lokal)drag_image.DataContext;
                if (MapaVecImaLokal(lokal,mapa44))
                {
                    Point point = new Point();
                    point = e.GetPosition(mapa44);
                    Canvas.SetLeft(drag_image, point.X);
                    Canvas.SetTop(drag_image, point.Y);
                }
            }
        }


        #endregion

        private void treeView1_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            treeView = sender as TreeView;
            treeViewItem = FindAncestor<TreeViewItem>((DependencyObject)e.OriginalSource);

            if (treeViewItem != null)
            {

                object help = treeView.ItemContainerGenerator.ItemFromContainer(treeViewItem);

                //Find its parent
                ItemsControl parent = FindParent<ItemsControl>(treeViewItem);
                //Get the bound object.

                object item = parent.ItemContainerGenerator.ItemFromContainer(treeViewItem);
                if (item != null)
                {
                    if (item.GetType() == typeof(Lokal))
                    {
                        Lokal l = (Lokal)item;
                        LokalDialog dialog = new LokalDialog(this, l);
                        dialog.InitializeComponent();
                        dialog.ShowDialog();
                    }
                    else if (item.GetType() == typeof(TipLokala))
                    {
                        TipLokala l = (TipLokala)item;
                        TipDialog dialog = new TipDialog(this, l);
                        dialog.InitializeComponent();
                        dialog.ShowDialog();
                    }

                }
            }

        }

        private void TreeViewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                object item = treeView.SelectedItem;
                if (item == null)
                    return;
                        if (item.GetType() == typeof(Lokal))
                        {
                            Lokal l = (Lokal)item;
                            LokalDialog dialog = new LokalDialog(this, l);
                            dialog.InitializeComponent();
                            dialog.ShowDialog();
                        }
                        else if (item.GetType() == typeof(TipLokala))
                        {
                            TipLokala l = (TipLokala)item;
                            TipDialog dialog = new TipDialog(this, l);
                            dialog.InitializeComponent();
                            dialog.ShowDialog();
                        }

                    
            }
        }

        private void imageDoubleClickHandler(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                Image img = (Image)sender;
                Lokal l = (Lokal)img.DataContext;
                LokalDialog dialog = new LokalDialog(this, l);
                dialog.InitializeComponent();
                dialog.ShowDialog();
            }
        }


        private bool MapaVecImaLokal(Lokal lokal, Canvas mapa)
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
            Canvas mapa2;
            if(lokal.Mapa=="1")
                 mapa2 = mapa14;
            else if(lokal.Mapa=="2")
                 mapa2 = mapa24;
            else if(lokal.Mapa=="3")
                 mapa2 = mapa34;
            else
                 mapa2 = mapa44;

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

            foreach (UIElement e in mapa2.Children)
            {
                Image img = (Image)e;
                if (img.DataContext != null)
                {
                    Lokal l = (Lokal)img.DataContext;
                    if (l.Oznaka.Equals(lokal.Oznaka))
                    {
                        mapa2.Children.Remove(e);
                        lokal.PozicijaX = -1;
                        lokal.PozicijaY = -1;
                        return;
                    }
                }
            }
        }

        private void ObrisiLokal(object sender, RoutedEventArgs args)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Da li ste sigurni?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                MenuItem m = (MenuItem)sender;
                Lokal lokal = (Lokal)m.DataContext;
                if (lokal == null)
                    return;
                removeLokalFromMap(lokal);
                repoLokali.izbaci(lokal);
                TipLokala tip = nadjiTipLokala(lokal);
                if (tip != null)
                    tip.izbaciLokal(lokal);
            }
        }

        private void ObrisiTip(object sender, RoutedEventArgs args)
        {  
            MenuItem m = (MenuItem)sender;
            TipLokala tip = (TipLokala)m.DataContext;
            if (tip == null)
                return;
            BrisanjeTipa dialog = new BrisanjeTipa(this, tip);
            dialog.ShowDialog();

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
            LokalDialog dialog = new LokalDialog(this, l);
            dialog.InitializeComponent();
            dialog.ShowDialog();
        }

        private void PrikaziTip(object sender, RoutedEventArgs args)
        {
            MenuItem m = (MenuItem)sender;
            TipLokala l = (TipLokala)m.DataContext;
            TipDialog dialog = new TipDialog(this, l);
            dialog.InitializeComponent();
            dialog.ShowDialog();
        }

        private void PremestiLokal(object sender, RoutedEventArgs args)
        {
            MenuItem m = (MenuItem)sender;
            int mapa = (int)m.Tag;
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Da li ste sigurni da zelite da premestite lokal na mapu "+mapa+"? \nLokal ce se nalaziti na istoj poziciji kao na trenutnoj mapi!", "Premestanje lokala", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Lokal l = (Lokal)m.DataContext;
                l.Mapa = mapa.ToString();
                ObrisiDecu();
                initializeMap();
                MessageBoxResult messageBox = System.Windows.MessageBox.Show("Lokal uspresno premesten na mapu "+mapa+"!", "Lokal premesten", System.Windows.MessageBoxButton.OK);
            }
        }

        private void initializeMap()
        {
            foreach (Lokal lokal in repoLokali.lokali)
            {
                if (lokal.PozicijaX != -1 && lokal.PozicijaY != -1 && lokal.Mapa == active_map)
                {
                    Image img = new Image();
                    Binding bind = new Binding("Slika");
                    bind.Source = lokal;
                    bind.Mode = BindingMode.OneWay;
                    img.SetBinding(Image.SourceProperty, bind);
                    img.Height = IMAGE_HEIGHT;
                    img.Width = IMAGE_WIDTH;
                    img.DataContext = lokal;
                    img.MouseDown += imageDoubleClickHandler;
                    ToolTip tt = new ToolTip();
                    tt.Content = lokal.Oznaka + " " + lokal.Naziv;
                    img.ToolTip = tt;

                    ContextMenu ctx = new ContextMenu();
                    MenuItem m1 = new MenuItem();
                    m1.DataContext = lokal;
                    m1.Header = "Ukloni lokal sa mape";
                    m1.Click += UkloniLokalIzMape;
                    MenuItem m2 = new MenuItem();
                    m2.DataContext = lokal;
                    m2.Header = "Prikazi lokal";
                    m2.Click += PrikaziLokal;
                    ctx.Items.Add(m2);
                    for (int i = 1; i < 5; i++)
                    {
                        if (i != Int32.Parse(active_map))
                        {
                            MenuItem m = new MenuItem();
                            m.DataContext = lokal;
                            m.Tag = i;
                            m.Header = "Premesti lokal na mapu " + i;
                            m.Click += PremestiLokal;
                            ctx.Items.Add(m);
                        }
                    }
                    ctx.Items.Add(m1);
                    img.ContextMenu = ctx;

                    mapa.Children.Add(img);
                    Canvas.SetLeft(img, lokal.PozicijaX);
                    Canvas.SetTop(img, lokal.PozicijaY);
                }
            }

            foreach (Lokal lokal in repoLokali.lokali)
            {
                if (lokal.PozicijaX != -1 && lokal.PozicijaY != -1 && lokal.Mapa == "1")
                {
                    putImageOnMap(lokal, 1, mapa14);
                }

                else if (lokal.PozicijaX != -1 && lokal.PozicijaY != -1 && lokal.Mapa == "2")
                {
                    putImageOnMap(lokal, 2, mapa24);
                }

                else if (lokal.PozicijaX != -1 && lokal.PozicijaY != -1 && lokal.Mapa == "3")
                {
                    putImageOnMap(lokal, 3, mapa34);
                }

                else  if (lokal.PozicijaX != -1 && lokal.PozicijaY != -1 && lokal.Mapa == "4")
                {
                    putImageOnMap(lokal, 4, mapa44);
                }
            }
        }

        private void initializeMapWithFilter(string text)
        {
            string[] words = text.Split(' ');
            foreach (Lokal lokal in repoLokali.lokali)
            {
                if (words.Any(word => lokal.Oznaka.ToUpper().Contains(word.ToUpper()) || lokal.Naziv.ToUpper().Contains(word.ToUpper()) ||
                            (lokal.Opis != null && lokal.Opis.ToUpper().Contains(word.ToUpper())) ||
                            lokal.Tip.Naziv.ToUpper().Contains(word.ToUpper())))
                {
                    if (lokal.PozicijaX != -1 && lokal.PozicijaY != -1 && lokal.Mapa == active_map)
                    {
                        Image img = new Image();
                        Binding bind = new Binding("Slika");
                        bind.Source = lokal;
                        bind.Mode = BindingMode.OneWay;
                        img.SetBinding(Image.SourceProperty, bind);
                        img.Height = 32;
                        img.Width = 32;
                        img.DataContext = lokal;
                        img.MouseDown += imageDoubleClickHandler;
                        ToolTip tt = new ToolTip();
                        tt.Content = lokal.Oznaka + " " + lokal.Naziv;
                        img.ToolTip = tt;

                        ContextMenu ctx = new ContextMenu();
                        MenuItem m1 = new MenuItem();
                        m1.DataContext = lokal;
                        m1.Header = "Ukloni lokal sa mape";
                        m1.Click += UkloniLokalIzMape;
                        MenuItem m2 = new MenuItem();
                        m2.DataContext = lokal;
                        m2.Header = "Prikazi lokal";
                        m2.Click += PrikaziLokal;
                        ctx.Items.Add(m2);
                        for (int i = 1; i < 5; i++)
                        {
                            if (i != Int32.Parse(active_map))
                            {
                                MenuItem m = new MenuItem();
                                m.DataContext = lokal;
                                m.Tag = i;
                                m.Header = "Premesti lokal na mapu " + i;
                                m.Click += PremestiLokal;
                                ctx.Items.Add(m);
                            }
                        }
                        ctx.Items.Add(m1);
                        img.ContextMenu = ctx;

                        mapa.Children.Add(img);
                        Canvas.SetLeft(img, lokal.PozicijaX);
                        Canvas.SetTop(img, lokal.PozicijaY);
                    }
                }
            }

            foreach (Lokal lokal in repoLokali.lokali)
            {
                if (words.Any(word => lokal.Oznaka.ToUpper().Contains(word.ToUpper()) || lokal.Naziv.ToUpper().Contains(word.ToUpper()) ||
                             (lokal.Opis != null && lokal.Opis.ToUpper().Contains(word.ToUpper())) ||
                            lokal.Tip.Naziv.ToUpper().Contains(word.ToUpper())))
                {
                    if (lokal.PozicijaX != -1 && lokal.PozicijaY != -1 && lokal.Mapa == "1")
                    {
                        putImageOnMap(lokal, 1, mapa14);
                    }

                    else if (lokal.PozicijaX != -1 && lokal.PozicijaY != -1 && lokal.Mapa == "2")
                    {
                        putImageOnMap(lokal, 2, mapa24);
                    }

                    else if (lokal.PozicijaX != -1 && lokal.PozicijaY != -1 && lokal.Mapa == "3")
                    {
                        putImageOnMap(lokal, 3, mapa34);
                    }

                    else if (lokal.PozicijaX != -1 && lokal.PozicijaY != -1 && lokal.Mapa == "4")
                    {
                        putImageOnMap(lokal, 4, mapa44);
                    }
                }
            }
        }

        private void putImageOnMap(Lokal lokal, int map, Canvas mapa)
        {
            Image img = new Image();
            Binding bind = new Binding("Slika");
            bind.Source = lokal;
            bind.Mode = BindingMode.OneWay;
            img.SetBinding(Image.SourceProperty, bind);
            img.Height = IMAGE_HEIGHT;
            img.Width = IMAGE_WIDTH;
            img.DataContext = lokal;
            img.MouseDown += imageDoubleClickHandler;
            ToolTip tt = new ToolTip();
            tt.Content = lokal.Oznaka + " " + lokal.Naziv;
            img.ToolTip = tt;

            ContextMenu ctx = new ContextMenu();
            MenuItem m1 = new MenuItem();
            m1.DataContext = lokal;
            m1.Header = "Ukloni lokal sa mape";
            m1.Click += UkloniLokalIzMape;
            MenuItem m2 = new MenuItem();
            m2.DataContext = lokal;
            m2.Header = "Prikazi lokal";
            m2.Click += PrikaziLokal;
            ctx.Items.Add(m2);
            for (int i = 1; i < 5; i++)
            {
                if (i != map)
                {
                    MenuItem m = new MenuItem();
                    m.DataContext = lokal;
                    m.Tag = i;
                    m.Header = "Premesti lokal na mapu " + i;
                    m.Click += PremestiLokal;
                    ctx.Items.Add(m);
                }
            }
            ctx.Items.Add(m1);
            img.ContextMenu = ctx;

            mapa.Children.Add(img);
            Canvas.SetLeft(img, lokal.PozicijaX);
            Canvas.SetTop(img, lokal.PozicijaY);
        }

        private void Promeni_Mapu(object sender, RoutedEventArgs e)
        {
            string myValue = "";
            if (sender.GetType() == typeof(Button))
                myValue = (string)((Button)sender).Tag;
            else if (sender.GetType() == typeof(MenuItem))
                myValue = (string)((MenuItem)sender).Tag;

            changeMap(myValue);
        }

        private void changeMap(string myValue)
        {
            Uri myUri;

            ObrisiDecu();
            filter.Text = "";
            pretraga.Text = "";

            if (myValue == "1")
            {
                myUri = new Uri("../../map.jpg", UriKind.RelativeOrAbsolute);
                active_map = "1";
                theScrollViewer.Visibility = Visibility.Visible;
                theScrollViewer2.Visibility = Visibility.Hidden;
            }
            else if (myValue == "2")
            {
                myUri = new Uri("../../map2.jpg", UriKind.RelativeOrAbsolute);
                active_map = "2";
                theScrollViewer.Visibility = Visibility.Visible;
                theScrollViewer2.Visibility = Visibility.Hidden;
            }
            else if (myValue == "3")
            {
                myUri = new Uri("../../map3.jpg", UriKind.RelativeOrAbsolute);
                active_map = "3";
                theScrollViewer.Visibility = Visibility.Visible;
                theScrollViewer2.Visibility = Visibility.Hidden;
            }
            else if (myValue == "4")
            {
                myUri = new Uri("../../map4.jpg", UriKind.RelativeOrAbsolute);
                active_map = "4";
                theScrollViewer.Visibility = Visibility.Visible;
                theScrollViewer2.Visibility = Visibility.Hidden;
            }
            else
            {
                active_map = "0";
                theScrollViewer.Visibility = Visibility.Hidden;
                theScrollViewer2.Visibility = Visibility.Visible;
                initializeMap();
                return;
            }
            JpegBitmapDecoder decoder = new JpegBitmapDecoder(myUri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource = decoder.Frames[0];
            myImage.Source = bitmapSource;
            initializeMap();
        }

        private void ObrisiDecu() {
            for (int i = mapa.Children.Count - 1; i >= 0; i--)
            {
                mapa.Children.RemoveAt(i);
            }

            for (int i = mapa14.Children.Count - 1; i >= 0; i--)
            {
                mapa14.Children.RemoveAt(i);
            }

            for (int i = mapa24.Children.Count - 1; i >= 0; i--)
            {
                mapa24.Children.RemoveAt(i);
            }

            for (int i = mapa34.Children.Count - 1; i >= 0; i--)
            {
                mapa34.Children.RemoveAt(i);
            }

            for (int i = mapa44.Children.Count - 1; i >= 0; i--)
            {
                mapa44.Children.RemoveAt(i);
            }
        
        }
        
        private void Filter(object sender, RoutedEventArgs e)
        {
            string text = filter.Text;
            if (text != "") {
                ObrisiDecu();
                initializeMapWithFilter(text);
            }
        }

        private void RemoveFilter(object sender, RoutedEventArgs e)
        {
            ObrisiDecu();
            initializeMap();
            filter.Text = "";
        }

        private void Pretrazi(object sender, RoutedEventArgs e)
        {  
           string text = pretraga.Text;
           string[] words = text.Split(' ');
           if (text != "")
           {
               foreach (UIElement i in mapa.Children)
               {
                   Image img = (Image)i;
                   if (img.DataContext != null)
                   {
                       Lokal lokal = (Lokal)img.DataContext;
                       if (words.Any(word => lokal.Oznaka.ToUpper().Contains(word.ToUpper()) || lokal.Naziv.ToUpper().Contains(word.ToUpper()) ||
                           (lokal.Opis != null && lokal.Opis.ToUpper().Contains(word.ToUpper())) ||
                            lokal.Tip.Naziv.ToUpper().Contains(word.ToUpper())))
                       {
                           theScrollViewer.ScrollToHorizontalOffset(lokal.PozicijaX);
                           theScrollViewer.ScrollToVerticalOffset(lokal.PozicijaY);
                        
                       }
                   }
               }
               foreach (UIElement i in mapa14.Children)
               {
                   Image img = (Image)i;
                   if (img.DataContext != null)
                   {
                       Lokal lokal = (Lokal)img.DataContext;
                       if (words.Any(word => lokal.Oznaka.ToUpper().Contains(word.ToUpper()) || lokal.Naziv.ToUpper().Contains(word.ToUpper()) ||
                             (lokal.Opis != null && lokal.Opis.ToUpper().Contains(word.ToUpper())) ||
                            lokal.Tip.Naziv.ToUpper().Contains(word.ToUpper())))
                       {
                           theScrollViewer14.ScrollToHorizontalOffset(lokal.PozicijaX);
                           theScrollViewer14.ScrollToVerticalOffset(lokal.PozicijaY);
                       }
                   }
               }
               foreach (UIElement i in mapa24.Children)
               {
                   Image img = (Image)i;
                   if (img.DataContext != null)
                   {
                       Lokal lokal = (Lokal)img.DataContext;
                       if (words.Any(word => lokal.Oznaka.ToUpper().Contains(word.ToUpper()) || lokal.Naziv.ToUpper().Contains(word.ToUpper()) ||
                            (lokal.Opis != null && lokal.Opis.ToUpper().Contains(word.ToUpper())) ||
                            lokal.Tip.Naziv.ToUpper().Contains(word.ToUpper())))
                       {
                           theScrollViewer24.ScrollToHorizontalOffset(lokal.PozicijaX);
                           theScrollViewer24.ScrollToVerticalOffset(lokal.PozicijaY);
                       }
                   }
               } foreach (UIElement i in mapa34.Children)
               {
                   Image img = (Image)i;
                   if (img.DataContext != null)
                   {
                       Lokal lokal = (Lokal)img.DataContext;
                       if (words.Any(word => lokal.Oznaka.ToUpper().Contains(word.ToUpper()) || lokal.Naziv.ToUpper().Contains(word.ToUpper()) ||
                             (lokal.Opis != null && lokal.Opis.ToUpper().Contains(word.ToUpper())) ||
                            lokal.Tip.Naziv.ToUpper().Contains(word.ToUpper())))
                       {
                           theScrollViewer34.ScrollToHorizontalOffset(lokal.PozicijaX);
                           theScrollViewer34.ScrollToVerticalOffset(lokal.PozicijaY);
                       }
                   }
               } foreach (UIElement i in mapa44.Children)
               {
                   Image img = (Image)i;
                   if (img.DataContext != null)
                   {
                       Lokal lokal = (Lokal)img.DataContext;
                       if (words.Any(word => lokal.Oznaka.ToUpper().Contains(word.ToUpper()) || lokal.Naziv.ToUpper().Contains(word.ToUpper()) ||
                             (lokal.Opis != null && lokal.Opis.ToUpper().Contains(word.ToUpper())) ||
                            lokal.Tip.Naziv.ToUpper().Contains(word.ToUpper())))
                       {
                           theScrollViewer44.ScrollToHorizontalOffset(lokal.PozicijaX);
                           theScrollViewer44.ScrollToVerticalOffset(lokal.PozicijaY);
                       }
                   }
               }
           }

        }

        private bool preklapaSe(Point new_point, Canvas mapa, Image self)
        {
            foreach (UIElement i in mapa.Children)
            {
                if (self == i)
                    continue;
                Image img = (Image)i;
                double left = Canvas.GetLeft(i);
                double top = Canvas.GetTop(i);
                if (left - img.ActualWidth < new_point.X && left + img.ActualWidth >= new_point.X
                    && top - img.ActualHeight < new_point.Y && top + img.ActualHeight >= new_point.Y)
                    return true;
            }
            return false;
        }
        
        private void SaveLokal(object sender, RoutedEventArgs args)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Da li ste sigurni da zelite da sacuvate izmene?", "Izmena lokala", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                
                treeViewSelectedLokal.Naziv = working_lokal.Naziv;
                treeViewSelectedLokal.Opis = working_lokal.Opis;
                treeViewSelectedLokal.Kapacitet = working_lokal.Kapacitet;
                treeViewSelectedLokal.Datum = working_lokal.Datum;
                System.Windows.MessageBox.Show("Izmena uspesna!", "Izmena lokala");
                
            }
            
        }

        private void SaveTip(object sender, RoutedEventArgs args)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Da li ste sigurni da zelite da sacuvate izmene?", "Izmena tipa", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {

                treeViewSelectedTip.setTipAs(working_tip);
                System.Windows.MessageBox.Show("Izmena uspesna!", "Izmena tipa");
            }
        }
        
        private void TreeViewSelectionChanged(object sender, RoutedPropertyChangedEventArgs<Object> args)
        {
            //treeView = sender as TreeView;
            //treeViewItem = FindAncestor<TreeViewItem>((DependencyObject)args.OriginalSource);
            object item = treeView.SelectedItem;
            

                //object help = treeView.ItemContainerGenerator.ItemFromContainer(treeViewItem);

                //Find its parent
                //ItemsControl parent = FindParent<ItemsControl>(treeViewItem);
                //Get the bound object.
                //if (parent == null)
                    //return;
                //object item = parent.ItemContainerGenerator.ItemFromContainer(treeViewItem);
                if (item != null)
                {
                    if (item.GetType() == typeof(Lokal))
                    {
                        detaljiScroll.Visibility = Visibility.Visible;
                        treeViewSelectedLokal = (Lokal)item;
                        working_lokal.setValuesAs(Lokal.getCopyLokal(treeViewSelectedLokal));
                        detalji.SetBinding(Grid.DataContextProperty, new Binding()
                        {
                            Source = working_lokal
                        });
                        DatumLokala.Visibility = Visibility.Visible;
                        LabelaDatum.Visibility = Visibility.Visible;
                        KapacitetLokala.Visibility = Visibility.Visible;
                        LabelaKapacitet.Visibility = Visibility.Visible;
                        slc = 1;
                    }
                    else if (item.GetType() == typeof(TipLokala))
                    {
                        detaljiScroll.Visibility = Visibility.Visible;
                        treeViewSelectedTip = (TipLokala)item;
                        working_tip.setTipAs(TipLokala.getCopyTip(treeViewSelectedTip));
                        detalji.SetBinding(Grid.DataContextProperty, new Binding()
                        {
                            Source = working_tip
                        });

                        DatumLokala.Visibility = Visibility.Hidden;
                        LabelaDatum.Visibility = Visibility.Hidden;
                        KapacitetLokala.Visibility = Visibility.Hidden;
                        LabelaKapacitet.Visibility = Visibility.Hidden;
                        slc = 2;
                    }
                    else
                        slc = 0;
                }
                else
                {
                    treeViewSelectedLokal.setValuesAs(new Lokal());
                    treeViewSelectedTip.setTipAs(new TipLokala());
                    slc = 0;
                }

            
        }

        private void Prikazi_Stavec(object sender, RoutedEventArgs args)
        {
            if (slc == 1)
                Prikazi_Tabela(null, null);
            else if (slc == 2)
                Prikazi_Tabelu_Tipova(null, null);
        }

        private void SaveStavec(object sender, RoutedEventArgs args)
        {
            if (slc == 1)
                SaveLokal(null, null);
            if (slc == 2)
                SaveTip(null, null);
        }

        private void PrikaziPomoc(object sender, RoutedEventArgs args)
        {
            HelpNavigationWindow helpWindow = new HelpNavigationWindow();
            helpWindow.HelpFrame.Navigate(new MainPage());
            helpWindow.ShowDialog();
        }

        private void UnosLokala_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Dodaj_Lokal(null, null);
        }
        private void UnosTipa_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Dodaj_Tip(null, null);
        }
        private void UnosEtikete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Dodaj_Etiketa(null, null);
        }

        private void TabelaLokala_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Prikazi_Tabela(null, null);
        }
        private void TabelaTipova_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Prikazi_Tabelu_Tipova(null, null);
        }
        private void TabelaEtiketa_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Prikazi_Tabelu_Etiketa(null, null);
        }

        private void Mapa1_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            changeMap("1");
        }
        private void Mapa2_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            changeMap("2");
        }
        private void Mapa3_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            changeMap("3");
        }
        private void Mapa4_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            changeMap("4");
        }
        private void Sve4Mape_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            changeMap("0");
        }


        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive));
            if (focusedControl is DependencyObject)
            {
                string str = HCI_Project.Help.HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                HCI_Project.Help.HelpProvider.ShowHelp(str, this);
            }
            else
            {
                HCI_Project.Help.HelpProvider.ShowHelp("index", this);
            }

        }
        

    }
}
