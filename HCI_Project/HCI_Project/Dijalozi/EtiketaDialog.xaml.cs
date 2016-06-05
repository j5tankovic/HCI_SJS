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
using HCI_Project.Help;

namespace HCI_Project.Dijalozi
{
    /// <summary>
    /// Interaction logic for EtiketaDialog.xaml
    /// </summary>
    public partial class EtiketaDialog : Window
    {
        private Etiketa etiketa;
        private MainWindow parent;
        private Boolean ok = false;

        public EtiketaDialog(MainWindow p)
        {
            this.parent = p;
            etiketa = new Etiketa();
            this.Resources.Add("etiketa", etiketa);
            this.Resources.Add("parent", parent);
            InitializeComponent();

            this.DataContext = etiketa;
        }

        private void ButtonPotvrdiClicked(object sender, RoutedEventArgs e)
        {
            if (ColorPicker.SelectedColor == null)
            {
                System.Windows.MessageBox.Show("Niste odabrali boju etikete. Morate odabrati boju da biste mogli da kreirate novu etiketu.", "Niste izabrali boju");
                return;
            }
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Da li ste sigurni da zelite da dodate ovu etiketu?", "Unos etikete", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.No)
                return;
            ok = parent.repoEtikete.dodaj(etiketa);
            if (ok)
            {
                MessageBox.Show("Uspešno ste dodali novu etiketu za lokal", "Dodavanje etikete");
                this.Close();
            }
            else
            {
                MessageBox.Show("Vec postoji etiketa sa tom oznakom! Molimo izaberite drugu.", "Greska!");
            }
        }

        private void ButtonOdustaniClicked(object sender, RoutedEventArgs e)
        {
            ok = false;
            this.Close();
        }

        private void colorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            etiketa.Boja = ColorPicker.SelectedColor.Value;
        }

        void EtiketaDialog_Closing(object sender, CancelEventArgs e)
        {
            if (!ok)
                etiketa = null;
        }

        private void EtiketaDialog_Loaded(object sender, RoutedEventArgs args)
        {
            oznakaEtikete.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty).UpdateSource();
        }


        private void PrikaziPomoc(object sender, RoutedEventArgs args)
        {
            HelpNavigationWindow helpWindow = new HelpNavigationWindow();
            helpWindow.HelpFrame.Navigate(new HelpDialogEtiketa());
            helpWindow.ShowDialog();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive));
            if (focusedControl is DependencyObject)
            {
                string str = HCI_Project.Help.HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                HCI_Project.Help.HelpProvider.ShowHelp(str, this);
            }
            else
            {
                HCI_Project.Help.HelpProvider.ShowHelp("index", this);
            }
        }

        private void Escape_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();

        }
    }
}
