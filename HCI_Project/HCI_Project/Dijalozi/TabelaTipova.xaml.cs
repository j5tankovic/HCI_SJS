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
    /// Interaction logic for TabelaTipova.xaml
    /// </summary>
    public partial class TabelaTipova : Window
    {

        public MainWindow parent { get; set; }

        private TipLokala _izabraniTip;
        public TipLokala IzabraniTip
        {
            get
            {
                return _izabraniTip;
            }
        }

        public TabelaTipova(MainWindow p)
        {
            this.parent = p;
            InitializeComponent();
            this.DataContext = this;
            this.dgrMain.ItemsSource = this.parent.repoTipovi.sviTipovi();
        }

        private void Delete(object sender, RoutedEventArgs args)
        {
            /*
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Da li ste sigurni?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                TipLokala tip = (TipLokala)dgrMain.SelectedItem;
                if (tip == null)
                    return;
                parent.repoTipovi.izbaci(tip);
            }*/
            TipLokala tip = (TipLokala)dgrMain.SelectedItem;
            if (tip == null)
                return;
            BrisanjeTipa dialog = new BrisanjeTipa(parent, tip);
            dialog.ShowDialog();
        }

        public void promeniIkonicu(object sender, RoutedEventArgs args)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Izaberite ikonicu";
            fileDialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                                "Portable Network Graphic (*.png)|*.png";
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TipLokala tl = this.dgrMain.SelectedItem as TipLokala;
                if (tl!= null)
                    tl.Slika = fileDialog.FileName;

            }
        }

        private void Clicked_OK(object sender, RoutedEventArgs args)
        {
            if (dgrMain.SelectedItem != null)
                _izabraniTip = (TipLokala)dgrMain.SelectedItem;
            else
                _izabraniTip = null;
            this.Close();
        }

        public void textFieldChanged(object sender, TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox textbox = sender as System.Windows.Controls.TextBox;
            string filter = textbox.Text;
            ICollectionView cv = CollectionViewSource.GetDefaultView(dgrMain.ItemsSource);
            if (filter == "")
                cv.Filter = null;
            else
            {
                cv.Filter = o =>
                {
                    TipLokala tip = o as TipLokala;
                    if (textbox.Name == "OznakaFilter")
                        return tip.Oznaka.ToUpper().StartsWith(filter.ToUpper());
                    else if (textbox.Name == "NazivFilter")
                        return tip.Naziv.ToUpper().StartsWith(filter.ToUpper());
                    return false;
                };
            }
        }


        public void deleteFilters(object sender, RoutedEventArgs e)
        {
            OznakaFilter.Text = "";
            NazivFilter.Text = "";
        }
    }
}
