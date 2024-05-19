using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Verwaltungssoftware
{
    public static class Menue
    {
        // Speicherdaten für die jeweiligen daten
        private static readonly string NutzerDateiPfad = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "nutzer.txt");

        private static readonly string ArtikelDateiPfad = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "artikel.txt");
        private static readonly string RechnungenDateiPfad = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rechnungen.txt");

        public static void Start()
        {
            Nutzer.NutzerAusDateiLaden(NutzerDateiPfad);
            Artikel.ArtikelAusDateiLaden(ArtikelDateiPfad);
            Rechnung.RechnungenAusDateiLaden(RechnungenDateiPfad);

            bool beenden = false;

            while (!beenden)
            {
                Console.WriteLine();

                Console.WriteLine("Willkommen im Verwaltungssoftware!");
                Console.WriteLine("1. Kunde hinzufügen");
                Console.WriteLine("2. Mitarbeiter hinzufügen");
                Console.WriteLine("3. Artikel hinzufügen");
                Console.WriteLine("4. Rechnung erstellen");
                Console.WriteLine("5. Alle Nutzer anzeigen");
                Console.WriteLine("6. Alle Artikel anzeigen");
                Console.WriteLine("7. Alle Rechnungen anzeigen");
                Console.WriteLine("8. Programm beenden");
                Console.Write("Bitte wählen Sie eine Option: ");
                string auswahl = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine(new string('-', 40));

                switch (auswahl)
                {
                    case "1":
                        KundeHinzufuegen();
                        break;

                    case "2":
                        MitarbeiterHinzufuegen();
                        break;

                    case "3":
                        ArtikelHinzufuegen();
                        break;

                    case "4":
                        RechnungErstellen();
                        break;

                    case "5":
                        AlleNutzerAnzeigen();
                        break;

                    case "6":
                        AlleArtikelAnzeigen();
                        break;

                    case "7":
                        AlleRechnungenAnzeigen();
                        break;

                    case "8":
                        beenden = true;
                        break;

                    default:
                        Console.WriteLine();

                        Console.WriteLine("Ungültige Auswahl, bitte versuchen Sie es erneut.");
                        Console.WriteLine(new string('-', 40)); // Linie
                        break;
                }
            }
        }

        private static void KundeHinzufuegen()
        {
            Console.Write("Vorname: ");
            string vorname = Console.ReadLine();
            Console.Write("Nachname: ");
            string nachname = Console.ReadLine();
            Console.Write("Adresse: ");
            string adresse = Console.ReadLine();
            int kundennummer = Nutzer.NutzerListe.OfType<Kunde>().Count() + 1;

            Kunde kunde = new Kunde
            {
                Vorname = vorname,
                Nachname = nachname,
                Adresse = adresse,
                Kundennummer = kundennummer
            };
            Nutzer.NutzerListe.Add(kunde);
            Nutzer.NutzerInDateiSpeichern(NutzerDateiPfad); // Nutzer speichern
            Console.WriteLine();
            Console.WriteLine("Kunde erfolgreich hinzugefügt.");
            Console.WriteLine();
            Console.WriteLine(new string('-', 40));
        }

        private static void MitarbeiterHinzufuegen()
        {
            Console.Write("Vorname: ");
            string vorname = Console.ReadLine();
            Console.Write("Nachname: ");
            string nachname = Console.ReadLine();
            Console.Write("Adresse: ");
            string adresse = Console.ReadLine();
            Console.Write("Mitarbeiterrabatt (z.B., 0.10 für 10%): ");
            double mitarbeiterrabatt = double.Parse(Console.ReadLine());

            int mitarbeiternummer = Nutzer.NutzerListe.OfType<Mitarbeiter>().Count() + 1;

            Mitarbeiter mitarbeiter = new Mitarbeiter
            {
                Vorname = vorname,
                Nachname = nachname,
                Adresse = adresse,
                Mitarbeiterrabatt = mitarbeiterrabatt,
                Mitarbeitennummer = mitarbeiternummer
            };
            Nutzer.NutzerListe.Add(mitarbeiter);
            Nutzer.NutzerInDateiSpeichern(NutzerDateiPfad); // Nutzer speichern
            Console.WriteLine("Mitarbeiter erfolgreich hinzugefügt.");
        }

        private static void ArtikelHinzufuegen()
        {
            Console.Write("Artikelnummer: ");
            int artikelnummer = int.Parse(Console.ReadLine());
            Console.Write("Bezeichnung: ");
            string bezeichnung = Console.ReadLine();
            Console.Write("Preis: ");
            double preis = double.Parse(Console.ReadLine());

            Artikel artikel = new Artikel
            {
                Artikelnummer = artikelnummer,
                Bezeichnung = bezeichnung,
                Preis = preis
            };
            Artikel.ArtikelListe.Add(artikel);
            Artikel.ArtikelInDateiSpeichern(ArtikelDateiPfad); // Artikel speichern
            Console.WriteLine("Artikel erfolgreich hinzugefügt.");
        }

        private static void RechnungErstellen()
        {
            Console.Write("Rechnungsnummer: ");
            int rechnungsnummer = int.Parse(Console.ReadLine());

            Console.Write("Käufernummer (Kunden-/Mitarbeiternummer): ");
            int käufernummer = int.Parse(Console.ReadLine());

            Nutzer käufer = Nutzer.NutzerListe.FirstOrDefault(n => (n is Kunde kunde && kunde.Kundennummer == käufernummer) || (n is Mitarbeiter mitarbeiter && mitarbeiter.Mitarbeitennummer == käufernummer));
            if (käufer == null)
            {
                Console.WriteLine();
                Console.WriteLine("Käufer nicht gefunden.");
                Console.WriteLine();
                Console.WriteLine(new string('-', 40)); // Linie
                return;
            }

            List<Artikel> artikelListe = new List<Artikel>();
            double gesamtsumme = 0;

            while (true)
            {
                Console.Write("Artikelnummer hinzufügen (0 zum Beenden): ");
                int artikelnummer = int.Parse(Console.ReadLine());
                if (artikelnummer == 0)
                    break;

                Artikel artikel = Artikel.ArtikelListe.FirstOrDefault(a => a.Artikelnummer == artikelnummer);
                if (artikel != null)
                {
                    artikelListe.Add(artikel);
                    gesamtsumme += artikel.Preis;
                    artikel.Verkaufsanzahl++;
                }
                else
                {
                    Console.WriteLine();

                    Console.WriteLine("Artikel nicht gefunden.");
                    Console.WriteLine();
                    Console.WriteLine(new string('-', 40)); // Linie
                }
            }

            if (käufer is Mitarbeiter mitarbeiterKäufer)
            {
                gesamtsumme *= (1 - mitarbeiterKäufer.Mitarbeiterrabatt);
            }

            Rechnung rechnung = new Rechnung
            {
                Rechnungsnummer = rechnungsnummer,
                Käufer = käufer,
                ArtikelListe = artikelListe,
                Gesamtsumme = gesamtsumme
            };
            Rechnung.RechnungsListe.Add(rechnung);
            Rechnung.RechnungenInDateiSpeichern(RechnungenDateiPfad); // Rechnung sofort speichern
            Console.WriteLine();
            Console.WriteLine("Rechnung erfolgreich erstellt.");
            Console.WriteLine();
        }

        private static void AlleNutzerAnzeigen()
        {
            if (Nutzer.NutzerListe.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("Keine Nutzer vorhanden.");
                Console.WriteLine();
                Console.WriteLine(new string('-', 40));
            }
            else
            {
                foreach (Nutzer nutzer in Nutzer.NutzerListe)
                {
                    Console.WriteLine();
                    Console.WriteLine(nutzer);
                    Console.WriteLine();
                    Console.WriteLine(new string('-', 40)); // Linie
                }
            }
        }

        private static void AlleArtikelAnzeigen()
        {
            if (Artikel.ArtikelListe.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("Keine Artikel vorhanden.");
                Console.WriteLine();
                Console.WriteLine(new string('-', 40));
            }
            else
            {
                foreach (Artikel artikel in Artikel.ArtikelListe)
                {
                    Console.WriteLine();
                    Console.WriteLine(artikel);
                    Console.WriteLine();
                    Console.WriteLine(new string('-', 40)); // Linie
                }
            }
        }

        private static void AlleRechnungenAnzeigen()
        {
            if (Rechnung.RechnungsListe.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("Keine Rechnungen vorhanden.");
                Console.WriteLine();
                Console.WriteLine(new string('-', 40));
            }
            else
            {
                foreach (Rechnung rechnung in Rechnung.RechnungsListe)
                {
                    Console.WriteLine();
                    Console.WriteLine(rechnung);
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine(new string('-', 40)); // Linie
                }
            }
        }
    }
}