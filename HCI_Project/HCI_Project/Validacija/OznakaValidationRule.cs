using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace HCI_Project.Validacija
{
    public class OznakaValidationRule : ValidationRule
    {
        public MainWindow parent { get; set; }

        public string vrsta { get; set; }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                var oznaka = value as string;
                if (String.IsNullOrWhiteSpace(oznaka))
                {
                    return new ValidationResult(false, "Morate uneti nesto u ovo polje.");
                }
                switch (vrsta)
                {
                    case "LOKAL":
                        if (parent.repoLokali.postojiOznaka(oznaka))
                            return new ValidationResult(false, "Vec postoji lokal sa tom oznakom.");
                        break;
                    case "TIP":
                        if (parent.repoTipovi.postojiOznaka(oznaka))
                            return new ValidationResult(false, "Vec postoji tip lokala sa tom oznakom.");
                        break;
                    case "ETIKETA":
                        if (parent.repoEtikete.postojiOznaka(oznaka))
                            return new ValidationResult(false, "Vec postoji etiketa sa tom oznakom.");
                        break;
                }
                return new ValidationResult(true, null);
            }
            catch
            {
                return new ValidationResult(false, "Dogodila se nepoznata greska.");
            }
        }
    }
}
