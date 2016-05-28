using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using HCI_Project.NotBeans;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows;

namespace HCI_Project.Repos
{
    public class RepoTipovi
    {
        public ObservableCollection<TipLokala> tipovi { get; set; }
        private readonly string datoteka;


        public RepoTipovi()
        {
            tipovi = new ObservableCollection<TipLokala>();
            datoteka = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tipovi.txt");
            ucitaj();
        }


        //Metoda za rad sa faljovima i serijalizacijom
        public void ucitaj()
        {

            if (File.Exists(datoteka))
            {

                using (StreamReader reader = File.OpenText(datoteka))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    tipovi = (ObservableCollection<TipLokala>)serializer.Deserialize(reader, typeof(ObservableCollection<TipLokala>));
                }
            }
            else
            {
                tipovi = new ObservableCollection<TipLokala>();
            }
        }



        public void memorisi()
        {

            using (StreamWriter writer = File.CreateText(datoteka))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, tipovi);
            }
        }


        //Metode za dodavanje i brisanje

        public bool dodaj(TipLokala tipLokala)
        {
            try
            {
                TipLokala temp = tipovi.Single(tip => tip.Oznaka == tipLokala.Oznaka);
                return false;
            }
            catch (InvalidOperationException)
            {
                tipovi.Add(tipLokala);
                memorisi();
                return true;
            }
            
        }


        public void obrisi(String oznaka)
        {
            foreach (TipLokala t in tipovi)
            {
                if (t.Oznaka.Equals(oznaka))
                    tipovi.Remove(t);
            }
            memorisi();
        }


        public TipLokala this[string oznaka]
        {
            get
            {
                try
                {
                    return tipovi.Single(tip => tip.Oznaka == oznaka);
                }
                catch (InvalidOperationException) { return null; }
            }
            set
            {
                try
                {
                    TipLokala temp = tipovi.Single(tip => tip.Oznaka == oznaka);
                    temp = value;
                }
                catch (InvalidOperationException) { MessageBox.Show("Nije uspeo set!"); }
            }
        }

        public ObservableCollection<TipLokala> sviTipovi()
        {
            return tipovi;
        }

        public TipLokala nadji(TipLokala t)
        {
            foreach (TipLokala tip in tipovi)
            {
                if (tip.Oznaka.Equals(t.Oznaka))
                    return tip;
            }
            return null;
        }

        public TipLokala nadjiPoOznaci(string oznaka)
        {
            foreach (TipLokala tip in tipovi)
            {
                if (tip.Oznaka.Equals(oznaka))
                    return tip;
            }
            return null;
        }

        public void izbaci(TipLokala tip)
        {
            for (int i = 0; i < tipovi.Count; i++)
            {
                TipLokala l = tipovi[i];
                if (l.Oznaka.Equals(tip.Oznaka))
                {
                    tipovi.RemoveAt(i);
                    return;
                }
            }
        }

        public void zameniTip(Collection<Lokal> lista, TipLokala noviTip)
        {
            foreach (Lokal l in lista)
            {
                l.Tip = noviTip;
                noviTip.ubaciLokal(l);
            }
        }

        public bool postojiOznaka(string o, TipLokala ti)
        {
            foreach (TipLokala t in tipovi)
            {
                if (t.Oznaka.Equals(o) && !t.Equals(ti))
                    return true;
            }
            return false;
        }

        public void nadjiTipPoOznaciIIzbaciLokal(string o, Lokal l)
        {
            TipLokala tip = nadjiPoOznaci(o);
            if (tip == null)
                return;
            tip.izbaciLokal(l);
        }
    }
}
