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
using HCI_Project.NotBeans;
using System.Windows.Forms;

namespace HCI_Project.Dijalozi
{
    /// <summary>
    /// Interaction logic for IzmenaLokala.xaml
    /// </summary>
    public partial class IzmenaLokala : Window
    {

        private Lokal lokal;

        public Lokal Lokal
        {
            get { return lokal; }
            set { lokal = value; }
        }

        private MainWindow parent;

        public IzmenaLokala(MainWindow m, Lokal l)
        {
            this.lokal = l;
            this.parent = m;
            this.DataContext = l;
            InitializeComponent();
            initializeCombos();
        }

        private void initializeCombos()
        {
            comboAlkohol.ItemsSource = new string[] { "NE SLUZI", "SLUZI DO 23", "DO KASNO NOCU" };
            comboCene.ItemsSource = new string[] { "IZUZETNO VISOKA", "VISOKA", "SREDNJA", "NISKA" };
            comboTipovi.ItemsSource = parent.repoTipovi.sviTipovi();
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
            Etikete etikete = new Etikete(this, this.parent.repoEtikete);
            etikete.ShowDialog();

        }

        private void CloseButtonClick(object sender, RoutedEventArgs args)
        {
            this.Close();
        }

    }
}
