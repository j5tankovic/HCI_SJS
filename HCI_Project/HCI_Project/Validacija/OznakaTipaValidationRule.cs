using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace HCI_Project.Validacija
{
    public class OznakaTipaValidationRule : ValidationRule
    {
        public MainWindow parent { get; set; }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                var oznaka = value as string;
                if (String.IsNullOrWhiteSpace(oznaka))
                {
                    return new ValidationResult(false, "Morate uneti nesto u ovo polje.");
                }
                if (parent.repoTipovi.nadjiPoOznaci(oznaka) == null)
                    return new ValidationResult(false, "Izabrali ste nepostojecu oznaku tipa.");
                return new ValidationResult(true, null);
            }
            catch
            {
                return new ValidationResult(false, "Dogodila se nepoznata greska.");
            }
        }

    }
}
