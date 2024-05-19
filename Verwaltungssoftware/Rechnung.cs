using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Verwaltungssoftware
{
    public class Rechnung
    {
        public int Rechnungsnummer { get; set; }
        public Nutzer Käufer { get; set; }
        public List<Artikel> ArtikelListe { get; set; } = new List<Artikel>();
        public double Gesamtsumme { get; set; }

        public static List<Rechnung> RechnungsListe { get; set; } = new List<Rechnung>();

        public override string ToString()
        {
            string artikelDetails = string.Join("\n", ArtikelListe.Select(a => a.ToString()));
            return $"Rechnungsnummer: {Rechnungsnummer}\nKäufer: {Käufer}\nArtikel:\n{artikelDetails}\nGesamtsumme: {Gesamtsumme}";
        }

        public static void RechnungenAusDateiLaden(string dateiPfad)
        {
            if (File.Exists(dateiPfad))
            {
                string[] zeilen = File.ReadAllLines(dateiPfad);
                foreach (string zeile in zeilen)
                {
                    string[] daten = zeile.Split(',');
                    if (daten.Length >= 3)
                    {
                        Nutzer käufer = Nutzer.NutzerListe.FirstOrDefault(n => (n is Kunde kunde && kunde.Kundennummer == int.Parse(daten[1])) || (n is Mitarbeiter mitarbeiter && mitarbeiter.Mitarbeitennummer == int.Parse(daten[1])));
                        List<Artikel> artikelListe = daten[2].Split(';').Select(artikelNummer => Artikel.ArtikelListe.FirstOrDefault(a => a.Artikelnummer == int.Parse(artikelNummer))).ToList();
                        double gesamtsumme = double.Parse(daten[3]);
                        Rechnung rechnung = new Rechnung
                        {
                            Rechnungsnummer = int.Parse(daten[0]),
                            Käufer = käufer,
                            ArtikelListe = artikelListe,
                            Gesamtsumme = gesamtsumme
                        };
                        RechnungsListe.Add(rechnung);
                    }
                }
            }
        }

        public static void RechnungenInDateiSpeichern(string dateiPfad)
        {
            using (StreamWriter writer = new StreamWriter(dateiPfad, false))
            {
                foreach (Rechnung rechnung in RechnungsListe)
                {
                    string artikelNummern = string.Join(";", rechnung.ArtikelListe.Select(a => a.Artikelnummer.ToString()));
                    writer.WriteLine($"{rechnung.Rechnungsnummer},{(rechnung.Käufer is Kunde kunde ? kunde.Kundennummer : (rechnung.Käufer as Mitarbeiter).Mitarbeitennummer)},{artikelNummern},{rechnung.Gesamtsumme}");
                }
            }
        }
    }
}