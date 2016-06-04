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
using System.Windows.Forms;

namespace HCI_Project.Dijalozi
{
    /// <summary>
    /// Interaction logic for BrisanjeTipa.xaml
    /// </summary>
    public partial class BrisanjeTipa : Window
    {

        public MainWindow parent {get; set;}
        public TipLokala tip;
        public TipLokala noviTip;

        public BrisanjeTipa(MainWindow p, TipLokala t)
        {
            this.parent = p;
            this.tip = t;
            InitializeComponent();
            this.DataContext = noviTip;
            dgrMain.ItemsSource = tip.Lokali;
            

        }

        private void IzmenaLokala(object sender, RoutedEventArgs args)
        {
            Lokal lokal = (Lokal) dgrMain.SelectedItem;
            if (lokal == null) return;
            LokalDialog dialog = new LokalDialog(parent, lokal);
            dialog.ShowDialog();
            parent.dopuniTip(lokal);
        }

        private void oznakaTextChanged(object sender, TextChangedEventArgs args)
        {
            System.Windows.Controls.TextBox box = (System.Windows.Controls.TextBox)sender;
            TipLokala tip1 = parent.repoTipovi.nadjiPoOznaci(box.Text);
            if (tip.Equals(tip1))
            {
                noviTip = null;
                this.DataContext = null;
                return;
            }
            noviTip = tip1;
            this.DataContext = noviTip;
        }

        private void otvoriTabeluTipova(object sender, RoutedEventArgs args)
        {
            TabelaTipova tabela = new TabelaTipova(this.parent);
            tabela.ShowDialog();
            TipLokala tip1 = tabela.IzabraniTip;
            if (tip1 != null && tip1.Equals(tip))
            {
                noviTip = tip1;
                this.DataContext = noviTip;
                OznakaTipa.Text = tip1.Oznaka;
            }     
        }

        private void obrisiSve(object sender, RoutedEventArgs args)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Da li ste sigurni da zelite da obrisete tip i sve njegove lokale?", "Brisanje tipa", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                parent.repoLokali.izbaciSve(tip.Lokali);
                parent.repoTipovi.izbaci(tip);
                this.Close();
            }
        }

        private void odustani(object sender, RoutedEventArgs args)
        {
            this.Close();
        }

        private void zameni(object sender, RoutedEventArgs args)
        {
            if (noviTip != null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Da li ste sigurni da zelite da obrisete tip, a njegovim lokalima dodelite novi tip?", "Brisanje tipa", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    parent.repoTipovi.zameniTip(tip.Lokali, noviTip);
                    parent.repoTipovi.izbaci(tip);
                    this.Close();
                }
            }
        }

    }
}
