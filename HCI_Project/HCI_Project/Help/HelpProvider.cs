using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using HCI_Project.Dijalozi;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Documents;

namespace HCI_Project.Help
{
    public class HelpProvider
    {
        public static string GetHelpKey(DependencyObject obj)
        {
            return obj.GetValue(HelpKeyProperty) as string;
        }

        public static void SetHelpKey(DependencyObject obj, string value)
        {
            obj.SetValue(HelpKeyProperty, value);
        }

        public static readonly DependencyProperty HelpKeyProperty =
            DependencyProperty.RegisterAttached("HelpKey", typeof(string), typeof(HelpProvider), new PropertyMetadata("index", HelpKey));
        private static void HelpKey(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //NOOP
        }

        public static void ShowHelp(string key, Window originator)
        {
            HelpNavigationWindow helpWindow = new HelpNavigationWindow();
            determinePage(key,helpWindow, originator);
        }

        public static void determinePage(string key,HelpNavigationWindow helpWindow,Window originator)
        {
            if (originator.GetType() == typeof(LokalDialog))
            {
                LokalDialog lokalDialog = originator as LokalDialog;
                HelpDialogLokal helplokal = new HelpDialogLokal();
                TextRange textRange = new TextRange(helplokal.Naslov.ContentStart, helplokal.Naslov.ContentEnd);
                if (!lokalDialog.Kreiranje)
                    textRange.Text = "Izmena lokala";
                if (key.Equals("index"))
                {
                    helpWindow.HelpFrame.Navigate(helplokal);
                }
                else
                {
                    Uri helpUri = new Uri("pack://application:,,,/Help/HelpDialogLokal.xaml#" + key);
                    helpWindow.HelpFrame.Navigate(helpUri);
                }
                helpWindow.ShowDialog();
               
            }
            else if (originator.GetType() == typeof(EtiketaDialog))
            {
                HelpDialogEtiketa helpEtiketa = new HelpDialogEtiketa();
                if (key.Equals("index"))
                {
                    helpWindow.HelpFrame.Navigate(helpEtiketa);
                }
                else
                {
                    Uri helpUri = new Uri("pack://application:,,,/Help/HelpDialogEtiketa.xaml#" + key);
                    helpWindow.HelpFrame.Navigate(helpUri);
                }
                helpWindow.ShowDialog();

            }
            else if (originator.GetType() == typeof(TipDialog))
            {
                HelpDialogTipLokala helpTip = new HelpDialogTipLokala();
                TipDialog tipDialog = originator as TipDialog;
                TextRange textRange = new TextRange(helpTip.Naslov.ContentStart, helpTip.Naslov.ContentEnd);
                if (!tipDialog.Kreiranje)
                    textRange.Text = "Izmena tipa lokala";
                if (key.Equals("index"))
                {
                    helpWindow.HelpFrame.Navigate(helpTip);
                }
                else
                {
                    Uri helpUri = new Uri("pack://application:,,,/Help/HelpDialogTipLokala.xaml#" + key);
                    helpWindow.HelpFrame.Navigate(helpUri);
                }
                helpWindow.ShowDialog();

            }
            else if (originator.GetType() == typeof(TabelaLokala))
            {
                HelpTabelaLokala helpTabelaLokala = new HelpTabelaLokala();
                if (key.Equals("index"))
                {
                    helpWindow.HelpFrame.Navigate(helpTabelaLokala);
                }
                else
                {
                    Uri helpUri = new Uri("pack://application:,,,/Help/HelpTabelaLokala.xaml#" + key);
                    helpWindow.HelpFrame.Navigate(helpUri);
                }
                helpWindow.ShowDialog();
            }
            else if (originator.GetType() == typeof(TabelaTipova))
            {
                HelpTabelaTipova helpTabelaTipova = new HelpTabelaTipova();
                if (key.Equals("index"))
                {
                    helpWindow.HelpFrame.Navigate(helpTabelaTipova);
                }
                else
                {
                    Uri helpUri = new Uri("pack://application:,,,/Help/HelpTabelaTipova.xaml#" + key);
                    helpWindow.HelpFrame.Navigate(helpUri);
                }
                helpWindow.ShowDialog();
            }
            else if (originator.GetType() == typeof(TabelaEtiketa))
            {
                HelpTabelaEtiketa helpTabelaEtiketa = new HelpTabelaEtiketa();
                if (key.Equals("index"))
                {
                    helpWindow.HelpFrame.Navigate(helpTabelaEtiketa);
                }
                else
                {
                    Uri helpUri = new Uri("pack://application:,,,/Help/HelpTabelaEtiketa.xaml#" + key);
                    helpWindow.HelpFrame.Navigate(helpUri);
                }
                helpWindow.ShowDialog();
            }
            else if (originator.GetType() == typeof(MainWindow))
            {
                PrikazMapa mapa = new PrikazMapa();
                if (key.Equals("index"))
                {
                    helpWindow.HelpFrame.Navigate(mapa);
                }
                else
                {
                    Uri helpUri = new Uri("pack://application:,,,/Help/PrikazMapa.xaml#" + key);
                    helpWindow.HelpFrame.Navigate(helpUri);
                }
                helpWindow.ShowDialog();
            }

        }
    }
}
