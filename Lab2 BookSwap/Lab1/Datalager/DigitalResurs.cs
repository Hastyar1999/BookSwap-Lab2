
using System;
using System.Collections.Generic;


namespace Lab1.Datalager
{
    // DigitalResurs ärver från Annons men har dessutom filformat och leveranssätt som extras strings.
    // På så sätt kan digitalt material hanteras som en vanlig annons men ändå ha egna uppgifter.
    public class DigitalResurs : Annons
    {
        private string filformat;
        private string leveranssätt;
       

        public DigitalResurs(
            string annonsId,
            string typavbok,
            string titel,
            double prisuppgift,
            DateTime publiceringsdatum,
            string status,
            string skick,
            string filformat,
            string leveranssätt,
            Student säljare)
            // De gemensamma värdena skickas till Annons-konstruktorn med base.
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
            this.filformat = filformat;
            this.leveranssätt = leveranssätt;
       
        }

        // Get-metoderna används för att läsa de uppgifter som bara gäller digitala resurser.
        public string GetFileFormat()
        {
            return filformat;
        }

        public string GetLeveranssätt()
        {
            return leveranssätt;
        }
        
    }
}
