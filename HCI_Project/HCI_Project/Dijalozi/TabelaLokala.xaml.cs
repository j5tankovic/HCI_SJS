﻿using System;
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
    /// Interaction logic for TabelaLokala.xaml
    /// </summary>
    public partial class TabelaLokala : Window
    {
        public MainWindow parent { get; set; }
        private string stariTipOznaka { get; set; }

        private Lokal lokal_za_izmenu { get; set; }
        private Lokal tekuci_lokal { get; set; }

        ObservableCollection<Lokal> lokali;

        string[] positive = { "SLUZI DO 23", "DO KASNO NOCU", "IZUZETNO VISOKA", "VISOKA", "SREDNJA", "NISKA", "DOZVOLJENO PUSENJE", "PRIMA REZERVACIJE", "ZA HENDIKEPIRANE", "NE SLUZI", "DA HENDIKEP"};
        string[] negative = { "BEZ PUSENJA", "NE PUSENJE", "NE PRIMA REZERVACIJE", "NE REZERVACIJE", "NE ZA HENDIKEPIRANE", "NE HENDIKEP"};

  
        public TabelaLokala(MainWindow p)
        {
            this.parent = p;
            this.tekuci_lokal = new Lokal();
            this.Resources.Add("parent", parent);
            this.DataContext = tekuci_lokal;
            InitializeComponent();
            initializeCombos();
            dgrMain.ItemsSource = this.parent.repoLokali.sviLokali();
            lokali = new ObservableCollection<Lokal>(this.parent.repoLokali.sviLokali());
            

        }

        private void initializeCombos()
        {
            comboAlkohol.ItemsSource = new string[] { "NE SLUZI", "SLUZI DO 23", "DO KASNO NOCU" };
            comboCene.ItemsSource = new string[] {"IZUZETNO VISOKA", "VISOKA", "SREDNJA", "NISKA"};

            
        }

        private void Delete(object sender, RoutedEventArgs args)
        {
            Lokal lokal = (Lokal)dgrMain.SelectedItem;
            if (lokal == null)
                return;
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Da li ste sigurni da zelite da obrisete lokal sa oznakom " + lokal.Oznaka + "?", "Brisanje lokala", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                parent.removeLokalFromMap(lokal);
                parent.repoLokali.izbaci(lokal);
                TipLokala tip = parent.nadjiTipLokala(lokal);
                if (tip != null)
                    tip.izbaciLokal(lokal);
            }
        }


        public void promeniIkonicu(object sender, RoutedEventArgs args)
        {
            if (dgrMain.SelectedItem != null)
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Title = "Izaberite ikonicu";
                fileDialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                                    "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                                    "Portable Network Graphic (*.png)|*.png";
                if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Lokal l = this.tekuci_lokal;
                    if (l != null)
                    {
                        l.Slika = fileDialog.FileName;
                    }

                }
            }
        }

        private void izberiEtikete(object sender, RoutedEventArgs args)
        {
            if (dgrMain.SelectedItem != null)
            {
                //this.DataContext = (Lokal)dgrMain.SelectedItem;
                Etikete etikete = new Etikete(this, this.parent.repoEtikete);
                etikete.ShowDialog();
                //this.DataContext = this.tekuci_lokal;
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void otvoriTabeluTipova(object sender, RoutedEventArgs args)
        {
            if (this.tekuci_lokal.Oznaka == null)
                return;
            TabelaTipova tabela = new TabelaTipova(this.parent);
            tabela.ShowDialog();
            TipLokala tip = tabela.IzabraniTip;
            if (tip != null)
            {
                Lokal lokal = this.tekuci_lokal;
                if (lokal.Oznaka == null)
                    return;
                NazivTipa.Text = tip.Naziv;
                OznakaTipa.Text = tip.Oznaka;
                lokal.Tip = tip;
            }            
        }

        private void oznakaTipa_TextChanged(object sender, RoutedEventArgs args)
        {
            System.Windows.Controls.TextBox box = (System.Windows.Controls.TextBox)sender;
            TipLokala tip = parent.repoTipovi.nadjiPoOznaci(box.Text);
            if (tekuci_lokal == null)
                return;
            tekuci_lokal.Tip = tip;
            OznakaTipa.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty).UpdateSource();
        }

        private void dgrMain_SelectedCellsChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgrMain.SelectedItem != null)
            {
                this.stariTipOznaka = ((Lokal)dgrMain.SelectedItem).Tip.Oznaka;

                this.lokal_za_izmenu = (Lokal)dgrMain.SelectedItem;
                Lokal kopija = Lokal.getCopyLokal((Lokal)dgrMain.SelectedItem);
                this.tekuci_lokal.setValuesAs(kopija);
            }
            else
            {
                this.tekuci_lokal = new Lokal() ;
                this.DataContext = this.tekuci_lokal;
            }

        }

       



        public void textFieldChanged(object sender, TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox textbox = sender as System.Windows.Controls.TextBox;
            string filter = textbox.Text;
            ICollectionView cv = CollectionViewSource.GetDefaultView(lokali);
            if (filter == "")
                cv.Filter = null;
            else
            {
                cv.Filter = o =>
                {
                    Lokal lokal = o as Lokal;
                    string[] words = filter.Split(' ');
                    if (words.Contains(""))
                        words = words.Where(word => word != "").ToArray();
                    return words.Any(word => lokal.Oznaka.ToUpper().Contains(word.ToUpper()) || lokal.Naziv.ToUpper().Contains(word.ToUpper()) ||
                             (lokal.Opis != null && lokal.Opis.ToUpper().Contains(word.ToUpper())) || lokal.Slika.ToUpper().Contains(word.ToUpper()) ||
                            lokal.Tip.Naziv.ToUpper().Contains(word.ToUpper())) || FilterOstalihPolja(filter,lokal);
                };

                dgrMain.ItemsSource = lokali;
            }
        }

      
        public void deleteFilter(object sender, RoutedEventArgs e)
        {
            TextFilter.Text = "";
        }

       

        private void sacuvajTekuci(object sender, RoutedEventArgs args)
        {
            if (this.lokal_za_izmenu == null)
                return;
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Da li ste sigurni da zelite da sacuvate izmene za lokal sa oznakom " + lokal_za_izmenu.Oznaka +"?", "Izmena lokala", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                this.lokal_za_izmenu.izmeniTipPotpuno(this.tekuci_lokal.Tip);
                this.lokal_za_izmenu.setValuesAs(this.tekuci_lokal);
                this.parent.repoLokali.memorisi();
                this.parent.repoTipovi.memorisi();

                System.Windows.MessageBox.Show("Izmena uspesna!", "Izmena lokala");
            }
        }

        public void PretraziLokale(object sender, RoutedEventArgs e)
        {
            unselectRows();
            string searchText = TextFieldPretraga.Text;
            bool selected = true;
            for (int i = 0; i < dgrMain.Items.Count; i++)
            {
                Lokal lokal = dgrMain.Items[i] as Lokal;
                if (lokal.Oznaka.ToUpper().Equals(searchText.ToUpper()) || (lokal.Opis != null && lokal.Opis.ToUpper().Contains(searchText.ToUpper())) ||
                    lokal.Naziv.ToUpper().Equals(searchText.ToUpper()) || lokal.Tip.Naziv.ToUpper().Equals(searchText.ToUpper()) || FilterOstalihPolja(searchText,lokal))
                {
                    DataGridRow row = (DataGridRow)dgrMain.ItemContainerGenerator.ContainerFromIndex(i);
                    row.Background = new SolidColorBrush(Colors.DarkSeaGreen);
                    if (selected)
                    {
                        dgrMain.SelectedItem = lokal;
                        dgrMain.ScrollIntoView(lokal);
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
            helpWindow.HelpFrame.Navigate(new HelpTabelaLokala());
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


        private bool FilterOstalihPolja(string filter, Lokal lokal)
        {
             if (positive.Any(s => filter.ToUpper().Contains(s.ToUpper())))
            {
                //rec je o dozvoli za pusenje
                if (filter.ToUpper().Contains("PUSENJE"))
                {
                    if (lokal.Pusenje) return true;
                }
                else if (filter.ToUpper().Contains("REZERVACIJE"))
                {
                    if (lokal.Rezervacije) return true;
                }
                else if (filter.ToUpper().Contains("HENDIKEPIRANE") || filter.ToUpper().Contains("HENDIKEP"))
                {
                    if (lokal.Hendikep) return true;
                }
                else 
                    return filter.ToUpper().Equals(EnumAkoholToStr(lokal).ToUpper()) || filter.ToUpper().Equals(EnumCeneToStr(lokal).ToUpper());

            }
            else if (negative.Any(s => filter.ToUpper().Contains(s.ToUpper())))
            {
                if (filter.ToUpper().Contains("PUSENJE") || filter.ToUpper().Contains("PUSENJA"))
                {
                    if (!lokal.Pusenje) return true;
                }
                else if (filter.ToUpper().Contains("REZERVACIJE"))
                {
                    if (!lokal.Rezervacije) return true;
                }
                else if (filter.ToUpper().Contains("HENDIKEPIRANE") || filter.ToUpper().Contains("HENDIKEP"))
                {
                    if (!lokal.Hendikep) return true;
                }
            }
            return false;

        }


        private string EnumAkoholToStr(Lokal l)
        {
           switch(l.Alkohol)
           {
               case SluzenjeAlkohola.DO_KASNO_NOCU:
                   return "DO KASNO NOCU";
               case SluzenjeAlkohola.NE_SLUZI:
                   return "NE SLUZI";
               case SluzenjeAlkohola.SLUZI_DO_23:
                   return "SLUZI DO 23";
               default: return "";
           }
        }

        private string EnumCeneToStr(Lokal l)
        {
           switch(l.Cene)
           {
               case  KategorijaCene.IZUZETNO_VISOKA:
                   return "IZUZETNO VISOKA";
               case KategorijaCene.NISKA:
                   return "NISKA";
               case KategorijaCene.SREDNJA:
                   return "SREDNJA";
               case KategorijaCene.VISOKA:
                   return "VISOKA";
               default: return "";
           }
        }
      
    }

}