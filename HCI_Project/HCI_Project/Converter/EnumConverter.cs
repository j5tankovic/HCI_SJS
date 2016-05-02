using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using HCI_Project.Beans;

namespace HCI_Project.Converter
{
    public class EnumConverter : IValueConverter
    {
       public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value.GetType().IsAssignableFrom(typeof(SluzenjeAlkohola)))
            {
                SluzenjeAlkohola s = (SluzenjeAlkohola) value;
                if (s == SluzenjeAlkohola.DO_KASNO_NOCU)
                    return "DO KASNO NOCU";
                else if (s == SluzenjeAlkohola.NE_SLUZI)
                    return "NE SLUZI";
                else return "SLUZI DO 23";
            }
            else if (value.GetType().IsAssignableFrom(typeof(KategorijaCene)))
            {
                KategorijaCene k = (KategorijaCene)value;
                if (k == KategorijaCene.IZUZETNO_VISOKA)
                    return "IZUZETNO VISOKA";
                else if (k == KategorijaCene.NISKA)
                    return "NISKA";
                else if (k == KategorijaCene.SREDNJA)
                    return "SREDNJA";
                else return "VISOKA";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            String str = (String)value;
            switch (str)
            {
                case "DO KASNO NOCU":
                    return SluzenjeAlkohola.DO_KASNO_NOCU;
                case "SLUZI DO 23":
                    return SluzenjeAlkohola.SLUZI_DO_23;
                case "NE SLUZI":
                    return SluzenjeAlkohola.NE_SLUZI;
                case "IZUZETNO VISOKA":
                    return KategorijaCene.IZUZETNO_VISOKA;
                case "VISOKA":
                    return KategorijaCene.VISOKA;
                case "SREDNJA":
                    return KategorijaCene.SREDNJA;
                case "NISKA":
                    return KategorijaCene.NISKA;
            }
            return null;
        }
    }
}
