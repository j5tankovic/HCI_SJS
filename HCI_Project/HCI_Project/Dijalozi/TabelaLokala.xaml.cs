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
using HCI_Project.Beans;

namespace HCI_Project.Dijalozi
{
    /// <summary>
    /// Interaction logic for TabelaLokala.xaml
    /// </summary>
    public partial class TabelaLokala : Window
    {
        private MainWindow parent;

        public ObservableCollection<Lokal> lokali
        {
            get;
            set;
        }
        
        public TabelaLokala(MainWindow p)
        {
            this.parent = p;
            lokali = new ObservableCollection<Lokal>();
            parent.repoLokali.lokali.ToList().ForEach(lokali.Add);
            InitializeComponent();
            this.DataContext = this;
            initializeCombos();
        }

        private void initializeCombos()
        {
            comboAlkohol.ItemsSource = new string[] { "NE SLUZI", "SLUZI DO 23", "DO KASNO NOCU" };
            comboCene.ItemsSource = new string[] {"IZUZETNO VISOKA", "VISOKA", "SREDNJA", "NISKA"};
            comboTipovi.ItemsSource = parent.repoTipovi.sviTipovi();

            
        }

        private void Delete(object sender, RoutedEventArgs args)
        {
            Lokal lokal = (Lokal)dgrMain.SelectedItem;
            if (lokal == null)
                return;
            izbaci(lokal);
            parent.repoLokali.izbaci(lokal);
            TipLokala tip = parent.nadjiTipLokala(lokal);
            if (tip != null)
                tip.izbaciLokal(lokal);
        }

        private void izbaci(Lokal lokal)
        {
            for (int i = 0; i < lokali.Count; i++)
            {
                Lokal l = lokali[i];
                if (l.Oznaka.Equals(lokal.Oznaka))
                {
                    lokali.RemoveAt(i);
                    return;
                }
            }
        }
      
    }

}