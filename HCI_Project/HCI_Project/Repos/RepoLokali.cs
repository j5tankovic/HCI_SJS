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
    public class RepoLokali
    {
        public List<Lokal> lokali { get; set; }

        private readonly string datoteka;


        public RepoLokali()
        {
            lokali = new List<Lokal>();
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
                    lokali = (List<Lokal>)serializer.Deserialize(reader, typeof(List<Lokal>));
                }
            }
            else
            {
                lokali = new List<Lokal>();
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

        public void dodaj(Lokal Lokal)
        {
            Lokal temp = lokali.Find(tip => tip.Oznaka == Lokal.Oznaka);
            if (temp == null)
                lokali.Add(Lokal);
            //memorisi();
        }


        public void obrisi(String oznaka)
        {
            lokali.RemoveAll(tip => tip.Oznaka==oznaka);
            memorisi();
        }


        public Lokal this[string oznaka]
        {
            get
            {
                return lokali.Find(tip => tip.Oznaka == oznaka);
            }
            set
            {
                Lokal temp = lokali.Find(tip => tip.Oznaka == oznaka);
                temp = value;
            }
        }

        public List<Lokal> sviLokali()
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


    }


  

}
