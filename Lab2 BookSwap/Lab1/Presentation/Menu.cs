using Lab1.Affarslager;
using Lab1.Datalager;

namespace Lab1.Presentation
{
    class Meny
    {
        private readonly Controller controller;
        private Student inloggadStudent;

        public Meny(Controller controller)
        {
            this.controller = controller;
        }

        public void Menu()
        {
            bool körs = true;

            while (körs)
            {
                MenyVarianter();
                Console.Write("Ange ett alternativ för att fortsätta: ");
                string menyval = Console.ReadLine();
                if (menyval == null)
                {
                    return;
                }

                int val;
                if (!int.TryParse(menyval, out val))
                {
                    Console.WriteLine("Ange ett giltigt nummer.");
                }
                else if (inloggadStudent == null)
                {
                    // Menyn före inloggning.
                    switch (val)
                    {
                        case 0:
                            körs = false;
                            break;
                        case 1:
                            LoggaIn();
                            break;
                        case 2:
                            RegistreraStudent();
                            break;
                        default:
                            Console.WriteLine("Välj ett giltigt alternativ.");
                            break;
                    }
                }
                else
                {
                    // Menyn efter inloggning.
                    switch (val)
                    {
                        case 0:
                            körs = false;
                            break;
                        case 1:
                            VisaTillSaluAnnonser();
                            break;
                        case 2:
                            ReserveraAnnons();
                            break;
                        case 3:
                            LoggaUt();
                            break;
                        default:
                            Console.WriteLine("Välj ett giltigt alternativ.");
                            break;
                    }
                }

                if (körs)
                {
                    Console.WriteLine("Tryck på en tangent för att gå tillbaka.");
                    if (Console.IsInputRedirected)
                    {
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.ReadKey(true);
                    }
                }
            }
        }

        private void MenyVarianter()
        {
            if (!Console.IsOutputRedirected)
            {
                Console.Clear();
            }

            Console.WriteLine("VÄLKOMMEN TILL BOOKSWAP");
            Console.WriteLine();

            if (inloggadStudent == null)
            {
                Console.WriteLine("1. Logga in");
                Console.WriteLine("2. Registrera student och skapa konto");
            }
            else
            {
                Console.WriteLine("Inloggad: " + inloggadStudent.GetFörnamn());
                Console.WriteLine("1. Visa till salu annonser");
                Console.WriteLine("2. Reservera annons");
                Console.WriteLine("3. Logga ut");
            }

            Console.WriteLine("0. Avsluta");
            Console.WriteLine();
        }

        // Visar riktiga Annons-objekt som Controller har hämtat.
        private void VisaTillSaluAnnonser()
        {
            List<Annons> annonser = controller.HämtaTillSaluAnnonser();
            Console.WriteLine("TILL SALU ANNONSER:");

            if (annonser.Count == 0)
            {
                Console.WriteLine("Det finns inga annonser till salu.");
            }

            foreach (Annons annons in annonser)
            {
                Console.WriteLine("AnnonsID: " + annons.GetAnnonsId() + " | " +
                    annons.GetTypAvBok() + " | " + annons.GetTitel() + " | " +
                    "Pris: " + annons.GetPrisuppgift() + " kr | " +
                    "Status: " + annons.GetStatus() + " | " +
                    "Skick: " + annons.GetSkick() + " | " +
                    "Säljare: " + annons.GetSäljare().GetFörnamn() + " " +
                    annons.GetSäljare().GetEfternamn());
            }
        }

        private void RegistreraStudent()
        {
            Console.WriteLine("REGISTRERA NY STUDENT");
            Console.Write("Student-ID: ");
            string studentId = Console.ReadLine();
            Console.Write("Förnamn: ");
            string förnamn = Console.ReadLine();
            Console.Write("Efternamn: ");
            string efternamn = Console.ReadLine();
            Console.Write("E-post: ");
            string epost = Console.ReadLine();
            Console.Write("Telefonnummer: ");
            string telefonnummer = Console.ReadLine();
            Console.Write("Lösenord: ");
            string lösenord = Console.ReadLine();

            string resultat = controller.RegistreraStudent(studentId, förnamn, efternamn,
                epost, telefonnummer, lösenord);
            Console.WriteLine(resultat);
        }

        private void LoggaIn()
        {
            Console.Write("E-post: ");
            string epost = Console.ReadLine();
            Console.Write("Lösenord: ");
            string lösenord = Console.ReadLine();

            Student student = controller.LoggaIn(epost, lösenord);
            if (student == null)
            {
                Console.WriteLine("Fel e-postadress eller lösenord.");
                return;
            }

            inloggadStudent = student;
            Console.WriteLine("Välkommen " + student.GetFörnamn() + "! Du är nu inloggad.");
        }

        private void LoggaUt()
        {
            inloggadStudent = null;
            Console.WriteLine("Du är nu utloggad.");
        }

        public void ReserveraAnnons()
        {
            if (inloggadStudent == null)
            {
                Console.WriteLine("Du måste logga in först.");
                return;
            }

            VisaTillSaluAnnonser();
            Console.Write("Skriv AnnonsID på den annons du vill reservera: ");
            string annonsId = Console.ReadLine();

            Annons annons = controller.ReserveraAnnons(annonsId,
                inloggadStudent.GetAnvändarId());
            if (annons == null)
            {
                Console.WriteLine("Det gick inte att reservera annonsen. " +
                    "Kontrollera ID, status och att annonsen inte är din egen.");
            }
            else
            {
                Console.WriteLine("Annonsen " + annons.GetAnnonsId() + " är reserverad.");
            }
        }
    }
}
