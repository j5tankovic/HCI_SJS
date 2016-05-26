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
using HCI_Project.Repos;
using System.Windows.Forms;

namespace HCI_Project.Dijalozi
{
    /// <summary>
    /// Interaction logic for LokalDijalog.xaml
    /// </summary>
    public partial class LokalDialog : Window
    {
        private Lokal lokal;
        private bool ok = false;
        private bool kreiranje;

        public Lokal Lokal
        {
            get { return lokal; }
            set { lokal = value; }
        }

        public MainWindow parent { get; set; }

        public LokalDialog(MainWindow p, Lokal l)
        {
            this.kreiranje = l == null ? true : false;
            this.lokal = l == null ? new Lokal() : l;
            this.parent = p;
            this.Resources.Add("parent", parent);
            InitializeComponent();
            if (l != null)
            {
                //oznakaLokala.IsReadOnly = true;
                oznakaTipa.Text = l.Tip.Oznaka;
            }
            else
                lokal.Datum = new DateTime(2016, 1, 1);
            this.DataContext = lokal;
            initializeCombos();
        }

        private void ButtonPotvrdiClicked(object sender, RoutedEventArgs args)
        {
            if (!kreiranje)
                this.Close();
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
        }

        void LokalDialog_Closing(object sender, CancelEventArgs e)
        {
            if (!ok)
                lokal = null;
        }

        private void izaberiFajlClicked(object sender, RoutedEventArgs args)
        {
            /*
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.RestoreDirectory = true;
            dialog.Filter = "Image Files (*.bmp, *.jpg)|*.bmp;*.jpg";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                String filename = dialog.FileName;
                if (filename != null && !filename.Equals(""))
                    lokal.Slika = filename;
            }
             * */

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Izaberite ikonicu";
            fileDialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                                "Portable Network Graphic (*.png)|*.png";
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Ikonica.Source = new BitmapImage(new Uri(fileDialog.FileName));
                lokal.Slika = fileDialog.FileName;

            }

        }

        private void izberiEtikete(object sender, RoutedEventArgs args)
        {
            Etikete etikete = new Etikete(this,this.parent.repoEtikete);
            etikete.Show();

        }

        private void OtvoriTabeluTipova(object sender, RoutedEventArgs args)
        {
            TabelaTipova tabela = new TabelaTipova(this.parent);
            tabela.ShowDialog();
            TipLokala tip = tabela.IzabraniTip;
            if (tip != null)
            {
                lokal.Tip = tip;
                oznakaTipa.Text = tip.Oznaka;
            }            
        }

        private void oznakaTipa_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox box = (System.Windows.Controls.TextBox)sender;
            TipLokala tip= parent.repoTipovi.nadjiPoOznaci(box.Text);
            lokal.Tip = tip;
            
        }


    }
}
