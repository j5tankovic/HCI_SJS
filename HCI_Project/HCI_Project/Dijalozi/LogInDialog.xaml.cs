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

namespace HCI_Project.Dijalozi
{
    /// <summary>
    /// Interaction logic for LogInDialog.xaml
    /// </summary>
    public partial class LogInDialog : Window
    {
        bool success = false;

        public bool Success
        {
            get { return success; }
            set { success = value; }
        }


        public LogInDialog()
        {
            InitializeComponent();
            KorisnickoIme.Focus();
        }


        public void UlogujSe(object sender, RoutedEventArgs args)
        {
            string korisnickoIme = KorisnickoIme.Text;
            string sifra = Sifra.Password;
            if (korisnickoIme.Equals("covek") && sifra.Equals("covek"))
            {
                success = true;
                this.Close();
            }
            else
            {
                System.Windows.MessageBox.Show("Korisnicko ime ili lozinka nisu ispravni. Molimo pokusajte ponovo.", "Login nije uspeo", MessageBoxButton.OK);
            }
        }

        private void loginClosed(object sender, EventArgs e)
        {
            if (success == false)
                Application.Current.Shutdown();
        }

        private void EnterClicked_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            UlogujSe(null, null);
        }
       
    }
}
