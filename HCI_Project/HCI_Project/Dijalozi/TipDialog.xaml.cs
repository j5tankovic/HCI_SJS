using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HCI_Project.NotBeans;
using HCI_Project.Repos;

namespace HCI_Project.Dijalozi
{
    /// <summary>
    /// Interaction logic for TipDialog.xaml
    /// </summary>
    public partial class TipDialog : Window
    {
        private TipLokala tipLokala;
        private Boolean ok = false;

        public TipLokala TipLokala
        {
            get { return tipLokala; }
            set { tipLokala = value; }
        }
        private MainWindow parent;

        public TipDialog(MainWindow p)
        {
            this.parent = p;
            InitializeComponent();
            tipLokala = new TipLokala();
            this.DataContext = tipLokala;
        }


        private void ButtonPotvrdiClicked(object sender, RoutedEventArgs args)
        {

            parent.repoTipovi.dodaj(tipLokala);
            MessageBox.Show("Uspešno ste dodali novi tip lokala", "Dodavanje tipa lokala");
            ok = true;
            this.Close();
        }

        private void ButtonOdustaniClicked(object sender, RoutedEventArgs args)
        {
            ok = false;
            this.Close();
        }

        void TipDialog_Closing(object sender, CancelEventArgs e)
        {
            if (!ok)
                tipLokala = null;
        }


        
    }
}
