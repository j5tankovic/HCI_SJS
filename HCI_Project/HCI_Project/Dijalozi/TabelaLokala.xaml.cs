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
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using HCI_Project.Beans;

namespace HCI_Project.Dijalozi
{
    /// <summary>
    /// Interaction logic for TabelaLokala.xaml
    /// </summary>
    public partial class TabelaLokala : Window
    {
        public ObservableCollection<Lokal> Lokali
        {
            get;
            set;
        }
        public TabelaLokala()
        {
            InitializeComponent();
            this.DataContext = this;
            Lokali = new ObservableCollection<Lokal>();
            Lokal l = new Lokal();
            l.Oznaka = "a11";
            l.Naziv = "Kafana";
            l.Hendikep = true;
            l.Pusenje = false;
            l.Rezervacije = true;
            l.Opis = "Opisss";
            l.Tip = "tttippp";
            l.Alkohol = SluzenjeAlkohola.SLUZI_DO_23;
            l.Cene = KategorijaCene.VISOKA;
            l.Datum = "10.2.2014.";
            l.Kapacitet = 100;
            Lokali.Add(l);
        }
    }
}
