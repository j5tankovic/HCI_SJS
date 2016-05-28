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
    /// Interaction logic for TabelaLokala.xaml
    /// </summary>
    public partial class TabelaLokala : Window
    {
        public MainWindow parent { get; set; }
        private string stariTipOznaka { get; set; }
  
        public TabelaLokala(MainWindow p)
        {
            this.parent = p;
            this.Resources.Add("parent", parent);
            InitializeComponent();
            this.DataContext = this;
            initializeCombos();
            dgrMain.ItemsSource = this.parent.repoLokali.sviLokali();
        }

        private void initializeCombos()
        {
            comboAlkohol.ItemsSource = new string[] { "NE SLUZI", "SLUZI DO 23", "DO KASNO NOCU" };
            comboCene.ItemsSource = new string[] {"IZUZETNO VISOKA", "VISOKA", "SREDNJA", "NISKA"};

            
        }

        private void Delete(object sender, RoutedEventArgs args)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Da li ste sigurni?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Lokal lokal = (Lokal)dgrMain.SelectedItem;
                if (lokal == null)
                    return;
                parent.removeLokalFromMap(lokal);
                parent.repoLokali.izbaci(lokal);
                TipLokala tip = parent.nadjiTipLokala(lokal);
                if (tip != null)
                    tip.izbaciLokal(lokal);
            }
        }


        public void promeniIkonicu(object sender, RoutedEventArgs args)
        {
            if (dgrMain.SelectedItem != null)
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Title = "Izaberite ikonicu";
                fileDialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                                    "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                                    "Portable Network Graphic (*.png)|*.png";
                if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Lokal l = this.dgrMain.SelectedItem as Lokal;
                    if (l != null)
                    {
                        l.Slika = fileDialog.FileName;
                    }

                }
            }
        }

        private void izberiEtikete(object sender, RoutedEventArgs args)
        {
            if (dgrMain.SelectedItem != null)
            {
                this.DataContext = (Lokal)dgrMain.SelectedItem;
                Etikete etikete = new Etikete(this, this.parent.repoEtikete);
                etikete.ShowDialog();
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void otvoriTabeluTipova(object sender, RoutedEventArgs args)
        {
            TabelaTipova tabela = new TabelaTipova(this.parent);
            tabela.ShowDialog();
            TipLokala tip = tabela.IzabraniTip;
            if (tip != null)
            {
                Lokal lokal = (Lokal)dgrMain.SelectedItem;
                if (lokal == null)
                    return;
                NazivTipa.Text = tip.Naziv;
                OznakaTipa.Text = tip.Oznaka;
                lokal.Tip = tip;
            }            
        }

        private void oznakaTipa_TextChanged(object sender, RoutedEventArgs args)
        {
            System.Windows.Controls.TextBox box = (System.Windows.Controls.TextBox)sender;
            TipLokala tip = parent.repoTipovi.nadjiPoOznaci(box.Text);
            if (tip == null)
                return;
            Lokal lokal = (Lokal)dgrMain.SelectedItem;
            if (lokal.Tip != null)
            {
                Console.WriteLine("Izbacujem iz "+lokal.Tip.Oznaka);
                lokal.Tip.izbaciLokal(lokal);
            }
            lokal.Tip = tip;
            lokal.Tip.ubaciLokal(lokal);
        }

        private void dgrMain_SelectedCellsChanged(object sender, SelectionChangedEventArgs e)
        {
            this.stariTipOznaka = ((Lokal)dgrMain.SelectedItem).Tip.Oznaka;
        }

        private void oznakaTipa_SourceUpdated(object sender, DataTransferEventArgs args)
        {
            Lokal lokal = (Lokal)dgrMain.SelectedItem;
            TipLokala noviTip = lokal.Tip;
            if (lokal == null || noviTip == null)
                return;
            parent.repoTipovi.nadjiTipPoOznaciIIzbaciLokal(this.stariTipOznaka, lokal);
            this.stariTipOznaka = lokal.Tip.Oznaka;
            lokal.Tip.ubaciLokal(lokal);
        }
      
    }

}