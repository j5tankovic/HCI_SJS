using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace HCI_Project.Validacija
{
    public class IntegerValidationRule : ValidationRule
    {

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                var s = value as string;
                int r;
                if (int.TryParse(s, out r))
                {
                    if (r <= 0)
                        return new ValidationResult(false, "Morate uneti broj veci od nule.");
                    return new ValidationResult(true, null);
                }
                return new ValidationResult(false, "Mozete uneti samo ceo broj.");
            }
            catch
            {
                return new ValidationResult(false, "Dogodila se nepoznata greska.");
            }
        }
    }
}
