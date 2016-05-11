using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
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
using HCI_Project.Repos;

namespace HCI_Project.Dijalozi
{
    /// <summary>
    /// Interaction logic for LokalDijalog.xaml
    /// </summary>
    public partial class LokalDialog : Window
    {
        private Lokal lokal;
        private Boolean ok = false;

        public Lokal Lokal
        {
            get { return lokal; }
            set { lokal = value; }
        }

        private MainWindow parent;

        public LokalDialog(MainWindow p)
        {
            this.parent = p;
            InitializeComponent();
            lokal = new Lokal();
            this.DataContext = lokal;
            initializeCombos();
        }

        private void ButtonPotvrdiClicked(object sender, RoutedEventArgs args)
        {
            ok = parent.repoLokali.dodaj(lokal);
            if (ok)
            {
                TipLokala tip = parent.repoTipovi[lokal.Tip.Oznaka];
                //if (tip != null)
                //tip.Lokali.Add(lokal);
                System.Windows.MessageBox.Show("Uspešno ste dodali novi lokal", "Dodavanje lokala");
                this.Close();
            }
            else
                System.Windows.MessageBox.Show("Vec postoji lokal sa tom oznakom! Molimo izaberite drugu.");
        }

        private void ButtonOdustaniClicked(object sender, RoutedEventArgs args)
        {
            ok = false;
            this.Close();
        }

        private void initializeCombos()
        {
            comboAlkohol.ItemsSource = Enum.GetNames(typeof(SluzenjeAlkohola));
            comboCene.ItemsSource = Enum.GetNames(typeof(KategorijaCene));
            comboTipovi.ItemsSource = parent.repoTipovi.sviTipovi();

            comboTipovi.SelectedIndex = 0;
        }

        void LokalDialog_Closing(object sender, CancelEventArgs e)
        {
            if (!ok)
                lokal = null;
        }

        private void izaberiFajlClicked(object sender, RoutedEventArgs args)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.RestoreDirectory = true;
            dialog.Filter = "Image Files (*.bmp, *.jpg)|*.bmp;*.jpg";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                String filename = dialog.FileName;
                if (filename != null && !filename.Equals(""))
                    lokal.Slika = filename;
            }

        }

        


      


    }
}
