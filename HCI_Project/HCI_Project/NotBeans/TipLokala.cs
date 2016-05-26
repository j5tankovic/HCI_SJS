using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Newtonsoft.Json;

namespace HCI_Project.NotBeans
{
    public class TipLokala : INotifyPropertyChanged, IEquatable<TipLokala>
    {
        private string _oznaka;
        private string _naziv;
        private string _opis;
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

        public string Slika
        {
            get
            {
                return _slika;
            }
            set
            {
                if (value != _slika)
                {
                    _slika = value;
                    OnPropertyChanged("Slika");
                }
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

        [JsonIgnore]
        public ObservableCollection<Lokal> Lokali
        {
            get;
            set;
        }

        public TipLokala()
        {
            Lokali = new ObservableCollection<Lokal>();
        }

        public bool Equals(TipLokala obj)
        {
            if (obj.Oznaka.Equals(this.Oznaka))
                return true;
            return false;
        }

        public void izbaciLokal(Lokal l)
        {
            foreach (Lokal lok in Lokali)
            {
                if (lok.Oznaka.Equals(l.Oznaka))
                {
                    Lokali.Remove(lok);
                    break;
                }
            }
        }

        public void ubaciLokal(Lokal l)
        {
            foreach (Lokal lokal in Lokali)
            {
                if (lokal.Equals(l))
                    return;
            }
            Lokali.Add(l);
        }
    }
}
