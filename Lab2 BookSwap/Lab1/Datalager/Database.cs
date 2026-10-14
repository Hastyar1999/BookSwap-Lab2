
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace Lab1.Datalager

{
    public class Database
    {
        
        private readonly List<Student> Studenter = new List<Student>();
        private readonly List<Annons> Annonser = new List<Annons>();
        private readonly List<Affär> Affärer = new List<Affär>();
        private readonly List <Administratör> Administratörer = new List<Affär>();

        // Filen sparas bredvid programmets körbara filer.
        private readonly string studentFil = Path.Combine(AppContext.BaseDirectory, "Konton.txt");


        
        

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
        public Student HämtaStudent(string användarId)
        {
            foreach (Student student in Studenter)
            {
                if (student.GetAnvändarId() == användarId)
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
        public bool FinnsanvändarId(string användarId)
        {
            return HämtaStudent(användarId) != null;
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
                if (FinnsanvändarId(delar[0]) || FinnsEpost(delar[3]))
                {
                    continue;
                }

                Konto konto = new Konto(delar[3], delar[5]);
                Student student = new Student(delar[0], delar[1], delar[2],
                    delar[3], delar[4], konto);
                Studenter.Add(student);
            }
        }

    

    }
}
