using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace HCI_Project.NotBeans
{
    
    public class Etiketa : INotifyPropertyChanged,  IEquatable<Etiketa>
    {
        
        private string _oznaka;

        
        private Color _boja;

        
        private string _opis;

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
        
        public Color Boja
        {
            get
            {
                return _boja;
            }
            set
            {
                if (value != _boja)
                {
                    _boja = value;
                    OnPropertyChanged("Boja");
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public bool Equals(Etiketa obj)
        {
            if (this == obj)
                return true;
            if (obj == null)
                return false;
            if (obj.Oznaka.Equals(this.Oznaka))
                return true;
            return false;
        }
    }
}
