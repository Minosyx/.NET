using System;

namespace CS9
{
    public record Osoba
    {
        public string Imie { get; init; }
        public string Nazwisko { get; init; }
        public int NumerTelefonu { get; set; }

        public Osoba(string Imie, string Nazwisko)
        {
            this.Imie = Imie;
            this.Nazwisko = Nazwisko;
        }
        public override string ToString() => $"{Imie} {Nazwisko}, {NumerTelefonu}";

        //public override bool Equals(object? obj)
        //{
        //    if (obj is Osoba)
        //    {
        //        Osoba _obj = obj as Osoba;
        //        return Imie == _obj.Imie && Nazwisko == _obj.Nazwisko && NumerTelefonu == _obj.NumerTelefonu;
        //    }
        //    else return false;
        //}
    }

    public record Student : Osoba
    {
        public int NumerLegitymacji { get; init; }

        public Student(string imie, string nazwisko, int numerLegitymacji)
            : base(imie, nazwisko)
        {
            NumerLegitymacji = numerLegitymacji;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Osoba o = new Osoba("Mikołaj", "Kopernik")
            {
                //Imie = "Mikołaj",
                //Nazwisko = "Kopernik",
                NumerTelefonu = 123456789
            };
            Console.WriteLine(o);
            //o.Imie = "Katarzyna";
            //o.NumerTelefonu = 987654321;
            //Console.WriteLine(o);

            //Osoba jm = new Osoba()
            //{
            //    Imie = "Jacek",
            //    Nazwisko = "Matulewski",
            //    NumerTelefonu = 987654321
            //};

            //Osoba jm = new Osoba()
            //{
            //    Imie = o.Imie,
            //    Nazwisko = o.Nazwisko,
            //    NumerTelefonu = o.NumerTelefonu
            //};

            Osoba jm = o with {Imie = "Henryk"};
            Console.WriteLine(jm);

            Console.WriteLine(o == jm);
            Console.WriteLine(o.Equals(jm));

            //for (int i = 0; i < 10; i++)
            //{
            //    Random r = new Random();
            //    int n = r.Next(10);
            //    string opis;
            //    switch (n)
            //    {
            //        case 1 or 7:
            //            opis = "weekend";
            //            break;
            //        case > 1 and < 7:
            //            opis = "dzień roboczy";
            //            break;
            //        default:
            //            opis = "błąd";
            //            break;
            //    }
            //    Console.WriteLine(opis);
            //}

            for (int i = 0; i < 10; i++)
            {
                Random r = new Random();
                int n = r.Next(10);
                string opis = n switch
                {
                    1 or 7 => "weekend",
                    > 1 and < 7 => "dzień roboczy",
                    _ => "błąd"
                };
                Console.WriteLine(opis);
            }
        }
    }
}
