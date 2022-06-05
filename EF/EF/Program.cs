namespace EF;
using static Console;

//Adres a1 = new Adres()
//{
//    Id = 1,
//    Miasto = "Toruń",
//    Ulica = "Grudziądzka",
//    NumerDomu = 5,
//    NumerMieszkania = 477
//};

//Adres a2 = new Adres()
//{
//    Id = 1,
//    Miasto = "Toruń",
//    Ulica = "Grudziądzka",
//    NumerDomu = 5,
//    NumerMieszkania = 477
//};

//Console.WriteLine(a1.Equals(a2));

public class Program
{
    public static void Main()
    {
        string dbName = "osoby.db";
        if(File.Exists(dbName))
            File.Delete(dbName);

        BazaDanychOsób db = new BazaDanychOsób();

        dodajPrzykładoweOsoby(db);
        pokażIdentyfikatoryOsób(db);
        podglądBazyDanych(db);

        WriteLine("Osoby:");
        pokażOsoby(db);

        //int idUsuwanejOsoby = 1;
        //WriteLine($"Usuwam osobę nr {idUsuwanejOsoby}");
        //db.UsuńOsobę(idUsuwanejOsoby);
        //podglądBazyDanych(db);

        db[1] = new Osoba()
        {
            Imię = "Jan",
            Nazwisko = "Kowalski",
            Adres = new Adres()
            {
                Miasto = "Toruń",
                Ulica = "Mostowa",
                NumerDomu = 100,
                NumerMieszkania = 20
            }
        };

        podglądBazyDanych(db);
    }

    static void podglądBazyDanych(BazaDanychOsób db)
    {
        try
        {
            WriteLine("Podgląd danych");
            WriteLine("Osoby:");
            foreach (Osoba osoba in db.Osoby)
            {
                WriteLine(osoba);
            }

            WriteLine("Adresy");
            foreach (Adres adres in db.Adresy)
            {
                WriteLine(adres);
            }
        }
        catch (Exception e)
        {
            WriteLine("Błąd podglądu bazy danych " + e.Message);
        }

    }
    static void dodajPrzykładoweOsoby(BazaDanychOsób db)
    {
        Adres a1 = new Adres()
        {
            Id = 1,
            Miasto = "Toruń",
            Ulica = "Grudziądzka",
            NumerDomu = 5,
            NumerMieszkania = 477
        };

        Adres a2 = new Adres()
        {
            Id = 2,
            Miasto = "Toruń",
            Ulica = "Pod Mostem",
            NumerDomu = 5,
        };

        Osoba o1 = new Osoba()
        {
            Imię = "Antoni",
            Nazwisko = "Gburek",
            NumerTelefonu = 123456,
            Adres = a1
        };

        Osoba o2 = new Osoba()
        {
            Imię = "Henryk",
            Nazwisko = "Garncarz",
            NumerTelefonu = 6431579,
            Adres = a2
        };

        Osoba o3 = new Osoba()
        {
            Imię = "Wincenty",
            Nazwisko = "Patyk",
            NumerTelefonu = 463242,
            Adres = a1
        };

        db.DodajOsobę(o1);
        db.DodajOsobę(o2);
        db.DodajOsobę(o3);
    }

    static void pokażIdentyfikatoryOsób(BazaDanychOsób db)
    {
        string s = "Identyfikatory osób: ";
        foreach (int idOsoby in db.identyfikatoryOsób)
        {
            s += idOsoby.ToString() + "; ";
        }
        WriteLine(s.Trim(' ', ';'));
    }

    static void pokażOsoby(BazaDanychOsób db)
    {
        foreach (int idOsoby in db.identyfikatoryOsób)
        {
            WriteLine(db[idOsoby]);
        }
    }
}