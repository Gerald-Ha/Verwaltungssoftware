namespace Verwaltungssoftware
{
    public class Kunde : Nutzer
    {
        public int Kundennummer { get; set; }
        public double Gesamtsumme { get; set; } = 0;

        public override string ToString()
        {
            return base.ToString() + $", Kundennummer: {Kundennummer}, Gesamtsumme: {Gesamtsumme}";
        }
    }
}