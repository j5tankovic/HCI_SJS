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
using System.Collections;

namespace HCI_Project.Dijalozi
{
    /// <summary>
    /// Interaction logic for TabelaLokala.xaml
    /// </summary>
    public partial class TabelaLokala : Window
    {
        public MainWindow parent { get; set; }
        private string stariTipOznaka { get; set; }

        private Lokal lokal_za_izmenu { get; set; }
        private Lokal tekuci_lokal { get; set; }

        ObservableCollection<Lokal> lokali;

  
        public TabelaLokala(MainWindow p)
        {
            this.parent = p;
            this.tekuci_lokal = new Lokal();
            this.Resources.Add("parent", parent);
            this.DataContext = tekuci_lokal;
            InitializeComponent();
            initializeCombos();
            dgrMain.ItemsSource = this.parent.repoLokali.sviLokali();
            lokali = new ObservableCollection<Lokal>(this.parent.repoLokali.sviLokali());
            

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
                    Lokal l = this.tekuci_lokal;
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
                //this.DataContext = (Lokal)dgrMain.SelectedItem;
                Etikete etikete = new Etikete(this, this.parent.repoEtikete);
                etikete.ShowDialog();
                //this.DataContext = this.tekuci_lokal;
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void otvoriTabeluTipova(object sender, RoutedEventArgs args)
        {
            if (this.tekuci_lokal.Oznaka == null)
                return;
            TabelaTipova tabela = new TabelaTipova(this.parent);
            tabela.ShowDialog();
            TipLokala tip = tabela.IzabraniTip;
            if (tip != null)
            {
                Lokal lokal = this.tekuci_lokal;
                if (lokal.Oznaka == null)
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
            if (tekuci_lokal == null)
                return;
            tekuci_lokal.Tip = tip;
            OznakaTipa.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty).UpdateSource();
        }

        private void dgrMain_SelectedCellsChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgrMain.SelectedItem != null)
            {
                this.stariTipOznaka = ((Lokal)dgrMain.SelectedItem).Tip.Oznaka;

                this.lokal_za_izmenu = (Lokal)dgrMain.SelectedItem;
                Lokal kopija = Lokal.getCopyLokal((Lokal)dgrMain.SelectedItem);
                this.tekuci_lokal.setValuesAs(kopija);
            }
            else
            {
                this.tekuci_lokal = null;
                this.DataContext = this.tekuci_lokal;
            }

        }

       



        public void textFieldChanged(object sender, TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox textbox = sender as System.Windows.Controls.TextBox;
            string filter = textbox.Text;
            ICollectionView cv = CollectionViewSource.GetDefaultView(lokali);
            if (filter == "")
                cv.Filter = null;
            else
            {
                cv.Filter = o =>
                {
                    Lokal lokal = o as Lokal;
                    string[] words = filter.Split(' ');
                    if (words.Contains(""))
                        words = words.Where(word => word != "").ToArray();
                    return words.Any(word => lokal.Oznaka.ToUpper().Contains(word.ToUpper()) || lokal.Naziv.ToUpper().Contains(word.ToUpper()) ||
                            lokal.Opis.ToUpper().Contains(word.ToUpper()) || lokal.Slika.ToUpper().Contains(word.ToUpper()) ||
                            lokal.Tip.Naziv.ToUpper().Contains(word.ToUpper()));
                };

                dgrMain.ItemsSource = lokali;
            }
        }

      
        public void deleteFilter(object sender, RoutedEventArgs e)
        {
            TextFilter.Text = "";
        }

       

        private void sacuvajTekuci(object sender, RoutedEventArgs args)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Da li ste sigurni da zelite da sacuvate izmene?", "Update Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                this.lokal_za_izmenu.izmeniTipPotpuno(this.tekuci_lokal.Tip);
                this.lokal_za_izmenu.setValuesAs(this.tekuci_lokal);
                this.parent.repoLokali.memorisi();
                this.parent.repoTipovi.memorisi();

                System.Windows.MessageBox.Show("Izmena uspesna!");
            }
        }

      
    }

}