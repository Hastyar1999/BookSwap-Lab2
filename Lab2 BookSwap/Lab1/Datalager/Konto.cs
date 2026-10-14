namespace Lab1.Datalager
{
    // Inloggningsuppgifter för en användare i laborationen.
    public class Konto
    {
        private readonly string epost;
        private readonly string lösenord;

        public Konto(string epost, string lösenord)
        {
            this.epost = epost;
            this.lösenord = lösenord;
        }

        public string GetEpost() { return epost; }

        // Används bara när studentens uppgifter sparas till textfilen.
        public string GetLösenord() { return lösenord; }
        public bool KontrolleraLösenord(string angivetLösenord)
        {
            return lösenord == angivetLösenord;
        }
    }
}
