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

namespace HCI_Project.Help
{
    /// <summary>
    /// Interaction logic for NavigationWindow.xaml
    /// </summary>
    public partial class HelpNavigationWindow : System.Windows.Navigation.NavigationWindow {


        public HelpNavigationWindow()
        {
            InitializeComponent();
        }


        private void DodavanjeEtikete(object sender,RoutedEventArgs args)
        {
            HelpFrame.Navigate(new HelpDialogEtiketa());
        }

        private void DodavanjeTipa(object sender, RoutedEventArgs args)
        {
            HelpFrame.Navigate(new HelpDialogTipLokala());
        }

        private void DodavanjeLokala(object sender, RoutedEventArgs args)
        {
            HelpFrame.Navigate(new HelpDialogLokal());
        }

        private void GlavniProzor(object sender, RoutedEventArgs args)
        {
            HelpFrame.Navigate(new MainPage());
        }

        private void TabelaEtiketa(object sender, RoutedEventArgs args)
        {
            HelpFrame.Navigate(new HelpTabelaEtiketa());
        }

        private void TabelaTipova(object sender, RoutedEventArgs args)
        {
            HelpFrame.Navigate(new HelpTabelaTipova());
        }

        private void TabelaLokala(object sender, RoutedEventArgs args)
        {
            HelpFrame.Navigate(new HelpTabelaLokala());
        }

        private void PrikazMapa(object sender, RoutedEventArgs args)
        {
            HelpFrame.Navigate(new PrikazMapa());
        }
    }
}
