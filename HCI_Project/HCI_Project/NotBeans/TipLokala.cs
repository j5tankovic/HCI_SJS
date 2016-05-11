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

    public class AddCommand : ICommand
    {
        private TipLokala tip;
        public AddCommand(TipLokala s)
        {
            tip = s;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            tip.Lokali.Add(new Lokal() { Naziv = "Novi Lokal" });
        }
    }

    public class TipLokala : INotifyPropertyChanged, IEquatable<TipLokala>
    {
        private string _oznaka;
        private string _naziv;
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

        private AddCommand _add;
        public AddCommand Add
        {
            get
            {
                return _add;
            }
            set
            {
                if (_add != value)
                {
                    _add = value;
                    OnPropertyChanged("Add");
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
            Add = new AddCommand(this);
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
    }
}
