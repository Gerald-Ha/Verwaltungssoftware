using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Verwaltungssoftware
{
    public class Nutzer
    {
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public string Adresse { get; set; }
        public static List<Nutzer> NutzerListe { get; set; } = new List<Nutzer>();

        public override string ToString()
        {
            return $"Vorname: {Vorname}, Nachname: {Nachname}, Adresse: {Adresse}";
        }

        public static void NutzerAusDateiLaden(string dateiPfad)
        {
            if (File.Exists(dateiPfad))
            {
                string[] zeilen = File.ReadAllLines(dateiPfad);
                foreach (string zeile in zeilen)
                {
                    string[] daten = zeile.Split(',');
                    if (daten.Length == 5)
                    {
                        if (daten[4] == "Kunde")
                        {
                            Kunde kunde = new Kunde
                            {
                                Vorname = daten[0],
                                Nachname = daten[1],
                                Adresse = daten[2],
                                Kundennummer = int.Parse(daten[3])
                            };
                            NutzerListe.Add(kunde);
                        }
                        else if (daten[4] == "Mitarbeiter")
                        {
                            Mitarbeiter mitarbeiter = new Mitarbeiter
                            {
                                Vorname = daten[0],
                                Nachname = daten[1],
                                Adresse = daten[2],
                                Mitarbeiterrabatt = double.Parse(daten[3]),
                                Mitarbeitennummer = NutzerListe.Count + 1
                            };
                            NutzerListe.Add(mitarbeiter);
                        }
                    }
                }
            }
        }

        public static void NutzerInDateiSpeichern(string dateiPfad)
        {
            using (StreamWriter writer = new StreamWriter(dateiPfad, false))
            {
                foreach (Nutzer nutzer in NutzerListe)
                {
                    if (nutzer is Kunde kunde)
                    {
                        writer.WriteLine($"{kunde.Vorname},{kunde.Nachname},{kunde.Adresse},{kunde.Kundennummer},Kunde");
                    }
                    else if (nutzer is Mitarbeiter mitarbeiter)
                    {
                        writer.WriteLine($"{mitarbeiter.Vorname},{mitarbeiter.Nachname},{mitarbeiter.Adresse},{mitarbeiter.Mitarbeiterrabatt},{mitarbeiter.Mitarbeitennummer},Mitarbeiter");
                    }
                }
            }
        }
    }
}