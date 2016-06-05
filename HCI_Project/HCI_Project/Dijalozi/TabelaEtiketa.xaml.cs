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
using Xceed.Wpf.Toolkit;
using System.Collections;
using HCI_Project.Help;

namespace HCI_Project.Dijalozi
{
    /// <summary>
    /// Interaction logic for TabelaEtiketa.xaml
    /// </summary>
    public partial class TabelaEtiketa : Window
    {

        public MainWindow parent { get; set; }
        private Etiketa etiketa_za_izmenu { get; set; }
        private Etiketa tekuca_etiketa { get; set; }

        ObservableCollection<Etiketa> etikete;

        public TabelaEtiketa(MainWindow p)
        {
            this.parent = p;
            InitializeComponent();
            this.tekuca_etiketa = new Etiketa();
            this.DataContext = this.tekuca_etiketa;
            this.dgrMain.ItemsSource = this.parent.repoEtikete.sveEtikete();
            etikete = new ObservableCollection<Etiketa>(this.parent.repoEtikete.sveEtikete());
            
        }

        private void colorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Etiketa etiketa = this.tekuca_etiketa;
            if (etiketa == null)
                return;
            etiketa.Boja = ColorPicker.SelectedColor.Value;
            //azurirajLokale(etiketa);
        }

        private void Delete(object sender, RoutedEventArgs args)
        {
            Etiketa etiketa = (Etiketa)dgrMain.SelectedItem;
            if (etiketa == null)
                return;
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Da li ste sigurni da zelite da obrisete etiketu sa oznakom " + etiketa.Oznaka + "?", "Brisanje etikete", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                parent.repoLokali.izbaciEtiketuIzLokala(etiketa);
                parent.repoEtikete.izbaci(etiketa);
                this.tekuca_etiketa = null;
                this.DataContext = this.tekuca_etiketa;
            }
        }

        public void azurirajLokale(Etiketa etiketa)
        {
            foreach (Lokal l in this.parent.repoLokali.sviLokali())
            {
                if (l.Etikete != null)
                    foreach (Etiketa e in l.Etikete)
                    {
                        if (etiketa.Oznaka.Equals(e.Oznaka))
                        {
                            e.Boja = etiketa.Boja;
                        }
                    }
            }
        }

        public void textFieldChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            string filter = textbox.Text;
            ICollectionView cv = CollectionViewSource.GetDefaultView(etikete);
            if (filter == "")
                cv.Filter = null;
            else
            {
                string[] words = filter.Split(' ');
                if (words.Contains(""))
                    words = words.Where(word => word != "").ToArray();
                cv.Filter = o => 
                {
                    Etiketa etiketa = o as Etiketa;
                    return words.Any(word => etiketa.Oznaka.ToUpper().Contains(word.ToUpper()) || etiketa.Opis.ToUpper().Contains(word.ToUpper())
                           || etiketa.Boja.ToString().ToUpper().Contains(word.ToUpper()));
                };

                dgrMain.ItemsSource = etikete;
            }
        }

        public void colorFilterChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            ICollectionView cv = CollectionViewSource.GetDefaultView(dgrMain.ItemsSource);
            ColorPicker picker = sender as ColorPicker;
            Color? choosedColor = picker.SelectedColor;
            if (choosedColor == null)
                cv.Filter = null;
            else
            {


                cv.Filter = o =>
                {
                    Etiketa etiketa = o as Etiketa;
                    return choosedColor.Equals(etiketa.Boja);
                };
            }

        }

        public void deleteFilter(object sender, RoutedEventArgs e)
        {
            TextFilter.Text = "";
        }

        private void sacuvajTekuci(object sender, RoutedEventArgs args)
        {
            if (etiketa_za_izmenu == null)
                return;
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Da li ste sigurni da zelite da sacuvate izmene za etiketu sa oznakom " + etiketa_za_izmenu.Oznaka +"?", "Update Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                this.etiketa_za_izmenu.setValuesAs(this.tekuca_etiketa);
                this.parent.repoEtikete.memorisi();
                this.azurirajLokale(this.etiketa_za_izmenu);

                System.Windows.MessageBox.Show("Uspesna izmena!");
            }
        }

        private void dgr_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            this.etiketa_za_izmenu = (Etiketa)dgrMain.SelectedItem;
            if (this.etiketa_za_izmenu == null)
                this.tekuca_etiketa = null;
            else
                this.tekuca_etiketa.setValuesAs(Etiketa.getCopy(this.etiketa_za_izmenu));
        }


        public void PretraziEtikete(object sender, RoutedEventArgs e)
        {
                unselectRows();
                string searchText = TextFieldPretraga.Text;
                bool selected = true;
                for(int i=0;i<dgrMain.Items.Count;i++)
                {
                    Etiketa etiketa = dgrMain.Items[i] as Etiketa;
                    if (etiketa.Oznaka.ToUpper().Equals(searchText.ToUpper()) || etiketa.Opis.ToUpper().Equals(searchText.ToUpper()))
                    {
                        DataGridRow row = (DataGridRow)dgrMain.ItemContainerGenerator.ContainerFromIndex(i);
                        row.Background = new SolidColorBrush(Colors.DarkSeaGreen);
                        if (selected)
                        {
                            dgrMain.SelectedItem = etiketa;
                            dgrMain.ScrollIntoView(etiketa);
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
            helpWindow.HelpFrame.Navigate(new HelpTabelaEtiketa());
            helpWindow.ShowDialog();
        }

        private void DeleteSomething_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Delete(null, null);
        }

        private void EnterClicked_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            sacuvajTekuci(null, null);
        }

        private void Escape_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
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
