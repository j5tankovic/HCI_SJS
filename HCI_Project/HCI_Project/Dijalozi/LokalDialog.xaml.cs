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
using HCI_Project.Help;

namespace HCI_Project.Dijalozi
{
    /// <summary>
    /// Interaction logic for LokalDijalog.xaml
    /// </summary>
    public partial class LokalDialog : Window
    {
        private Lokal lokal;
        private Lokal lokal_za_izmenu;
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
            this.lokal_za_izmenu = l;
            this.parent = p;
            if (l != null)
            {
                this.lokal = Lokal.getCopyLokal(l);
            }
            else
            {
                this.lokal = new Lokal();
                lokal.Datum = new DateTime(2016, 1, 1);
            }
            this.Resources.Add("parent", parent);
            this.Resources.Add("lokal", lokal);
            InitializeComponent();
            if (lokal.Tip != null)
                oznakaTipa.Text = lokal.Tip.Oznaka;
            if (!kreiranje)
            {
                oznakaLokala.IsReadOnly = true;
                oznakaLokala.Background = new SolidColorBrush(Colors.WhiteSmoke);
            }
            this.DataContext = lokal;
            initializeCombos();
        }

        private void ButtonPotvrdiClicked(object sender, RoutedEventArgs args)
        {
            if (!checkForm())
                return;
            string poruka = kreiranje ? "Da li ste sigurni da zelite da unesete ovaj lokal?" : "Da li ste sigurni da zelite da izmenite ovaj lokal?";
            string naslov = kreiranje ? "Unos lokala" : "Izmena lokala";
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(poruka, naslov, System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.No)
                return;
            if (!kreiranje)
            {
                this.lokal_za_izmenu.izmeniTipPotpuno(this.lokal.Tip);
                this.lokal_za_izmenu.setValuesAs(this.lokal);
                this.Close();
                return;
            }
            ok = parent.repoLokali.dodaj(lokal);
            if (ok)
            {
                //TipLokala tip = parent.repoTipovi[lokal.Tip.Oznaka];
                //if (tip != null)
                //tip.Lokali.Add(lokal);
                System.Windows.MessageBox.Show("Uspešno ste dodali novi lokal", "Dodavanje lokala");
                this.Close();
            }
            else
                System.Windows.MessageBox.Show("Vec postoji lokal sa tom oznakom! Molimo izaberite drugu.", "Greska!");
        }

        private void ButtonOdustaniClicked(object sender, RoutedEventArgs args)
        {
            ok = false;
            this.Close();
        }

        private void initializeCombos()
        {
            comboAlkohol.ItemsSource = new string[] { "NE SLUZI", "SLUZI DO 23", "DO KASNO NOCU" };
            comboCene.ItemsSource = new string[] { "IZUZETNO VISOKA", "VISOKA", "SREDNJA", "NISKA" };
            
        }

        void LokalDialog_Closing(object sender, CancelEventArgs e)
        {
            if (!ok)
                lokal = null;
            this.parent.repoLokali.memorisi();
            this.parent.repoTipovi.memorisi();
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
            etikete.ShowDialog();

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
            TipLokala tip = parent.repoTipovi.nadjiPoOznaci(box.Text);
            lokal.Tip = tip;
            oznakaTipa.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty).UpdateSource();
        }

        private void LokalDialog_Loaded(object sender, RoutedEventArgs args)
        {
            nazivLokala.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty).UpdateSource();
            oznakaLokala.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty).UpdateSource();
            kapacitetLokala.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty).UpdateSource();
            oznakaTipa.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty).UpdateSource();
        }

        private bool checkForm()
        {
            if (lokal.Tip == null)
            {
                System.Windows.MessageBox.Show("Niste izabrali tip lokala.", "Greska!");
                return false;
            }
            if (comboAlkohol.SelectedItem.Equals(""))
            {
                System.Windows.MessageBox.Show("Niste izabrali nacin sluzenja alkohola.", "Greska!");
                return false;
            }
            if (comboAlkohol.SelectedItem.Equals(""))
            {
                System.Windows.MessageBox.Show("Niste izabrali kategoriju cene.", "Greska!");
                return false;
            }
            var result = Validation.GetErrors(oznakaLokala);
            if (result.Count > 0) // has errors.
                return false;
            result = Validation.GetErrors(nazivLokala);
            if (result.Count > 0)
                return false;
            result = Validation.GetErrors(kapacitetLokala);
            if (result.Count > 0)
                return false;
            return true;
        }

        private void PrikaziPomoc(object sender, RoutedEventArgs args)
        {
            HelpNavigationWindow helpWindow = new HelpNavigationWindow();
            helpWindow.HelpFrame.Navigate(new HelpDialogLokal());
            helpWindow.ShowDialog();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(System.Windows.Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                string str = HCI_Project.Help.HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                HCI_Project.Help.HelpProvider.ShowHelp(str, this);
            }
        }

        


    }
}
