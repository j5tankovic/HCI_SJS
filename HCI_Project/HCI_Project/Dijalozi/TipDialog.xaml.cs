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
using HCI_Project.Beans;
using HCI_Project.Repos;

namespace HCI_Project.Dijalozi
{
    /// <summary>
    /// Interaction logic for TipDialog.xaml
    /// </summary>
    public partial class TipDialog : Window
    {
        private TipLokala tipLokala;
        private RepoTipovi repoTipovi;

        public TipDialog()
        {
            InitializeComponent();
            tipLokala = new TipLokala();
            this.DataContext = tipLokala;
            repoTipovi = new RepoTipovi();
        }


        private void ButtonPotvrdiClicked(object sender, RoutedEventArgs args)
        {
            
            repoTipovi.dodaj(tipLokala);
            MessageBox.Show("Uspešno ste dodali novi tip lokala", "Dodavanje tipa lokala");
            this.Close();
        }

        private void ButtonOdustaniClicked(object sender, RoutedEventArgs args)
        {
            this.Close();
        }
    }
}
