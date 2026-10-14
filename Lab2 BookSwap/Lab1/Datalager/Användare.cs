using System;
using System.Collections.Generic;


namespace Lab1.Datalager
{
    // Användare samlar de grunduppgifter som en användare i systemet behöver.
    // Klassen fungerar som förälder till Student så att gemensam kod kan återanvändas.
    public class Användare
    {
        private string användarId;
        private string förnamn;
        private string efternamn;
        private string ePost;
        private string telefonnummer;
        private Konto konto;

        // Konstruktorn körs när ett nytt användarobjekt skapas och sparar de värden som skickas in.
        public Användare(
            string användarId,
            string förnamn,
            string efternamn,
            string ePost,
            string telefonnummer,
            Konto konto = null)
        {
            this.användarId = användarId;
            this.förnamn = förnamn;
            this.efternamn = efternamn;
            this.ePost = ePost;
            this.telefonnummer = telefonnummer;
            this.konto = konto;
        }

        // Get-metoderna gör att andra klasser kan läsa de privata fälten på ett kontrollerat sätt.
        public string GetAnvändarId()
        {
            return användarId;
        }

        public string GetFörnamn()
        {
            return förnamn;
        }

        public string GetEfternamn()
        {
            return efternamn;
        }

        public string GetEPost()
        {
            return ePost;
        }

        public string GetTelefonnummer()
        {
            return telefonnummer;
        }

        // Student ärver den här metoden från Användare.
        public Konto GetKonto()
        {
            return konto;
        }
    }
}
