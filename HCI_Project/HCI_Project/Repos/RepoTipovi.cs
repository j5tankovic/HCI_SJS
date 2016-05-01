﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using HCI_Project.Beans;
using Newtonsoft.Json;

namespace HCI_Project.Repos
{
    class RepoTipovi
    {
        private List<TipLokala> tipovi = new List<TipLokala>();
        private readonly string datoteka;


        public RepoTipovi()
        {
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
                    tipovi = (List<TipLokala>)serializer.Deserialize(reader, typeof(List<TipLokala>));
                }
            }
            else
            {
                tipovi = new List<TipLokala>();
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

        public void dodaj(TipLokala tipLokala)
        {
            TipLokala temp = tipovi.Find(tip => tip.Oznaka == tipLokala.Oznaka);
            if (temp == null)
                tipovi.Add(tipLokala);
            memorisi();
        }


        public void obrisi(String oznaka)
        {
            tipovi.RemoveAll(tip => tip.Oznaka==oznaka);
            memorisi();
        }


        public TipLokala this[string oznaka]
        {
            get
            {
                return tipovi.Find(tip => tip.Oznaka == oznaka);
            }
            set
            {
                TipLokala temp = tipovi.Find(tip => tip.Oznaka == oznaka);
                temp = value;
            }
        }

        public List<TipLokala> sviTipovi()
        {
            return tipovi;
        }
    }
}