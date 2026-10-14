using Lab1.Datalager;

namespace Lab1.Affarslager
{
    // Controller tar emot anrop från menyn och använder databasen.
    public class Controller
    {
        private readonly Database databas;

        public Controller()
        {
            databas = new Database();
            databas.LäsInStudenter();
        }

        // Hämtar annonser som fortfarande är till salu.
        public List<Annons> HämtaTillSaluAnnonser()
        {
            return databas.HämtaTillSaluAnnonser();
        }

        public Student HämtaStudent(string studentId)
        {
            return databas.HämtaStudent(studentId);
        }

        // Skapar ett konto och en student. Konto sparas i Användare genom arv.
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
            {
                return "Alla fält måste fyllas i.";
            }

            if (!epost.Contains('@') || epost.StartsWith('@') || epost.EndsWith('@'))
            {
                return "Ange en giltig e-postadress.";
            }

            // Vi skiljer uppgifterna med semikolon i Konton.txt.
            string[] uppgifter = { studentId, förnamn, efternamn, epost, telefonnummer, lösenord };
            foreach (string uppgift in uppgifter)
            {
                if (uppgift.Contains(';') || uppgift.Contains('\n') || uppgift.Contains('\r'))
                {
                    return "Fälten får inte innehålla semikolon eller radbrytningar.";
                }
            }

            if (databas.FinnsStudentId(studentId))
            {
                return "Student-ID finns redan.";
            }

            if (databas.FinnsEpost(epost))
            {
                return "E-postadressen används redan.";
            }

            Konto konto = new Konto(epost, lösenord);
            Student student = new Student(studentId, förnamn, efternamn,
                epost, telefonnummer, konto);
            try
            {
                databas.LäggTillStudent(student);
            }
            catch (System.IO.IOException)
            {
                return "Kunde inte spara studenten till Konton.txt.";
            }
            catch (UnauthorizedAccessException)
            {
                return "Saknar behörighet att skriva till Konton.txt.";
            }

            return "Studenten har registrerats och sparats i Konton.txt!";
        }

        // Söker efter studenten och använder Konto för att kontrollera lösenordet.
        public Student LoggaIn(string epost, string lösenord)
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

            return student;
        }

        // Returnerar den reserverade annonsen, eller null om det inte går.
        public Annons ReserveraAnnons(string annonsId, string studentId)
        {
            Student köpare = databas.HämtaStudent(studentId);
            Annons annons = databas.HämtaAnnons(annonsId);

            if (köpare == null || annons == null)
            {
                return null;
            }

            if (annons.GetStatus() != "Till salu")
            {
                return null;
            }

            // En student får inte reservera sin egen annons.
            if (annons.GetSäljare().GetAnvändarId() == köpare.GetAnvändarId())
            {
                return null;
            }

            string affärId = "AFF" + Guid.NewGuid().ToString("N").ToUpper();
            Affär affär = new Affär(affärId, DateTime.Now, "Reserverad",
                annons.GetSäljare(), köpare, annons);

            annons.SetStatus("Reserverad");
            databas.LäggTillAffär(affär);
            return annons;
        }
    }
}
