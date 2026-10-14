using System;
using System.Collections.Generic;


namespace Lab1.Datalager
{
    // Kursbok är en specialisering av Annons. Den ärver allt gemensamt från Annons
    // och lägger sedan till information som bara behövs för kursböcker.
    public class Kursbok : Annons
    {
        private string isbn;
        private string författare;
        private string upplaga;

        public Kursbok(
            string annonsId,
            string typavbok,
            string titel,
            double prisuppgift,
            DateTime publiceringsdatum,
            string status,
            string skick,
            string isbn,
            string författare,
            string upplaga,
            Student säljare)
            // De gemensamma annonsuppgifterna skickas till basklassen Annons med base.
            : base(
                  annonsId,
                  typavbok,
                  titel,
                  prisuppgift,
                  publiceringsdatum,
                  status,
                  skick,
                  säljare
                 )
        {
            this.isbn = isbn;
            this.författare = författare;
            this.upplaga = upplaga;
        }

        // Get-metoderna nedan används för att läsa Kursbokens egna extra uppgifter.
        public string GetIsbn()
        {
            return isbn;
        }

        public string GetFörfattare()
        {
            return författare;
        }

        public string GetUpplaga()
        {
            return upplaga;
        }
        public string GetTypAvbok()
        {
            return GetTypAvBok();
        }
    }
}
