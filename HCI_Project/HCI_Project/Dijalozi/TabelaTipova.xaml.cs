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
using System.Windows.Forms;
using System.Collections;
using HCI_Project.Help;

namespace HCI_Project.Dijalozi
{
    /// <summary>
    /// Interaction logic for TabelaTipova.xaml
    /// </summary>
    public partial class TabelaTipova : Window
    {

        public MainWindow parent { get; set; }
        private TipLokala tekuci_tip { get; set; }
        private TipLokala tip_za_izmenu { get; set; }

        private TipLokala _izabraniTip;
        public TipLokala IzabraniTip
        {
            get
            {
                return _izabraniTip;
            }
        }

        ObservableCollection<TipLokala> tipovi;

        public TabelaTipova(MainWindow p)
        {
            this.parent = p;
            InitializeComponent();
            this.tekuci_tip = new TipLokala();
            this.DataContext = this.tekuci_tip;
            this.dgrMain.ItemsSource = this.parent.repoTipovi.sviTipovi();
            tipovi = new ObservableCollection<TipLokala>(this.parent.repoTipovi.sviTipovi());
            
        }

        private void Delete(object sender, RoutedEventArgs args)
        {
            /*
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Da li ste sigurni?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                TipLokala tip = (TipLokala)dgrMain.SelectedItem;
                if (tip == null)
                    return;
                parent.repoTipovi.izbaci(tip);
            }*/
            TipLokala tip = (TipLokala)dgrMain.SelectedItem;
            if (tip == null)
                return;
            BrisanjeTipa dialog = new BrisanjeTipa(parent, tip);
            dialog.ShowDialog();
        }

        public void promeniIkonicu(object sender, RoutedEventArgs args)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Izaberite ikonicu";
            fileDialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                                "Portable Network Graphic (*.png)|*.png";
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TipLokala tl = this.tekuci_tip;
                if (tl!= null)
                    tl.Slika = fileDialog.FileName;

            }
        }

        private void Clicked_OK(object sender, RoutedEventArgs args)
        {
            if (dgrMain.SelectedItem != null)
                _izabraniTip = (TipLokala)dgrMain.SelectedItem;
            else
                _izabraniTip = null;
            this.Close();
        }

        public void textFieldChanged(object sender, TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox textbox = sender as System.Windows.Controls.TextBox;
            string filter = textbox.Text;
            ICollectionView cv = CollectionViewSource.GetDefaultView(tipovi);
            if (filter == "")
                cv.Filter = null; 
            else
            {
                string[] words = filter.Split(' ');
                if (words.Contains(""))
                    words = words.Where(word => word != "").ToArray();
                cv.Filter = o =>
                {
                    TipLokala tip = o as TipLokala;
                    return words.Any(word => tip.Oznaka.ToUpper().Contains(word.ToUpper()) || tip.Naziv.ToUpper().Contains(word.ToUpper()) ||
                            tip.Opis.ToUpper().Contains(word.ToUpper()) || tip.Slika.ToUpper().Contains(word.ToUpper()));
                };
                dgrMain.ItemsSource = tipovi;
            }
        }


        public void deleteFilter(object sender, RoutedEventArgs e)
        {
            TextFilter.Text = "";
        }

        private void sacuvajTekuci(object sender, RoutedEventArgs args)
        {
            if (tip_za_izmenu == null)
                return;
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Da li ste sigurni da zelite da sacuvate izmene za tip sa oznakom " + tip_za_izmenu.Oznaka +"?", "Izmena tipa", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                this.tip_za_izmenu.setTipAs(this.tekuci_tip);
                this.parent.repoTipovi.memorisi();

                System.Windows.MessageBox.Show("Uspesna izmena!", "Izmena tipa");
            }
        }

        private void dgr_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            this.tip_za_izmenu = (TipLokala)dgrMain.SelectedItem;
            if (this.tip_za_izmenu != null)
                this.tekuci_tip.setTipAs(TipLokala.getCopyTip(this.tip_za_izmenu));
            else
            {
                this.tekuci_tip = new TipLokala() ;
                this.DataContext = this.tekuci_tip;
            }
        }

        public void PretraziTipove(object sender, RoutedEventArgs e)
        {
            unselectRows();
            string searchText = TextFieldPretraga.Text;
            bool selected = true;
            for (int i = 0; i < dgrMain.Items.Count; i++)
            {
                TipLokala tipLokala = dgrMain.Items[i] as TipLokala;
                if (tipLokala.Oznaka.ToUpper().Equals(searchText.ToUpper()) || tipLokala.Opis.ToUpper().Equals(searchText.ToUpper()) ||
                    tipLokala.Naziv.ToUpper().Equals(searchText.ToUpper()))
                {
                    DataGridRow row = (DataGridRow)dgrMain.ItemContainerGenerator.ContainerFromIndex(i);
                    row.Background = new SolidColorBrush(Colors.DarkSeaGreen);
                    if (selected)
                    {
                        dgrMain.SelectedItem = tipLokala;
                        dgrMain.ScrollIntoView(tipLokala);
                        row.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                        selected = false;
                    }
                }

            }

        }

        public void unselectRows()
        {
            for (int i = 0; i < dgrMain.Items.Count; i++)
            {
                DataGridRow row = (DataGridRow)dgrMain.ItemContainerGenerator.ContainerFromIndex(i);
                row.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void PrikaziPomoc(object sender, RoutedEventArgs args)
        {
            HelpNavigationWindow helpWindow = new HelpNavigationWindow();
            helpWindow.HelpFrame.Navigate(new HelpTabelaTipova());
            helpWindow.ShowDialog();
        }

        private void DeleteSomething_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Delete(null, null);
        }

        private void EnterClicked_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (sacuvajBtn.IsEnabled)
                sacuvajTekuci(null, null);
        }

        private void Escape_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
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
    }
}
