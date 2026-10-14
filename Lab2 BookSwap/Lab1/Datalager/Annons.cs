using System;
using System.Collections.Generic;


namespace Lab1.Datalager
{
    // Annons är en abstrakt basklass för alla annonstyper i BookSwap.
    // Gemensamma egenskaper ligger här så vi slipper skriva samma kod i varje underklass.
    public abstract class Annons
    {
        private string annonsId;
        private string typavbok;
        private string titel;
        private double prisuppgift;
        private DateTime publiceringsdatum;
        private string status;
        private string skick;
        private Student säljare;
       
        

        // Konstruktorn tar emot allt som är gemensamt för en annons och sparar det i objektets fält.
        public Annons(string annonsId, string typavbok,
            string titel, double prisuppgift,
            DateTime publiceringsdatum,
            string status, string skick, Student säljare)
          
          
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


        // Get-metoderna används för att läsa privata fält utanför klassen utan att göra fälten publika.
        public string GetAnnonsId()
        {
            return annonsId;
        }
        public string GetTitel()
        {
            return titel;
        }
        public double GetPrisuppgift()
        {
            return prisuppgift;
        }
        public DateTime GetPubliceringsdatum()
        {
            return publiceringsdatum;
        }

        public string GetStatus()
        {
            return status;
        }

        // SetStatus används när annonsens status behöver ändras, till exempel vid en reservation.
        public void SetStatus(string status)
        {
            this.status = status;
        }
        public string GetSkick()
        {
            return skick;
        }
        public Student GetSäljare()
        {
            return säljare;
        }
        public string GetTypAvBok()
        {
            
            return typavbok;
        }
    
    
    }
}

