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
using System.Windows.Navigation;
using System.Windows.Shapes;
using HCI_Project.Dijalozi;

namespace HCI_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //InitializeComponent();

            EtiketaDialog ed = new EtiketaDialog();
            ed.InitializeComponent();
            ed.Show();

            LokalDialog ld = new LokalDialog();
            ld.InitializeComponent();
            ld.Show();

            TipDialog td = new TipDialog();
            td.InitializeComponent();
            td.Show();

            TabelaLokala tl = new TabelaLokala();
            tl.InitializeComponent();
            tl.Show();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
