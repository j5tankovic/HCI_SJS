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
using HCI_Project.Beans;

namespace HCI_Project.Dijalozi
{
    /// <summary>
    /// Interaction logic for TabelaEtiketa.xaml
    /// </summary>
    public partial class TabelaEtiketa : Window
    {

        private MainWindow parent;

        public ObservableCollection<Etiketa> etikete
        {
            get;
            set;
        }

        public TabelaEtiketa(MainWindow p)
        {
            this.parent = p;
            etikete = new ObservableCollection<Etiketa>();
            parent.repoEtikete.etikete.ToList().ForEach(etikete.Add);
            InitializeComponent();
            this.DataContext = this;
        }

        private void colorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            ((Etiketa)dgrMain.SelectedItem).Boja = ColorPicker.SelectedColor.Value;
        }

        private void Delete(object sender, RoutedEventArgs args)
        {
            Etiketa etiketa = (Etiketa)dgrMain.SelectedItem;
            if (etiketa == null)
                return;
            izbaci(etiketa);
            parent.repoEtikete.izbaci(etiketa);
        }

        private void izbaci(Etiketa etiketa)
        {
            for (int i = 0; i < etikete.Count; i++)
            {
                Etiketa l = etikete[i];
                if (l.Oznaka.Equals(etiketa.Oznaka))
                {
                    etikete.RemoveAt(i);
                    return;
                }
            }
        }
    }
}
