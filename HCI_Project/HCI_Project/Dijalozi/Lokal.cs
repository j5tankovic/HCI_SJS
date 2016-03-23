using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace HCI_Project.Dijalozi
{
    public class Lokal : INotifyPropertyChanged
    {
        private string oznaka { get; set; }
        private string naziv { get; set; }
        private string opis { get; set; }
        private string tip { get; set; }
        private Boolean alhohol { get; set; }
        private Boolean hendikep { get; set; }
        private Boolean pusenje { get; set; }
        private Boolean rezervacije { get; set; }
        private string cene { get; set; }
        private int kapacitet { get; set; }
        private string datum { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }


    }
}
