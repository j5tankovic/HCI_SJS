using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


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
        private bool _hendikep;
        private bool _pusenje;
        private bool _rezervacije;
        private KategorijaCene _cene;
        private int _kapacitet;
        private DateTime _datum;
        private List<Etiketa> _etikete;
        private string _slika;
        private double _pozicijaX = -1;
        private double _pozicijaY = -1;
        private string _mapa;

        public Lokal()
        {
            this._etikete = new List<Etiketa>();
        }

        public void setValuesAs(Lokal l)
        {
            this.Oznaka = l._oznaka;
            this.Naziv = l._naziv;
            this.Opis = l._opis;
            this.Tip = l._tip;
            this.Alkohol = l._alhohol;
            this.Hendikep = l._hendikep;
            this.Pusenje = l._pusenje;
            this.Rezervacije = l._rezervacije;
            this.Cene = l._cene;
            this.Kapacitet = l._kapacitet;
            this.Etikete = l._etikete;
            this.Slika = l._slika;
            this.PozicijaX = l._pozicijaX;
            this.PozicijaY = l._pozicijaY;
            this.Mapa = l._mapa;
            this.Datum = l._datum;
        }

        public static Lokal getCopyLokal(Lokal l)
        {
            if (l == null)
                return null;
            Lokal novi = new Lokal();
            novi.Oznaka = l._oznaka;
            novi.Alkohol = l._alhohol;
            novi.Cene = l._cene;
            novi.Datum = l._datum;
            novi.Etikete = new List<Etiketa>();
            if (l._etikete != null)
             foreach (Etiketa e in l._etikete)
                novi.Etikete.Add(e);
            novi.Hendikep = l._hendikep;
            novi.Kapacitet = l._kapacitet;
            novi.Mapa = l._mapa;
            novi.Naziv = l._naziv;
            novi.Opis = l._opis;
            novi.PozicijaX = l._pozicijaX;
            novi.PozicijaY = l._pozicijaY;
            novi.Pusenje = l._pusenje;
            novi.Rezervacije = l._rezervacije;
            novi.Slika = l._slika;
            novi.Tip = l._tip;
            return novi;
        }

        public string Mapa
        {
            get { return _mapa; }
            set
            {
                if (value != _mapa)
                {
                    _mapa = value;
                    OnPropertyChanged("Mapa");
                }
            }
        }

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
        
        public bool Hendikep
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
        
        public bool Pusenje
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
        
        public bool Rezervacije
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
        
        public DateTime Datum
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

        public double PozicijaX
        {
            get
            {
                return _pozicijaX;
            }
            set
            {
                if (value != _pozicijaX)
                    _pozicijaX = value;
                OnPropertyChanged("PozicijaX");
            }
        }

        public double PozicijaY
        {
            get
            {
                return _pozicijaY;
            }
            set
            {
                if (value != _pozicijaY)
                    _pozicijaY = value;
                OnPropertyChanged("PozicijaY");
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
            if (this == obj)
                return true;
            if (obj == null)
                return false;
            if (obj.Oznaka.Equals(this.Oznaka))
                return true;
            return false;
        }

        public void izmeniTipPotpuno(TipLokala t)
        {
            TipLokala tip = this.Tip;
            tip.izbaciLokal(this);
            this.Tip = t;
            if (t != null)
                t.ubaciLokal(this);
        }
    }
}
