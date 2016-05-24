using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using HCI_Project.NotBeans;

namespace HCI_Project.Library
{
    public class CustomLokalConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                if (value.GetType() == typeof(TipLokala))
                {
                    TipLokala tip = (TipLokala)value;
                    return tip;
                }
                else
                {
                    Lokal lokal = (Lokal)value;
                    return lokal;
                }
            } return "";

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return "";
        }

        #endregion
    }
}
