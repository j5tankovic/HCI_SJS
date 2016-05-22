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
using System.ComponentModel;
using System.Collections.ObjectModel;
using HCI_Project.NotBeans;

namespace HCI_Project.Dijalozi
{
    /// <summary>
    /// Interaction logic for TabelaLokala.xaml
    /// </summary>
    public partial class TabelaLokala : Window
    {
        public MainWindow parent { get; set; }

        ObservableCollection<Lokal> sviLokali = new ObservableCollection<Lokal>();

        public ObservableCollection<Lokal> SviLokali
        {
            get { return sviLokali; }
            set { sviLokali = value; }
        }

        
        public TabelaLokala(MainWindow p)
        {
            this.parent = p;
            InitializeComponent();
            this.DataContext = this;
            initializeCombos();
            this.sviLokali = this.parent.repoLokali.sviLokali();
            dgrMain.ItemsSource = this.sviLokali;
        }

        private void initializeCombos()
        {
            comboAlkohol.ItemsSource = new string[] { "NE SLUZI", "SLUZI DO 23", "DO KASNO NOCU" };
            comboCene.ItemsSource = new string[] {"IZUZETNO VISOKA", "VISOKA", "SREDNJA", "NISKA"};
            comboTipovi.ItemsSource = parent.repoTipovi.sviTipovi();

            
        }

        private void Delete(object sender, RoutedEventArgs args)
        {
            Lokal lokal = (Lokal)dgrMain.SelectedItem;
            if (lokal == null)
                return;
            parent.repoLokali.izbaci(lokal);
            TipLokala tip = parent.nadjiTipLokala(lokal);
            if (tip != null)
                tip.izbaciLokal(lokal);
        }



      
    }

}