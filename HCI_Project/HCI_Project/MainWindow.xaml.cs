using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HCI_Project.Dijalozi;
using HCI_Project.Beans;
using System.Collections.ObjectModel;
using HCI_Project.Repos;

namespace HCI_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<TipLokala> Tipovi
        {
            get;
            set;
        }

        private RepoTipovi repoTipovi;
        private RepoLokali repoLokali;
        private RepoEtikete repoEtikete;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            repoTipovi = new RepoTipovi();
            repoLokali = new RepoLokali();
            repoEtikete = new RepoEtikete();
            List<TipLokala> popunjeniTipovi = popuniLokalima();
            Tipovi = new ObservableCollection<TipLokala>(popunjeniTipovi);
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
            JpegBitmapDecoder decoder2 = new JpegBitmapDecoder(myUri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource2 = decoder2.Frames[0];

            // Draw the Image
            myImage.Source = bitmapSource2;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void Dodaj_Lokal(object sender, RoutedEventArgs e)
        {
            LokalDialog lokal = new LokalDialog();
            lokal.InitializeComponent();
            lokal.Show();
        }

        private void Dodaj_Tip(object sender, RoutedEventArgs e)
        {
            TipDialog tip = new TipDialog();
            tip.InitializeComponent();
            tip.Show();
        }

        private void Dodaj_Etiketa(object sender, RoutedEventArgs e)
        {
            EtiketaDialog etiketa = new EtiketaDialog();
            etiketa.InitializeComponent();
            etiketa.Show();
        }

        private void Prikazi_Tabela(object sender, RoutedEventArgs e)
        {
            TabelaLokala tabela = new TabelaLokala();
            tabela.InitializeComponent();
            tabela.Show();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs args)
        {
            myImage.Width = theScrollViewer.ViewportWidth;
            myImage.Height = theScrollViewer.ViewportHeight;
        }


        private List<TipLokala> popuniLokalima()
        {
            List<TipLokala> tipoviLokala = repoTipovi.sviTipovi();
            List<Lokal> lokali = repoLokali.sviLokali();
            foreach(var lokal in lokali)
            {
                tipoviLokala.ForEach(x => { if (x.Oznaka == lokal.Tip.Oznaka) x.Lokali.Add(lokal); });
            }

            return tipoviLokala;
        }


    }
}
