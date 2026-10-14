
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace Lab1.Datalager

{
    public class Database
    {
        // I den här laborationen använder vi listor som en enkel databas i minnet.
        // Det betyder att informationen finns kvar så länge programmet körs, men inte efter att det stängs.
        private readonly List<Student> Studenter = new List<Student>();
        private static readonly List<Annons> Annonser = new List<Annons>();
        private readonly List<Affär> Affärer = new List<Affär>();
        private readonly List<Administratör> Administratörer = new List<Administratör>();

        // Filen sparas bredvid programmets körbara filer.
        private readonly string studentFil = Path.Combine(AppContext.BaseDirectory, "Konton.txt");


        // Seed skapar testdata: först studenter och sedan annonser som kopplas till säljarna.
        // Det gör att vi slipper mata in all data manuellt varje gång programmet startar.
        public void Seed()
        {
            // Testdata ska bara skapas en gång.
            if (Studenter.Count > 0 || Annonser.Count > 0)
            {
                return;
            }

            Student student1 = new Student(
               användarId: "A001",
               förnamn: "Robert",
               efternamn: "Petre",
               ePost: "robert@hb.se",
               telefonnummer: "0701286786"
            );
            Student student2 = new Student(
               användarId: "A002",
               förnamn: "Hastyar",
               efternamn: "Hastyarsefternamn",
               ePost: "Hastyar@hb.se",
               telefonnummer: "0701238765"
            );
            Student student3 = new Student(
               användarId: "A003",
               förnamn: "Jenny",
               efternamn: "Vu",
               ePost: "Jenny@hb.se",
               telefonnummer: "0701290009"
            );

            // Studenterna läggs in i studentlistan så att de senare kan sökas fram via sitt ID.
            Studenter.Add(student1);
            Studenter.Add(student2);
            Studenter.Add(student3);

            Kursbok kursbok1 = new Kursbok(
                annonsId: "ANN001",
                typavbok: "Kursbok",
                titel: "Databasteknik",
                prisuppgift: 300,
                publiceringsdatum: new DateTime(2026, 9, 20),
                status: "Till salu",
                skick: "Bra",
                isbn: "123456789",
                författare: "Anders Andersson",
                upplaga: "6",
                säljare: student1
            );

            Kompendium kompendium1 = new Kompendium(
                annonsId: "ANN002",
                typavbok: "Kompendium",
                titel: "Programmering C#",
                prisuppgift: 100,
                publiceringsdatum: new DateTime(2026, 9, 21),
                status: "Till salu",
                skick: "Bra",
                sidor: 120,
                utgivningsår: 2025,
                säljare: student2
            );

            DigitalResurs digitalResurs1 = new DigitalResurs(
                annonsId: "ANN003",
                typavbok: "Digital Resurs",
                titel: "Python Material",
                prisuppgift: 50,
                publiceringsdatum: new DateTime(2026, 9, 22),
                status: "Till salu",
                skick: "Digital",
                filformat: "PDF",
                leveranssätt: "E-post",
                säljare: student3
            );

            // De olika objekten kan ligga i samma lista eftersom Kursbok, Kompendium och DigitalResurs ärver från Annons.
            Annonser.Add(kursbok1);
            Annonser.Add(kompendium1);
            Annonser.Add(digitalResurs1);

        }

        // Söker igenom annonserna efter ett specifikt annons-ID.
        // Om ingen match hittas returneras null, vilket betyder att ingen annons hittades.
        public Annons HämtaAnnons(string annonsId)
        {
            foreach (Annons annons in Annonser)
            {
                if (annons.GetAnnonsId() == annonsId)
                {
                    return annons;
                }
            }

            return null;
        }


        // Samma princip som ovan, men här söker vi efter en student med rätt användar-ID.
        public Student HämtaStudent(string studentId)
        {
            foreach (Student student in Studenter)
            {
                if (student.GetAnvändarId() == studentId)
                {
                    return student;
                }
            }

            return null;
        }


        // Hittar studenten som har den e-postadress som angavs vid registreringen.
        public Student HämtaStudentViaEpost(string epost)
        {
            foreach (Student student in Studenter)
            {
                if (string.Equals(student.GetEPost(), epost, StringComparison.OrdinalIgnoreCase))
                {
                    return student;
                }
            }
            return null;
        }

        // E-post måste vara unik även om användaren skriver med olika stora bokstäver.
        public bool FinnsEpost(string epost)
        {
            foreach (Student student in Studenter)
            {
                if (string.Equals(student.GetEPost(), epost, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        // Kontrollerar ID så att samma student inte registreras två gånger.
        public bool FinnsStudentId(string studentId)
        {
            return HämtaStudent(studentId) != null;
        }

        // Skapar en ny lista som bara innehåller annonser med statusen "Till salu".
        // På så sätt visas inte redan reserverade annonser när användaren ska välja.
        public List<Annons> HämtaTillSaluAnnonser()
        {
            List<Annons> tillSalu = new List<Annons>();
            foreach (Annons annons in Annonser)
            {
                if (annons.GetStatus() == "Till salu")
                {
                    tillSalu.Add(annons);
                }
            }
            return tillSalu;
        }


        // Controllern använder metoden för att spara en genomförd reservation.
        public void LäggTillAffär(Affär affär)
        {
            Affärer.Add(affär);
        }

        // De här metoderna ändrar innehållet i våra listor, ungefär som enkla databasoperationer.
        // Den här metoden lägger till en ny annons.
        public void LäggTillAnnons(Annons annons)
        {
            Annonser.Add(annons);
        }

        // Tar bort den annons som skickas in från listan.
        public void TaBortAnnons(Annons annons)
        {
            Annonser.Remove(annons);
        }

        // Sparar den nya studenten både i textfilen och i listan.
        // Varje rad innehåller studentens uppgifter, separerade med semikolon.
        public void LäggTillStudent(Student student)
        {
            Konto konto = student.GetKonto();
            if (konto == null)
            {
                throw new InvalidOperationException("Studenten saknar konto.");
            }

            string rad = student.GetAnvändarId() + ";" +
                         student.GetFörnamn() + ";" +
                         student.GetEfternamn() + ";" +
                         student.GetEPost() + ";" +
                         student.GetTelefonnummer() + ";" +
                         konto.GetLösenord();

            // AppendAllText skapar filen om den inte finns och lägger till en rad.
            File.AppendAllText(studentFil, rad + Environment.NewLine, Encoding.UTF8);
            Studenter.Add(student);
        }

        // Läser de sparade studenterna när programmet startar.
        public void LäsInStudenter()
        {
            if (!File.Exists(studentFil))
            {
                return;
            }

            foreach (string rad in File.ReadAllLines(studentFil, Encoding.UTF8))
            {
                string[] delar = rad.Split(';');
                if (delar.Length != 6)
                {
                    continue;
                }

                // Lägg inte in samma student igen om ID eller e-post redan finns.
                if (FinnsStudentId(delar[0]) || FinnsEpost(delar[3]))
                {
                    continue;
                }

                Konto konto = new Konto(delar[3], delar[5]);
                Student student = new Student(delar[0], delar[1], delar[2],
                    delar[3], delar[4], konto);
                Studenter.Add(student);
            }
        }

        // Visar var textfilen har skapats om vi behöver hitta den.
        public string HämtaStudentFil()
        {
            return studentFil;
        }


        public List<Annons> HämtaAllaAnnonser()

        {
            return Database.Annonser;
        }
    }
}
