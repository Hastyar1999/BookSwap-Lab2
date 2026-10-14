using System;
using System.Collections.Generic;

namespace Lab1.Datalager
{
    // Affär representerar resultatet av en reservation.
    // Den kopplar ihop vilken annons som reserverats, vem som säljer, vem som köper och när det hände.
    public class Affär
    {
        private string affärId;
        private DateTime datum;
        private string status;
        private Student säljare;
        private Student köpare;
        private Annons annons;

        // Konstruktorn används när en ny affär skapas efter att en annons reserverats.
        public Affär(
            string affärId,
            DateTime datum,
            string status,
            Student säljare,
            Student köpare,
            Annons annons)
        {
            this.affärId = affärId;
            this.datum = datum;
            this.status = status;
            this.säljare = säljare;
            this. köpare = köpare;
            this.annons = annons;
        }
    }
}
