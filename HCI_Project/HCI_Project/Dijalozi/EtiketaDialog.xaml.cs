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
    /// Interaction logic for EtiketaDialog.xaml
    /// </summary>
    public partial class EtiketaDialog : Window
    {
        private Etiketa etiketa;
        private RepoEtikete repoEtiketa;

        public EtiketaDialog()
        {
            InitializeComponent();
            etiketa = new Etiketa();
            repoEtiketa = new RepoEtikete();
            this.DataContext = etiketa;
        }

        private void ButtonPotvrdiClicked(object sender, RoutedEventArgs e)
        {
            repoEtiketa.dodaj(etiketa);
            MessageBox.Show("Uspešno ste dodali novu etiketu za lokal","Dodavanje etikete");
            this.Close();
        }

        private void ButtonOdustaniClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void colorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            etiketa.Boja = ColorPicker.SelectedColor.Value;
        }
    }
}
