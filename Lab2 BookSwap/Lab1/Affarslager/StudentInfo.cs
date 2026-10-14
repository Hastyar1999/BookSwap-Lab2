namespace Lab1.Affarslager
{
    // Presentationen läser studentuppgifter genom affärslagret.
    public class StudentInfo
    {
        private readonly string användarId, förnamn, efternamn, ePost, telefonnummer;

        public StudentInfo(string användarId, string förnamn, string efternamn,
            string ePost, string telefonnummer)
        {
            this.användarId = användarId;
            this.förnamn = förnamn;
            this.efternamn = efternamn;
            this.ePost = ePost;
            this.telefonnummer = telefonnummer;
        }

        public string GetAnvändarId() { return användarId; }
        public string GetFörnamn() { return förnamn; }
        public string GetEfternamn() { return efternamn; }
        public string GetEPost() { return ePost; }
        public string GetTelefonnummer() { return telefonnummer; }
    }
}
