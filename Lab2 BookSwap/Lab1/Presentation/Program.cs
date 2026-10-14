using Lab1.Affarslager;

namespace Lab1.Presentation
{
    class Program
    {
        public static void Main(string[] args)
        {
            // Controllern skapar databasen. Presentationen använder bara affärslagret.
            Controller controller = new Controller();
            Meny meny = new Meny(controller);
            meny.Menu();
        }
    }
}
