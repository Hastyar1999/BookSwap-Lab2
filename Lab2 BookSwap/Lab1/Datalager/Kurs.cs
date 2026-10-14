
using System;

namespace Datalager
{

           public class Kurs
        {
            private string kurskod;
            private string kursnamn;

            // Konstruktor
            public Kurs(string kurskod, string kursnamn)
            {
                if (string.IsNullOrWhiteSpace(kurskod) ||
                    string.IsNullOrWhiteSpace(kursnamn))
                {
                    throw new ArgumentException(
                        "Kurskod och kursnamn måste anges.");
                }

                this.kurskod = kurskod;
                this.kursnamn = kursnamn;
            }

            // Hämtar kurskoden
            public string GetKurskod()
            {
                return kurskod;
            }

            // Hämtar kursnamnet
            public string GetKursnamn()
            {
                return kursnamn;
            }

            // Administratören kan ändra kursnamnet
            public void ÄndraNamn(string nyttNamn)
            {
                if (string.IsNullOrWhiteSpace(nyttNamn))
                {
                    throw new ArgumentException(
                        "Kursnamnet får inte vara tomt.");
                }

                kursnamn = nyttNamn;
            }

            public override string ToString()
            {
                return kurskod + " - " + kursnamn;
            }
        }
    }
