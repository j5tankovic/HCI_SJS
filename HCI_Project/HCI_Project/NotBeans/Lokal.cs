using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace HCI_Project.NotBeans
{
    public enum SluzenjeAlkohola
    {
        NE_SLUZI, SLUZI_DO_23, DO_KASNO_NOCU
    }

    public enum KategorijaCene
    {
        NISKA, SREDNJA, VISOKA, IZUZETNO_VISOKA
    }

    public class Lokal : INotifyPropertyChanged, IEquatable<Lokal>
    {
        private string _oznaka;
        private string _naziv;
        private string _opis;
        private TipLokala _tip;
        private SluzenjeAlkohola _alhohol;
        private Boolean _hendikep;
        private Boolean _pusenje;
        private Boolean _rezervacije;
        private KategorijaCene _cene;
        private int _kapacitet;
        private string _datum;
        private List<Etiketa> _etikete;
        private string _slika;

        public string Oznaka
        {
            get
            {
                return _oznaka;
            }
            set
            {
                if (value != _oznaka)
                {
                    _oznaka = value;
                    OnPropertyChanged("Oznaka");
                }
            }
        }
        
        public string Naziv
        {
            get
            {
                return _naziv;
            }
            set
            {
                if (value != _naziv)
                {
                    _naziv = value;
                    OnPropertyChanged("Naziv");
                }
            }
        }
        
        public string Opis
        {
            get
            {
                return _opis;
            }
            set
            {
                if (value != _opis)
                {
                    _opis = value;
                    OnPropertyChanged("Opis");
                }
            }
        }
        
        public TipLokala Tip
        {
            get
            {
                return _tip;
            }
            set
            {
                if (value != _tip)
                {
                    _tip = value;
                    OnPropertyChanged("Tip");
                }
            }
        }
        
        public SluzenjeAlkohola Alkohol
        {
            get
            {
                return _alhohol;
            }
            set
            {
                if (value != _alhohol)
                {
                    _alhohol = value;
                    OnPropertyChanged("Alkohol");
                }
            }
        }
        
        public Boolean Hendikep
        {
            get
            {
                return _hendikep;
            }
            set
            {
                if (value != _hendikep)
                {
                    _hendikep = value;
                    OnPropertyChanged("Hendikep");
                }
            }
        }
        
        public Boolean Pusenje
        {
            get
            {
                return _pusenje;
            }
            set
            {
                if (value != _pusenje)
                {
                    _pusenje = value;
                    OnPropertyChanged("Pusenje");
                }
            }
        }
        
        public Boolean Rezervacije
        {
            get
            {
                return _rezervacije;
            }
            set
            {
                if (value != _rezervacije)
                {
                    _rezervacije = value;
                    OnPropertyChanged("Rezervacije");
                }
            }
        }
        
        public KategorijaCene Cene
        {
            get
            {
                return _cene;
            }
            set
            {
                if (value != _cene)
                {
                    _cene = value;
                    OnPropertyChanged("Cene");
                }
            }
        }
        
        public int Kapacitet
        {
            get
            {
                return _kapacitet;
            }
            set
            {
                if (value != _kapacitet)
                {
                    _kapacitet = value;
                    OnPropertyChanged("Kapacitet");
                }
            }
        }
        
        public string Datum
        {
            get
            {
                return _datum;
            }
            set
            {
                if (value != _datum)
                {
                    _datum = value;
                    OnPropertyChanged("Datum");
                }
            }
        }

        public List<Etiketa> Etikete
        {
            get
            {
                return _etikete;
            }
            set
            {
                if (value != _etikete)
                {
                    _etikete = value;
                    OnPropertyChanged("Etikete");
                }
            }
        }

        public String Slika
        {
            get
            {
                return _slika;
            }
            set
            {
                if (value != _slika)
                    _slika = value;
                OnPropertyChanged("Slika");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public bool Equals(Lokal obj)
        {
            if (obj.Oznaka.Equals(this.Oznaka))
                return true;
            return false;
        }
    }
}
