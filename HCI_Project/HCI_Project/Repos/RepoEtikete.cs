using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HCI_Project.Beans;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

namespace HCI_Project.Repos
{
    class RepoEtikete
    {
        private List<Etiketa> etikete = new List<Etiketa>();
        private readonly string datoteka;


        public RepoEtikete()
        {
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
                    etikete = (List<Etiketa>)serializer.Deserialize(reader, typeof(List<Etiketa>));
                }
            }
            else
            {
                etikete = new List<Etiketa>();
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

        public void dodaj(Etiketa Etiketa)
        {
            Etiketa temp = etikete.Find(tip => tip.Oznaka == Etiketa.Oznaka);
            if (temp == null)
                etikete.Add(Etiketa);
            memorisi();
        }


        public void obrisi(String oznaka)
        {
            etikete.RemoveAll(tip => tip.Oznaka==oznaka);
            memorisi();
        }


        public Etiketa this[string oznaka]
        {
            get
            {
                return etikete.Find(tip => tip.Oznaka == oznaka);
            }
            set
            {
                Etiketa temp = etikete.Find(tip => tip.Oznaka == oznaka);
                temp = value;
            }
        }

        public List<Etiketa> sveEtikete()
        {
            return etikete;
        }
    }
}
