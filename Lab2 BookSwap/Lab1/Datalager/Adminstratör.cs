
using Lab1.Datalager;

namespace Datalager
{
    public class Administratör : Användare
    {
        // Konstruktor
        public Administratör(
            string användarId,
            string förnamn,
            string efternamn,
            string ePost,
            string telefonnummer,
            Konto konto = null)

            : base(
                användarId,
                förnamn,
                efternamn,
                ePost,
                telefonnummer,
                konto)
        {
        }
    }
}
