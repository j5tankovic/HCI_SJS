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
using Xceed.Wpf.Toolkit;

namespace HCI_Project.Dijalozi
{
    /// <summary>
    /// Interaction logic for TabelaEtiketa.xaml
    /// </summary>
    public partial class TabelaEtiketa : Window
    {

        public MainWindow parent { get; set; }

        public TabelaEtiketa(MainWindow p)
        {
            this.parent = p;
            InitializeComponent();
            this.DataContext = this;
            this.dgrMain.ItemsSource = this.parent.repoEtikete.sveEtikete();
            
        }

        private void colorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Etiketa etiketa = ((Etiketa)dgrMain.SelectedItem);
            if (etiketa == null)
                return;
            etiketa.Boja = ColorPicker.SelectedColor.Value;
            azurirajLokale(etiketa);
        }

        private void Delete(object sender, RoutedEventArgs args)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Da li ste sigurni?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Etiketa etiketa = (Etiketa)dgrMain.SelectedItem;
                if (etiketa == null)
                    return;
                parent.repoEtikete.izbaci(etiketa);
            }
        }

        public void azurirajLokale(Etiketa etiketa)
        {
            foreach (Lokal l in this.parent.repoLokali.sviLokali())
            {
                if (l.Etikete != null)
                    foreach (Etiketa e in l.Etikete)
                    {
                        if (etiketa.Oznaka.Equals(e.Oznaka))
                        {
                            e.Boja = etiketa.Boja;
                        }
                    }
            }
        }

        public void textFieldChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            string filter = textbox.Text;
            ICollectionView cv = CollectionViewSource.GetDefaultView(dgrMain.ItemsSource);
            if (filter == "")
                cv.Filter = null;
            else
            {
                cv.Filter = o => 
                {
                    Etiketa etiketa = o as Etiketa;
                    if (textbox.Name == "OznakaFilter")
                        return etiketa.Oznaka.ToUpper().StartsWith(filter.ToUpper());
                    else if (textbox.Name == "OpisFilter")
                        return etiketa.Opis.ToUpper().StartsWith(filter.ToUpper());
                    return false;
                };
            }
        }

        public void colorFilterChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            ICollectionView cv = CollectionViewSource.GetDefaultView(dgrMain.ItemsSource);
            ColorPicker picker = sender as ColorPicker;
            Color? choosedColor = picker.SelectedColor;
            if (choosedColor == null)
                cv.Filter = null;
            else
            {

                cv.Filter = o =>
                {
                    Etiketa etiketa = o as Etiketa;
                    return choosedColor.Equals(etiketa.Boja);
                };
            }

        }

        public void deleteFilters(object sender, RoutedEventArgs e)
        {
            OznakaFilter.Text = "";
            OpisFilter.Text = "";
        }
    }
}
