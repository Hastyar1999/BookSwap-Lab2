using Lab1.Affarslager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1.Presentation
{
    class Meny
    {
        // Menyn använder affärslagret och har ingen egen databas.
        private readonly Controller controller;
        private StudentInfo inloggadStudent;

        public Meny(Controller controller)
        {
            this.controller = controller;
        }

        public void Menu()
        {

            bool körs = true;

            // while-loopen gör att huvudmenyn fortsätter visas tills användaren väljer 0.
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
                // TryParse kontrollerar att användaren faktiskt har skrivit ett heltal.
                if (int.TryParse(menyval, out val))
                {

                    // Vi visar olika menyval beroende på om studenten är inloggad.
                    if (inloggadStudent == null)
                    {
                        switch (val)
                        {
                            case 0:
                                Console.WriteLine("PROGRAMMET AVSLUTAS...");
                                körs = false;
                                break;
                            case 1:
                                LoggaIn();
                                break;
                            case 2:
                                RegistreraStudent();
                                break;
                            default:
                                Console.WriteLine("Vänligen ange ett giltigt alternativ för att fortsätta!");
                                break;
                        }
                    }
                    else
                    {
                        switch (val)
                        {
                            case 0:
                                Console.WriteLine("PROGRAMMET AVSLUTAS...");
                                körs = false;
                                break;
                            case 1:
                                Console.WriteLine("TILL SALU ANNONSER:");
                                VisaTillSaluAnnonser();
                                break;
                            case 2:
                                ReserveraAnnons();
                                break;
                            case 3:
                                LoggaUt();
                                break;
                            default:
                                Console.WriteLine("Vänligen ange ett giltigt alternativ för att fortsätta!");
                                break;
                        }
                    }
                }

                else
                {
                    Console.WriteLine("Vänligen ange ett giltigt alternativ för att fortsätta! ");
                }

                if (körs)
                {
                    Console.WriteLine("Tryck på valfri tangent för att återgå till huvudmenyn.");
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


        // Skriver ut valen som finns i huvudmenyn.
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


        // Hämtar annonser genom controllern och visar bara de som fortfarande är till salu.
        // Menyn behöver alltså inte själv veta hur filtreringen i databasen fungerar.
        private void VisaTillSaluAnnonser()
        {

            // Affärslagret skickar läsbara annonsuppgifter till menyn.
            List<AnnonsInfo> tillSaluLista = controller.HämtaTillSaluAnnonser();


            // Går igenom listan och skriver ut en annons i taget.
            foreach (AnnonsInfo a in tillSaluLista)
            {
                Console.WriteLine(
                    $"AnnonsID: {a.GetAnnonsId()} | " +
                    $"{a.GetTypAvBok()}, | " +
                    $"{a.GetTitel()}, | " +
                    $"Pris: {a.GetPrisuppgift()} kr, | " +
                    $"Status: {a.GetStatus()}, | " +
                    $"Skick: {a.GetSkick()} | " +
                    $"Säljare: {a.GetSäljare().GetFörnamn()} {a.GetSäljare().GetEfternamn()}"
                );
            }
            
        }
        // Inmatningen sker i presentationslagret. Controller skapar och lagrar objekten.
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

        // Inloggningen söker efter e-post och kontrollerar lösenordet.
        private void LoggaIn()
        {
            Console.Write("E-post: ");
            string epost = Console.ReadLine();
            Console.Write("Lösenord: ");
            string lösenord = Console.ReadLine();

            StudentInfo student = controller.LoggaIn(epost, lösenord);
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

        // Här startar hela flödet för att reservera en annons.
        // Studenten slås upp via controllern. Affärslagret kontrollerar reservationen.
        public void ReserveraAnnons()
        {

            // Studenten som loggat in används automatiskt vid reservationen.
            if (inloggadStudent == null)
            {
                Console.WriteLine("Du måste logga in först.");
                return;
            }
            string StudentID = inloggadStudent.GetAnvändarId();

            VisaTillSaluAnnonser();

            Console.Write("Skriv AnnonsID på den annonsen du vill reservera: ");
            string AnnonsID = Console.ReadLine();

            // Affärslagret kontrollerar reglerna och genomför reservationen.
            ReservationsResultat resultat = controller.ReserveraAnnons(AnnonsID, StudentID);
            Console.WriteLine(resultat.Meddelande);

        }
    }

}
