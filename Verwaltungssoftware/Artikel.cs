using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Verwaltungssoftware
{
    public class Artikel
    {
        public int Artikelnummer { get; set; }
        public string Bezeichnung { get; set; }
        public double Preis { get; set; }
        public int Verkaufsanzahl { get; set; } = 0;

        public static List<Artikel> ArtikelListe { get; set; } = new List<Artikel>();

        public override string ToString()
        {
            return $"Artikelnummer: {Artikelnummer}, Bezeichnung: {Bezeichnung}, Preis: {Preis}, Verkaufsanzahl: {Verkaufsanzahl}";
        }

        public static void ArtikelAusDateiLaden(string dateiPfad)
        {
            if (File.Exists(dateiPfad))
            {
                string[] zeilen = File.ReadAllLines(dateiPfad);
                foreach (string zeile in zeilen)
                {
                    string[] daten = zeile.Split(',');
                    if (daten.Length == 4)
                    {
                        Artikel artikel = new Artikel
                        {
                            Artikelnummer = int.Parse(daten[0]),
                            Bezeichnung = daten[1],
                            Preis = double.Parse(daten[2]),
                            Verkaufsanzahl = int.Parse(daten[3])
                        };
                        ArtikelListe.Add(artikel);
                    }
                }
            }
        }

        public static void ArtikelInDateiSpeichern(string dateiPfad)
        {
            using (StreamWriter writer = new StreamWriter(dateiPfad, false))
            {
                foreach (Artikel artikel in ArtikelListe)
                {
                    writer.WriteLine($"{artikel.Artikelnummer},{artikel.Bezeichnung},{artikel.Preis},{artikel.Verkaufsanzahl}");
                }
            }
        }
    }
}