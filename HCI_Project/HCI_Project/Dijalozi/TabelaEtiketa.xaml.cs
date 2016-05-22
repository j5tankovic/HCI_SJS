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
                foreach (Etiketa e in l.Etikete)
                {
                    if (etiketa.Oznaka.Equals(e.Oznaka))
                    {
                        e.Boja = etiketa.Boja;
                    }
                }
            }
        }
    }
}
