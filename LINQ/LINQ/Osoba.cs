using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQ
{
    public class Osoba
    {
        public int Id { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public int NumerTelefonu { get; set; }
        public int Wiek { get; set; }

        public override string ToString() => $"{Id}, {Imie}, {Nazwisko}, {NumerTelefonu}, {Wiek}";
    }
}
