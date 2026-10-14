using Lab1.Datalager;

namespace Lab1.Affarslager
{
    // Affärslagret kontrollerar reglerna och anropar datalagret.
    public class Controller
    {
        private readonly Database databas;

        public Controller()
        {
            // Samma databas används under hela denna controllers livstid.
            databas = new Database();
            databas.Seed();
            databas.LäsInStudenter();
        }

        public List<AnnonsInfo> HämtaTillSaluAnnonser()
        {
            List<AnnonsInfo> annonser = new List<AnnonsInfo>();
            foreach (Annons annons in databas.HämtaTillSaluAnnonser())
            {
                annonser.Add(SkapaAnnonsInfo(annons));
            }
            return annonser;
        }

        public StudentInfo HämtaStudent(string studentId)
        {
            Student student = databas.HämtaStudent(studentId);
            if (student == null)
            {
                return null;
            }
            return new StudentInfo(student.GetAnvändarId(), student.GetFörnamn(),
                student.GetEfternamn(), student.GetEPost(), student.GetTelefonnummer());
        }

        // Skapar både ett konto och en student, och sparar studenten i minnesdatabasen.
        // Returnerar ett felmeddelande om uppgifterna inte är giltiga.
        public string RegistreraStudent(string studentId, string förnamn, string efternamn,
            string epost, string telefonnummer, string lösenord)
        {
            studentId = studentId?.Trim() ?? "";
            förnamn = förnamn?.Trim() ?? "";
            efternamn = efternamn?.Trim() ?? "";
            epost = epost?.Trim() ?? "";
            telefonnummer = telefonnummer?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(studentId) || string.IsNullOrWhiteSpace(förnamn) ||
                string.IsNullOrWhiteSpace(efternamn) || string.IsNullOrWhiteSpace(epost) ||
                string.IsNullOrWhiteSpace(telefonnummer) || string.IsNullOrWhiteSpace(lösenord))
                return "Alla fält måste fyllas i.";

            if (!epost.Contains('@') || epost.StartsWith('@') || epost.EndsWith('@'))
                return "Ange en giltig e-postadress.";

            // Semikolon och radbrytningar används av textfilens format.
            string[] uppgifter = { studentId, förnamn, efternamn, epost, telefonnummer, lösenord };
            foreach (string uppgift in uppgifter)
            {
                if (uppgift.Contains(';') || uppgift.Contains('\n') || uppgift.Contains('\r'))
                    return "Fälten får inte innehålla semikolon eller radbrytningar.";
            }

            if (databas.FinnsStudentId(studentId))
                return "Student-ID finns redan. Välj ett annat.";

            if (databas.FinnsEpost(epost))
                return "E-postadressen används redan.";

            Konto konto = new Konto(epost, lösenord);
            Student student = new Student(studentId, förnamn, efternamn, epost, telefonnummer, konto);
            try
            {
                databas.LäggTillStudent(student);
            }
            catch (System.IO.IOException)
            {
                return "Kunde inte spara studenten till textfilen.";
            }
            catch (UnauthorizedAccessException)
            {
                return "Saknar behörighet att skriva till textfilen.";
            }

            return "Studenten har registrerats och sparats i databasen och Konton.txt!";
        }

        // Söker upp studenten och kontrollerar lösenordet via Konto.
        // Returnerar null om inloggningsuppgifterna är felaktiga.
        public StudentInfo LoggaIn(string epost, string lösenord)
        {
            epost = epost?.Trim() ?? "";
            Student student = databas.HämtaStudentViaEpost(epost);
            if (student == null || student.GetKonto() == null)
            {
                return null;
            }

            if (!student.GetKonto().KontrolleraLösenord(lösenord))
            {
                return null;
            }

            return new StudentInfo(student.GetAnvändarId(), student.GetFörnamn(),
                student.GetEfternamn(), student.GetEPost(), student.GetTelefonnummer());
        }

        public ReservationsResultat ReserveraAnnons(string annonsId, string studentId)
        {
            Student köpare = databas.HämtaStudent(studentId);
            if (köpare == null)
            {
                return new ReservationsResultat(false, "Ogiltigt StudentID.", null);
            }

            Annons annons = databas.HämtaAnnons(annonsId);
            if (annons == null)
            {
                return new ReservationsResultat(false, "Ogiltigt AnnonsID.", null);
            }
            if (annons.GetStatus() != "Till salu")
            {
                return new ReservationsResultat(false, "Annonsen är redan reserverad eller inte till salu.", null);
            }
            // Alla regler kontrolleras innan status ändras eller en affär sparas.
            if (annons.GetSäljare().GetAnvändarId() == köpare.GetAnvändarId())
            {
                return new ReservationsResultat(false, "Du kan inte reservera din egen annons.", null);
            }

            string nyAffärId = "AFF" + Guid.NewGuid().ToString("N").ToUpper();
            Affär affär = new Affär(nyAffärId, DateTime.Now, "Reserverad",
                annons.GetSäljare(), köpare, annons);
            annons.SetStatus("Reserverad");
            databas.LäggTillAffär(affär);
            return new ReservationsResultat(true, "Annonsen är reserverad.", SkapaAnnonsInfo(annons));
        }

        // Menyn får läsbara kopior och behöver inte referera datalagrets klasser.
        private static AnnonsInfo SkapaAnnonsInfo(Annons annons)
        {
            return new AnnonsInfo(annons.GetAnnonsId(), annons.GetTypAvBok(),
                annons.GetTitel(), annons.GetPrisuppgift(), annons.GetPubliceringsdatum(),
                annons.GetStatus(), annons.GetSkick(),
                new StudentInfo(annons.GetSäljare().GetAnvändarId(),
                    annons.GetSäljare().GetFörnamn(), annons.GetSäljare().GetEfternamn(),
                    annons.GetSäljare().GetEPost(), annons.GetSäljare().GetTelefonnummer()));
        }
    }
}
