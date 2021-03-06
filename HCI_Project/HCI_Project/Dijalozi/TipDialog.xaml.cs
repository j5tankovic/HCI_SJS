﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
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
    /// Interaction logic for TipDialog.xaml
    /// </summary>
    public partial class TipDialog : Window
    {
        public TipLokala tipLokala { get; set; }
        private TipLokala za_izmenu;
        private bool ok = false;
        private bool kreiranje;

        public bool Kreiranje
        {
            get { return kreiranje; }
            set { kreiranje = value; }
        }

        public TipLokala TipLokala
        {
            get { return tipLokala; }
            set { tipLokala = value; }
        }
        private MainWindow parent;

        public TipDialog(MainWindow p, TipLokala t)
        {
            this.parent = p;
            this.Resources.Add("parent", parent);
            this.kreiranje = t == null ? true : false;
            this.za_izmenu = t;
            if (t == null)
            {
                tipLokala = new TipLokala();
                this.Title = "Unos tipa lokala";
            }
            else
            {
                tipLokala = TipLokala.getCopyTip(t);
                this.Title = "Izmena tipa " + t.Oznaka;
            }
            this.Resources.Add("tip", tipLokala);
            this.DataContext = tipLokala;
            InitializeComponent();
            if (!kreiranje)
            {
                oznakaTipa.Background = new SolidColorBrush(Colors.WhiteSmoke);
                oznakaTipa.IsReadOnly = true;
            }
        }


        private void ButtonPotvrdiClicked(object sender, RoutedEventArgs args)
        {
            if (!checkForm())
                return;
            string poruka = kreiranje ? "Da li ste sigurni da zelite da unesete ovaj tip?" : "Da li ste sigurni da zelite da izmenite ovaj tip?";
            string naslov = kreiranje ? "Unos tipa" : "Izmena tipa";
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(poruka, naslov, System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.No)
                return;
            if (!kreiranje)
            {
                this.za_izmenu.setTipAs(this.tipLokala);
                System.Windows.MessageBox.Show("Izmena uspesna!", "Izmena tipa");
                this.Close();
                return;
            }
            ok = parent.repoTipovi.dodaj(tipLokala);
            if (ok)
            {
                System.Windows.MessageBox.Show("Uspešno ste dodali novi tip lokala", "Dodavanje tipa lokala");
                this.Close();
            }
            else
                System.Windows.MessageBox.Show("Vec postoji tip sa tom oznakom! Molimo izaberite drugu.", "Greska!");
        }

        private void ButtonOdustaniClicked(object sender, RoutedEventArgs args)
        {
            ok = false;
            this.Close();
        }

        void TipDialog_Closing(object sender, CancelEventArgs e)
        {
            if (!ok)
                tipLokala = null;
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
                {
                    
                    tipLokala.Slika = filename;
                }
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
                tipLokala.Slika = fileDialog.FileName;

            }
        }

        private void TipDialog_Loaded(object sender, RoutedEventArgs args)
        {
            oznakaTipa.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty).UpdateSource();
            nazivTipa.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty).UpdateSource();
        }

        private void PrikaziPomoc(object sender, RoutedEventArgs args)
        {
            HelpNavigationWindow helpWindow = new HelpNavigationWindow();
            HelpDialogTipLokala helpTip = new HelpDialogTipLokala();
            TextRange textRange = new TextRange(helpTip.Naslov.ContentStart, helpTip.Naslov.ContentEnd);
            if (!kreiranje)
                textRange.Text = "Izmena tipa lokala";
            helpWindow.HelpFrame.Navigate(helpTip);
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

        private bool checkForm()
        {
            if (tipLokala.Slika == null)
            {
                System.Windows.MessageBox.Show("Niste izabrali ikonicu.", "Greska!");
                return false;
            }
            return true;
        }

        private void EnterClicked_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (saveBtn.IsEnabled)
                ButtonPotvrdiClicked(null, null);
        }

        private void Escape_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
        
    }
}
