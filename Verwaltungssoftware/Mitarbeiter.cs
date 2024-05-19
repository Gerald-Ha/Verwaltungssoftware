namespace Verwaltungssoftware
{
    public class Mitarbeiter : Nutzer
    {
        public int Mitarbeitennummer { get; set; }
        public double Mitarbeiterrabatt { get; set; } = 0.1;

        public override string ToString()
        {
            return base.ToString() + $", Mitarbeitennummer: {Mitarbeitennummer}, Mitarbeiterrabatt: {Mitarbeiterrabatt * 100}%";
        }
    }
}