using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HCI_Project.NotBeans;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows;

namespace HCI_Project.Repos
{
    public class RepoLokali
    {
        public ObservableCollection<Lokal> lokali { get; set; }

        private readonly string datoteka;


        public RepoLokali()
        {
            lokali = new ObservableCollection<Lokal>();
            datoteka = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lokali.txt");
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
                    lokali = (ObservableCollection<Lokal>)serializer.Deserialize(reader, typeof(ObservableCollection<Lokal>));
                }
            }
            else
            {
                lokali = new ObservableCollection<Lokal>();
            }
        }



        public void memorisi()
        {

            using (StreamWriter writer = File.CreateText(datoteka))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, lokali);
                writer.Close();
            }
        }


        //Metode za dodavanje i brisanje

        public bool dodaj(Lokal Lokal)
        {
            try
            {
                Lokal temp = lokali.Single(tip => tip.Oznaka == Lokal.Oznaka);
                return false;
            }
            catch (InvalidOperationException)
            {
                lokali.Add(Lokal);
                memorisi();
                return true;
            }
        }


        public void obrisi(String oznaka)
        {
            foreach (Lokal l in lokali)
            {
                if (l.Oznaka.Equals(oznaka))
                    lokali.Remove(l);
            }
            memorisi();
        }


        public Lokal this[string oznaka]
        {
            get
            {
                try
                {
                    return lokali.Single(tip => tip.Oznaka == oznaka);
                }
                catch (InvalidOperationException) { return null; }
            }
            set
            {
                try
                {
                    Lokal temp = lokali.Single(tip => tip.Oznaka == oznaka);
                    temp = value;
                }
                catch (InvalidOperationException) { MessageBox.Show("Nije uspeo set!"); }
            }
        }

        public ObservableCollection<Lokal> sviLokali()
        {
            return lokali;
        }

        public void izbaci(Lokal lokal)
        {
            for (int i = 0; i < lokali.Count; i++)
            {
                Lokal l = lokali[i];
                if (l.Oznaka.Equals(lokal.Oznaka))
                {
                    lokali.RemoveAt(i);
                    return;
                }
            }
        }

        public void izbaciSve(Collection<Lokal> lista)
        {
            foreach (Lokal l in lista)
                izbaci(l);
        }

        public bool postojiOznaka(string o, Lokal lo)
        {
            foreach (Lokal l in lokali)
            {
                if (l.Oznaka.Equals(o) && !l.Equals(lo))
                    return true;
            }
            return false;
        }
        


    }


  

}
