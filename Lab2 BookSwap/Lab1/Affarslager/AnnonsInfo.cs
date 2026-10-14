namespace Lab1.Affarslager
{
    // En läsbar kopia av annonsuppgifterna som presentationen behöver.
    public class AnnonsInfo
    {
        private readonly string annonsId, typavbok, titel, status, skick;
        private readonly double prisuppgift;
        private readonly DateTime publiceringsdatum;
        private readonly StudentInfo säljare;

        public AnnonsInfo(string annonsId, string typavbok, string titel,
            double prisuppgift, DateTime publiceringsdatum, string status,
            string skick, StudentInfo säljare)
        {
            this.annonsId = annonsId;
            this.typavbok = typavbok;
            this.titel = titel;
            this.prisuppgift = prisuppgift;
            this.publiceringsdatum = publiceringsdatum;
            this.status = status;
            this.skick = skick;
            this.säljare = säljare;
        }

        public string GetAnnonsId() { return annonsId; }
        public string GetTypAvBok() { return typavbok; }
        public string GetTitel() { return titel; }
        public double GetPrisuppgift() { return prisuppgift; }
        public DateTime GetPubliceringsdatum() { return publiceringsdatum; }
        public string GetStatus() { return status; }
        public string GetSkick() { return skick; }
        public StudentInfo GetSäljare() { return säljare; }
    }
}
