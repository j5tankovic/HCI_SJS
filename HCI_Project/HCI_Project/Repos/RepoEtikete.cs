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
    public class RepoEtikete
    {
        public ObservableCollection<Etiketa> etikete
        {
            get;
            set;
        }
        private readonly string datoteka;


        public RepoEtikete()
        {
            etikete = new ObservableCollection<Etiketa>();
            datoteka = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "etikete.txt");
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
                    etikete = (ObservableCollection<Etiketa>)serializer.Deserialize(reader, typeof(ObservableCollection<Etiketa>));
                }
            }
            else
            {
                etikete = new ObservableCollection<Etiketa>();
            }
        }



        public void memorisi()
        {

            using (StreamWriter writer = File.CreateText(datoteka))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, etikete);
            }
        }


        //Metode za dodavanje i brisanje

        public bool dodaj(Etiketa Etiketa)
        {
            try
            {
                Etiketa temp = etikete.Single(tip => tip.Oznaka == Etiketa.Oznaka);
                return false;
            }
            catch (InvalidOperationException)
            {
                etikete.Add(Etiketa);
                memorisi();
                return true;
            }
            
        }


        public void obrisi(String oznaka)
        {
            foreach (Etiketa e in etikete)
            {
                if (e.Oznaka.Equals(oznaka))
                    etikete.Remove(e);
            }
            memorisi();
        }


        public Etiketa this[string oznaka]
        {
            get
            {
                try
                {
                    return etikete.Single(tip => tip.Oznaka == oznaka);
                }
                catch (InvalidOperationException) { return null; }
            }
            set
            {
                try
                {
                    Etiketa temp = etikete.Single(tip => tip.Oznaka == oznaka);
                    temp = value;
                }
                catch (InvalidOperationException) { MessageBox.Show("Nije uspeo set!"); }
            }
        }

        public ObservableCollection<Etiketa> sveEtikete()
        {
            return etikete;
        }

        public void izbaci(Etiketa etiketa)
        {
            for (int i = 0; i < etikete.Count; i++)
            {
                Etiketa l = etikete[i];
                if (l.Oznaka.Equals(etiketa.Oznaka))
                {
                    etikete.RemoveAt(i);
                    return;
                }
            }
        }

        public bool postojiOznaka(string o)
        {
            foreach (Etiketa e in etikete)
            {
                if (e.Oznaka.Equals(o))
                    return true;
            }
            return false;
        }
    }
}
