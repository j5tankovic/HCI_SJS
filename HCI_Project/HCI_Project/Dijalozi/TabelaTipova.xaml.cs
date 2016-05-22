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

        public TabelaTipova(MainWindow p)
        {
            this.parent = p;
            InitializeComponent();
            this.DataContext = this;
            this.dgrMain.ItemsSource = this.parent.repoTipovi.sviTipovi();
        }

        private void Delete(object sender, RoutedEventArgs args)
        {
            TipLokala tip = (TipLokala)dgrMain.SelectedItem;
            if (tip == null)
                return;
            parent.repoTipovi.izbaci(tip);
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
    }
}
