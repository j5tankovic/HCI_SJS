using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using HCI_Project.NotBeans;

namespace HCI_Project.Converter
{
    public class TipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
           System.Globalization.CultureInfo culture)
        {
            TipLokala tip = (TipLokala)value;
            if (tip == null)
                return null;
            return tip.Oznaka;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            MainWindow parent = (MainWindow)parameter;
            var oznaka = value as string;
            return parent.repoTipovi.nadjiPoOznaci(oznaka);
        }
    }
}
