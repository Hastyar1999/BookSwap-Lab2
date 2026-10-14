
using System;
using System.Collections.Generic;

namespace Lab1.Datalager
{
    // Student ärver från Användare. På så sätt får Student automatiskt tillgång till
    // de gemensamma uppgifterna och sitt Konto via Användare.
    public class Student : Användare
    {
        public Student(
            string användarId,
            string förnamn,
            string efternamn,
            string ePost,
            string telefonnummer,
            Konto konto = null)
            // base skickar vidare värdena till konstruktorn i Användare.
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
