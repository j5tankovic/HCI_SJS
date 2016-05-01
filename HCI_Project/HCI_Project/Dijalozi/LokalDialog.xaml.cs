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
using HCI_Project.Repos;

namespace HCI_Project.Dijalozi
{
    /// <summary>
    /// Interaction logic for LokalDijalog.xaml
    /// </summary>
    public partial class LokalDialog : Window
    {
        private Lokal lokal;
        private RepoLokali repoLokali;
        private RepoTipovi repoTipovi;

        public LokalDialog()
        {
            InitializeComponent();
            lokal = new Lokal();
            this.DataContext = lokal;
            repoLokali = new RepoLokali();
            repoTipovi = new RepoTipovi();
            initializeCombos();
        }

        private void ButtonPotvrdiClicked(object sender, RoutedEventArgs args)
        {
            repoLokali.dodaj(lokal);
            MessageBox.Show("Uspešno ste dodali novi lokal", "Dodavanje lokala");
            this.Close();
        }

        private void ButtonOdustaniClicked(object sender, RoutedEventArgs args)
        {
            this.Close();
        }

        private void initializeCombos()
        {
            comboAlkohol.ItemsSource = Enum.GetNames(typeof(SluzenjeAlkohola));
            comboCene.ItemsSource = Enum.GetNames(typeof(KategorijaCene));
            comboTipovi.ItemsSource = repoTipovi.sviTipovi();

            comboTipovi.SelectedIndex = 0;
        }


      


    }
}
