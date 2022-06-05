using System.Runtime.CompilerServices;
using System.Text;
using LINQ;

List<Osoba> listaOsob = new List<Osoba>
{
    new Osoba {Id = 1, Imie = "Jacek", Nazwisko = "Matulewski", NumerTelefonu = 123456789, Wiek = 49},
    new Osoba {Id = 2, Imie = "Jan", Nazwisko = "Kowalski", NumerTelefonu = 562311489, Wiek = 22},
    new Osoba {Id = 3, Imie = "Jan1", Nazwisko = "Kowalski", NumerTelefonu = 213154126, Wiek = 14},
    new Osoba {Id = 4, Imie = "Jan2", Nazwisko = "Kowalski", NumerTelefonu = 694134162, Wiek = 32},
    new Osoba {Id = 5, Imie = "Jan3", Nazwisko = "Kowalski", NumerTelefonu = 461973412, Wiek = 26},
    new Osoba {Id = 6, Imie = "Jan", Nazwisko = "Nowak", NumerTelefonu = 562311489, Wiek = 22},
    new Osoba {Id = 7, Imie = "Jan1", Nazwisko = "Nowak", NumerTelefonu = 213154126, Wiek = 14},
    new Osoba {Id = 8, Imie = "Jan2", Nazwisko = "Nowak", NumerTelefonu = 694134162, Wiek = 32},
    new Osoba {Id = 9, Imie = "Jan3", Nazwisko = "Nowak", NumerTelefonu = 461973412, Wiek = 26},
    new Osoba {Id = 10, Imie = "Magda", Nazwisko = "Nowak", NumerTelefonu = 694134162, Wiek = 32},
    new Osoba {Id = 11, Imie = "Daria", Nazwisko = "Nowak", NumerTelefonu = 461973412, Wiek = 26},
    new Osoba {Id = 12, Imie = "Iga", Nazwisko = "Nowak", NumerTelefonu = 694134162, Wiek = 15},
};

string pokazOsoby(IEnumerable<Osoba> listaOsob)
{
    StringBuilder s = new StringBuilder("Lista osób:\n");
    foreach (Osoba osoba in listaOsob)
        s.Append(osoba.ToString() + Environment.NewLine);
    return s.ToString();
}

string s = pokazOsoby(listaOsob);
Console.WriteLine(s);

var listaOsobPelnoletnich = from osoba in listaOsob
                            where osoba.Wiek >= 18
                            orderby osoba.Wiek
                            select osoba;

s = pokazOsoby(listaOsobPelnoletnich.ToList());
Console.WriteLine(s);

//var listaOsobPelnoletnich1 = listaOsob.Where(o => o.Wiek >= 18).OrderBy(o => o.Wiek).Select(o => o.ToString());

//StringBuilder sb = new StringBuilder("Lista osób:\n");
//foreach (string osoba in listaOsobPelnoletnich1)
//    sb.Append(osoba + "\n");

//Console.WriteLine(sb.ToString());

//var listaOsobPelnoletnich2 = from osoba in listaOsob
//                            where osoba.Wiek >= 18
//                            orderby osoba.Wiek
//                            select new Osoba() {Id = osoba.Id, Imie = osoba.Imie, Nazwisko = osoba.Nazwisko, NumerTelefonu = osoba.NumerTelefonu, Wiek = osoba.Wiek};

//s = pokazOsoby(listaOsobPelnoletnich2.ToList());
//Console.WriteLine(s);

//listaOsobPelnoletnich2 = listaOsobPelnoletnich2.ToList();
//listaOsobPelnoletnich2.ElementAt(0).Wiek = 101;

//s = pokazOsoby(listaOsobPelnoletnich2.ToList());
//Console.WriteLine(s);

//Console.WriteLine(listaOsobPelnoletnich.Sum(o => o.Wiek));
//Console.WriteLine(listaOsobPelnoletnich.Aggregate((o, s) => new Osoba {Wiek = o.Wiek + s.Wiek }));

//Console.WriteLine(listaOsobPelnoletnich.Max(o => o.Wiek));
//Console.WriteLine(listaOsobPelnoletnich.First(o => o.Wiek > 25));
//Console.WriteLine(listaOsobPelnoletnich.Single(o => o.Wiek == listaOsobPelnoletnich.Max(o => o.Wiek)));

//Console.WriteLine(listaOsobPelnoletnich.All(o => o.Wiek >= 18));
//Console.WriteLine(listaOsobPelnoletnich.Any(o => o.Wiek > 100));

var grupyOsobNazwisko = from osoba in listaOsob
                        group osoba by osoba.Nazwisko into grupa
                        select grupa;
Console.WriteLine("Grupy:");
foreach (var grupa in grupyOsobNazwisko)
{
    Console.WriteLine("Nazwisko: " + grupa.Key);
    foreach (Osoba osoba in grupa) Console.WriteLine(osoba);
    Console.WriteLine();
}

var listaKobiet = from osoba in listaOsob
                  where osoba.Imie.EndsWith("a")
                  select osoba;

s = pokazOsoby(listaKobiet);

Console.WriteLine(s);

var listaPelnoletnichKobiet = listaKobiet.Concat(listaKobiet).Distinct().Where(k => k.Wiek >= 18);
s = pokazOsoby(listaPelnoletnichKobiet);
Console.WriteLine(s);


var listaTelefonow = from osoba in listaOsob
                     select new {osoba.Id, osoba.NumerTelefonu};
var listaPersonaliow = from osoba in listaOsob
    select new { osoba.Id, osoba.Imie, osoba.Nazwisko };

var lista = from telefon in listaTelefonow
            join personalia in listaPersonaliow
            on telefon.Id equals personalia.Id
            select new {telefon.Id, personalia.Imie, personalia.Nazwisko, telefon.NumerTelefonu};

double[] d = {42,364,2,1,6,95};
Console.WriteLine(d.StdDev());

Console.WriteLine(listaOsob.StdDev(o => o.Wiek));

var result = listaOsob.DiscardThickErrors(o => o.Wiek);
Console.WriteLine(listaOsob.Count);
Console.WriteLine(result.Count());