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
using System.Collections.ObjectModel;

namespace HCI_Project.Dijalozi
{
    /// <summary>
    /// Interaction logic for TabelaLokala.xaml
    /// </summary>
    public partial class TabelaLokala : Window
    {
        public ObservableCollection<Lokal> Lokali
        {
            get;
            set;
        }
        public TabelaLokala()
        {
            InitializeComponent();
            this.DataContext = this;
            Lokali = new ObservableCollection<Lokal>();
            Lokali.Add(new Lokal ());
        }
    }
}
