using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using HCI_Project.Dijalozi;
using System.Windows.Input;

namespace HCI_Project.Help
{
    class HelpProvider
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
            determinePage(helpWindow, originator);
        }

        public static void determinePage(HelpNavigationWindow helpWindow,Window originator)
        {
            if (originator.GetType() == typeof(LokalDialog))
            {
                HelpDialogLokal helplokal = new HelpDialogLokal();
                helpWindow.HelpFrame.Navigate(helplokal);
                helpWindow.ShowDialog();
               
            }
            else if (originator.GetType() == typeof(EtiketaDialog))
            {
                HelpDialogEtiketa helpEtiketa = new HelpDialogEtiketa();
                helpWindow.HelpFrame.Navigate(helpEtiketa);
                helpWindow.ShowDialog();

            }
            else if (originator.GetType() == typeof(TipDialog))
            {
                HelpDialogTipLokala helpTip = new HelpDialogTipLokala();
                helpWindow.HelpFrame.Navigate(helpTip);
                helpWindow.ShowDialog();

            }

        }
    }
}
