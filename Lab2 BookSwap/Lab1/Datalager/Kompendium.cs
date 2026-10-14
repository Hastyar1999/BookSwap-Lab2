using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace Lab1.Datalager
{
    // Kompendium ärver från Annons och återanvänder alla gemensamma annonsfält.
    // Här lägger vi bara till det som är specifikt för ett kompendium.
    public class Kompendium : Annons 
    {
        private int sidor;
        private int utgivningsår;

        public Kompendium(
            string annonsId,
            string typavbok,
            string titel,
            double prisuppgift,
            DateTime publiceringsdatum,
            string status,
            string skick,
            int sidor,
            int utgivningsår,
            Student säljare)
            // Gemensamma annonsvärden skickas vidare till konstruktorn i Annons.
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
            this.sidor = sidor;
            this.utgivningsår = utgivningsår;
        }

        // De här metoderna låter andra delar av programmet läsa de specifika kompendiumuppgifterna.
        public int GetSidor()
        {
            return this.sidor;
        }
        public int GetUtgivningsår()
        {
            return this.utgivningsår;
        }


    }
}
