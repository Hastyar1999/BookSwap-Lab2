namespace Lab1.Affarslager
{
    // Menyn kan visa resultatet utan att själv kontrollera affärsreglerna.
    public class ReservationsResultat
    {
        public bool Lyckades { get; }
        public string Meddelande { get; }
        public AnnonsInfo Annons { get; }

        public ReservationsResultat(bool lyckades, string meddelande, AnnonsInfo annons)
        {
            Lyckades = lyckades;
            Meddelande = meddelande;
            Annons = annons;
        }
    }
}
